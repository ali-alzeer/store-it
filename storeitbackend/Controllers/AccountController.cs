#nullable disable

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using storeitbackend.Data;
using storeitbackend.Dtos.Account;
using storeitbackend.Interfaces;
using storeitbackend.Models;
using storeitbackend.Services;

namespace storeitbackend.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AccountController : ControllerBase
  {
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJWTService _jwtService;
    private readonly IFileService _fileService;
    private readonly AppDbContext _context;
    private readonly Cloudinary _cloudinary;
    public AccountController(Cloudinary cloudinary, AppDbContext context, IJWTService jwtService, IFileService fileService, SignInManager<User> signInManager, UserManager<User> userManager)
    {
      _jwtService = jwtService;
      _fileService = fileService;
      _userManager = userManager;
      _signInManager = signInManager;
      _context = context;
      _cloudinary = cloudinary;
    }

    [HttpPost("sign-in")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDto>> SignIn(SignInDto signInDto)
    {
      var SignInFailedResult = Problem(detail: "Invalid email or password", statusCode: StatusCodes.Status401Unauthorized);

      bool notMatched = Regex.IsMatch(signInDto.Email, "^\\w+@[a-zA-Z0-9-]+\\.[a-zA-Z]{2,}$");
      if (!notMatched) return SignInFailedResult;
      User user = await _userManager.FindByEmailAsync(signInDto.Email);
      if (user == null) return SignInFailedResult;
      var result = await _signInManager.CheckPasswordSignInAsync(user, signInDto.Password, false);
      if (!result.Succeeded) return SignInFailedResult;

      return Ok(CreateUserDto(user));
    }

    [HttpPost("sign-up")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SignUp(SignUpDto signUpDto)
    {

      if (await CheckEmailExistsAsync(signUpDto.Email))
      {
        return Problem(detail: "Email already exists", statusCode: StatusCodes.Status400BadRequest);
      }

      var user = new User
      {
        FirstName = signUpDto.FirstName.ToLower(),
        LastName = signUpDto.LastName.ToLower(),
        Email = signUpDto.Email.ToLower(),
        UserName = signUpDto.Email.ToLower()
      };

      var result = await _userManager.CreateAsync(user, signUpDto.Password);

      if (!result.Succeeded) return Problem(detail: "Unknown error occurred", statusCode: StatusCodes.Status400BadRequest, extensions: ConvertErrorsToDictionary(result.Errors));

      return Ok(new { detail = "Account created successfully" });
    }

    [HttpPost("validate-jwt")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ValidateJwt([FromBody] JwtDto jwtDto)
    {
      if (string.IsNullOrEmpty(jwtDto.Token))
      {
        return Problem(detail: "Token is empty", statusCode: StatusCodes.Status400BadRequest);
      }

      TokenValidation TokenValidationObject = _jwtService.ValidateJwt(jwtDto.Token);
      if (!TokenValidationObject.isValid)
      {
        if (TokenValidationObject.tokenValidationErrorType == TokenValidationErrorType.Expired)
        {
          return Problem(detail: "Token is expired", statusCode: StatusCodes.Status400BadRequest, extensions: ConvertTokenErrorsToDictionary(TokenValidationObject));
        }
        else if (TokenValidationObject.tokenValidationErrorType == TokenValidationErrorType.Invalid)
        {
          return Problem(detail: "Token is Invalid", statusCode: StatusCodes.Status400BadRequest, extensions: ConvertTokenErrorsToDictionary(TokenValidationObject));
        }
      }

      return Ok(TokenValidationObject);
    }


    [HttpPost("image-change")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDto>> ChangeImage(IFormFile image)
    {
      string authHeader = Request.Headers.Authorization;

      if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
      {
        return Unauthorized("No valid JWT token found in the request headers.");
      }

      // Extract the raw token by removing the "Bearer " prefix
      string token = authHeader.Substring("Bearer ".Length).Trim();

      string userId = _jwtService.ExtractUserIdFromJwt(token);

      User? user = await _userManager.FindByIdAsync(userId);

      if (user == null) return Problem(detail: $"No user found with id = {userId}", statusCode: StatusCodes.Status404NotFound);

      if (image.Length == 0) return Problem(detail: "Image file is empty", statusCode: StatusCodes.Status400BadRequest);

      if (_fileService.GetFileType(image.FileName).Type != "image") return Problem(detail: "Unsupported media type", statusCode: StatusCodes.Status415UnsupportedMediaType);

      if (user.ImageUrl != null)
      {

        //Remove image before uploading new one
        string publicIdWithoutExtension = _fileService.ExtractPublicId(user.ImageUrl);

        var deletionParams = new DeletionParams(publicIdWithoutExtension)
        {
          Invalidate = true,
          PublicId = publicIdWithoutExtension,
          ResourceType = ResourceType.Image,
        };


        var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

        if (deletionResult.Error != null)
        {
          return Problem(detail: deletionResult.Error.Message, statusCode: StatusCodes.Status400BadRequest);
        }
      }

      var uploadParams = new ImageUploadParams()
      {
        File = new FileDescription(image.FileName, image.OpenReadStream()),
      };

      var result = await _cloudinary.UploadAsync(uploadParams);

      if (result.Error == null)
      {
        user.ImageUrl = result.Url.ToString();
        var RowsAffected = await _context.SaveChangesAsync();

        if (RowsAffected > 0)
        {
          return Ok(CreateUserDto(user));
        }
        else if (RowsAffected == 0)
        {
          return Problem(detail: "Uploaded image successfully but error occurred when saving info in database", statusCode: StatusCodes.Status400BadRequest);
        }
        else
        {
          return Problem(detail: "Upload result in null", statusCode: StatusCodes.Status400BadRequest);
        }
      }
      else
      {
        return Problem(detail: result.Error.Message, statusCode: StatusCodes.Status400BadRequest);
      }




    }

    [HttpPost("image-delete")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDto>> DeleteImage(ImageDeleteDto imageDeleteDto)
    {
      string authHeader = Request.Headers.Authorization;

      if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
      {
        return Unauthorized("No valid JWT token found in the request headers.");
      }

      // Extract the raw token by removing the "Bearer " prefix
      string token = authHeader.Substring("Bearer ".Length).Trim();

      string userId = _jwtService.ExtractUserIdFromJwt(token);

      User? user = await _userManager.FindByIdAsync(userId);

      if (user == null) return Problem(detail: $"No user found with id = {userId}", statusCode: StatusCodes.Status404NotFound);

      if (user.ImageUrl == null) return Problem(detail: "User does have an image to delete", statusCode: StatusCodes.Status400BadRequest);

      if (user.ImageUrl.ToLower() != imageDeleteDto.ImageUrl.ToLower()) return Problem(detail: "ImageUrl does not match user imageUrl", statusCode: StatusCodes.Status400BadRequest);

      //Remove image before uploading new one
      string publicIdWithoutExtension = _fileService.ExtractPublicId(user.ImageUrl);

      var deletionParams = new DeletionParams(publicIdWithoutExtension)
      {
        Invalidate = true,
        PublicId = publicIdWithoutExtension,
        ResourceType = ResourceType.Image,
      };


      var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

      if (deletionResult.Error != null)
      {
        return Problem(detail: deletionResult.Error.Message, statusCode: StatusCodes.Status400BadRequest);
      }

      user.ImageUrl = null;
      int RowsAffected = await _context.SaveChangesAsync();

      if (RowsAffected == 0)
      {
        return Problem(detail: "Unknown error occurred while deleting image from database", statusCode: StatusCodes.Status400BadRequest);
      }

      return Ok(CreateUserDto(user));

    }

    [HttpPost("name-change")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDto>> ChangeName(NameChangeDto nameChangeDto)
    {
      string authHeader = Request.Headers.Authorization;

      if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
      {
        return Unauthorized("No valid JWT token found in the request headers.");
      }

      // Extract the raw token by removing the "Bearer " prefix
      string token = authHeader.Substring("Bearer ".Length).Trim();

      string userId = _jwtService.ExtractUserIdFromJwt(token);

      User? user = await _userManager.FindByIdAsync(userId);

      if (user == null) return Problem(detail: $"No user found with id = {userId}", statusCode: StatusCodes.Status404NotFound);

      if (nameChangeDto.FirstName == "" || nameChangeDto.FirstName == null || nameChangeDto.LastName == "" || nameChangeDto.LastName == null) return Problem(detail: "First name and last name fields are required", statusCode: StatusCodes.Status400BadRequest);

      user.FirstName = nameChangeDto.FirstName;
      user.LastName = nameChangeDto.LastName;
      int RowsAffected = await _context.SaveChangesAsync();

      if (RowsAffected == 0)
      {
        return Problem(detail: "Unknown error occurred while deleting image from database", statusCode: StatusCodes.Status400BadRequest);
      }

      return Ok(CreateUserDto(user));

    }


    [HttpDelete("account-delete")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteAccount()
    {
      string authHeader = Request.Headers.Authorization;

      if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
      {
        return Unauthorized("No valid JWT token found in the request headers.");
      }

      // Extract the raw token by removing the "Bearer " prefix
      string token = authHeader.Substring("Bearer ".Length).Trim();

      string userId = _jwtService.ExtractUserIdFromJwt(token);

      User? user = await _userManager.FindByIdAsync(userId);

      if (user == null) return Problem(detail: $"No user found with id = {userId}", statusCode: StatusCodes.Status404NotFound);

      // Get all files for this user
      List<Models.OwnerFile> ownerfilesToDelete = await _context.OwnersFiles.Where(owf => owf.UserId == user.Id).ToListAsync();

      if (ownerfilesToDelete.Count == 0)
      {
        return await DeleteUserFromDb(user);
      }
      else
      {
        // Extract public ids for all files
        Models.File[] filesToDelete = new Models.File[ownerfilesToDelete.Count];
        string[] publicIds = new string[ownerfilesToDelete.Count];
        bool[] deletionResults = new bool[ownerfilesToDelete.Count];

        // Make public ids of 100 for each array
        for (int i = 0; i < ownerfilesToDelete.Count; i++)
        {
          filesToDelete[i] = await _context.Files.FirstOrDefaultAsync(f => f.Id == ownerfilesToDelete[i].FileId);
          if (filesToDelete[i] != null)
          {
            ResourceType resourceType = ResourceType.Auto;
            var fileTypeExtension = _fileService.GetFileType(filesToDelete[i].Name);
            if (fileTypeExtension.Type == "image")
            {
              publicIds[i] = _fileService.ExtractPublicId(filesToDelete[i].Url);
              resourceType = ResourceType.Image;
            }
            else
            {
              publicIds[i] = $"{_fileService.ExtractPublicId(filesToDelete[i].Url)}.{fileTypeExtension.Extension}";
              if (fileTypeExtension.Type == "video")
              {
                resourceType = ResourceType.Video;

              }
              else
              {
                resourceType = ResourceType.Raw;

              }
            }

            // Search for the asset
            var search = _cloudinary.Search();
            SearchResult searchResult;
            searchResult = search
                  .Expression($"public_id:{publicIds[i]}")
                  .Execute();

            if (searchResult.Resources.Count <= 0)
            {
              deletionResults[i] = false;
            }
            else
            {

              // Delete from storage
              var deletionParams = new DeletionParams(publicIds[i])
              {
                // purging cached versions from the CDN.
                Invalidate = true,
                ResourceType = resourceType,
                PublicId = publicIds[i],
              };

              var result = await _cloudinary.DestroyAsync(deletionParams);

              if (result.Error != null)
              {
                deletionResults[i] = false;
              }

              deletionResults[i] = true;

            }


          }
          else
          {
            publicIds[i] = "";
          }
        }

        if (deletionResults.All(r => r == false))
        {
          return Problem(detail: "Failed to delete files", statusCode: StatusCodes.Status400BadRequest);
        }

        if (user.ImageUrl != null)
        {
          var userImageDeletionParams = new DeletionParams(_fileService.ExtractPublicId(user.ImageUrl))
          {
            // purging cached versions from the CDN.
            Invalidate = true,
            ResourceType = ResourceType.Image,
            PublicId = _fileService.ExtractPublicId(user.ImageUrl),
          };

          await _cloudinary.DestroyAsync(userImageDeletionParams);
        }

        try
        {
          // Delete files from db
          _context.Files.RemoveRange(filesToDelete);
        }
        catch
        {
          return Problem(detail: "Failed to delete files", statusCode: StatusCodes.Status400BadRequest);
        }

        // Delete user from db 
        return await DeleteUserFromDb(user);

      }
    }


    #region Private Helper Methods
    private UserDto CreateUserDto(User user)
    {
      return new UserDto
      {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        CreatedAt = user.CreatedAt,
        Email = user.Email,
        ImageUrl = user.ImageUrl,
        JWT = _jwtService.CreateJWT(user),
      };
    }

    private async Task<bool> CheckEmailExistsAsync(string email)
    {
      return await _userManager.FindByEmailAsync(email) != null;
    }

    private IDictionary<string, object?> ConvertErrorsToDictionary(IEnumerable<IdentityError> errors)
    {
      var ErrorsValues = new string[errors.Count()];
      for (int i = 0; i < errors.Count(); i++)
      {
        ErrorsValues[i] = errors.ToArray()[i].Description;
      }
      var ErrorsDictionary = new Dictionary<string, object?>()
      {
        { "errors", ErrorsValues }
      };
      return ErrorsDictionary;
    }

    private IDictionary<string, object?> ConvertTokenErrorsToDictionary(TokenValidation tokenValidationObject)
    {
      var ErrorsDictionary = new Dictionary<string, object?>()
      {
        { "tokenValidation",  tokenValidationObject}
      };
      return ErrorsDictionary;
    }

    private async Task<ActionResult> DeleteUserFromDb(User user)
    {

      try
      {
        await _userManager.DeleteAsync(user);
      }
      catch
      {
        return Problem(detail: "Failed to delete user", statusCode: StatusCodes.Status400BadRequest);
      }

      return Ok(new { detail = "User deleted successfully" });
    }

    #endregion
  };

}