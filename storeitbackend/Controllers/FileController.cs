#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using storeitbackend.Models;
using storeitbackend.Data;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using storeitbackend.Services;
using System.IO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using storeitbackend.Dtos.File;
using storeitbackend.Dtos.Account;
using storeitbackend.Interfaces;

namespace storeitbackend.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class FileController : ControllerBase
  {
    private readonly ICloudinary _cloudinary;
    private readonly ICloudinaryExtensionService _cloudinaryExtensionService;
    private readonly IAppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IFileService _fileService;
    private readonly IJWTService _jwtService;

    public FileController(ICloudinary cloudinary, ICloudinaryExtensionService cloudinaryExtensionService, IAppDbContext context, IConfiguration configuration, IFileService fileService, IJWTService jwtService)
    {
      _cloudinary = cloudinary;
      _cloudinaryExtensionService = cloudinaryExtensionService;
      _context = context;
      _configuration = configuration;
      _fileService = fileService;
      _jwtService = jwtService;
    }

    [HttpPost("upload")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
      if (file == null)
      {
        return Problem(detail: "File is empty", statusCode: StatusCodes.Status400BadRequest);
      }
      if (file.Length > Convert.ToInt32(_configuration.GetSection("MaxFileSizeInBytes").Value))
      {
        return Problem(detail: "File size must be lower than 5MB", statusCode: StatusCodes.Status400BadRequest);
      }

      string authHeader = Request.Headers.Authorization;

      if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
      {
        return Unauthorized("No valid JWT token found in the request headers.");
      }

      // Extract the raw token by removing the "Bearer " prefix
      string token = authHeader.Substring("Bearer ".Length).Trim();


      string userId = _jwtService.ExtractUserIdFromJwt(token);
      FileTypeResult FileTypeResultObject = _fileService.GetFileType(file.FileName);
      var filesExistsInDb = _context.Files.Where(f => f.Name == Path.GetFileName(file.FileName) && f.Type.Name.ToLower() == FileTypeResultObject.Type.ToLower() && f.Extension.Name.ToLower() == FileTypeResultObject.Extension.ToLower());

      if (filesExistsInDb.Count() > 0)
      {
        foreach (var fileInDb in filesExistsInDb)
        {
          User? owner = await _fileService.GetOwnerByFileId(fileInDb.Id);
          if (owner != null && owner.Id == userId)
          {
            return Problem(detail: "File already exists", statusCode: StatusCodes.Status400BadRequest);
          }
        }
      }

      if (FileTypeResultObject.Type == "image")
      {
        var uploadParams = new ImageUploadParams()
        {
          File = new FileDescription(file.FileName, file.OpenReadStream()),
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error == null)
        {
          var RowsAffected = await _fileService.SaveFileToDb(file, userId, imageUploadResult: result);

          if (RowsAffected > 0)
          {
            return Ok(new { detail = "File uploaded successfully" });
          }
          else if (RowsAffected == 0)
          {
            return Problem(detail: "Uploaded file successfully but error occurred when storing file info in database", statusCode: StatusCodes.Status400BadRequest);
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
      else if (FileTypeResultObject.Type == "video")
      {
        var uploadParams = new VideoUploadParams()
        {
          File = new FileDescription(file.FileName, file.OpenReadStream()),
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error == null)
        {
          var RowsAffected = await _fileService.SaveFileToDb(file, userId, videoUploadResult: result);

          if (RowsAffected > 0)
          {
            return Ok(new { detail = "File uploaded successfully" });
          }
          else if (RowsAffected == 0)
          {
            return Problem(detail: "Uploaded file successfully but error occurred when storing file info in database", statusCode: StatusCodes.Status400BadRequest);
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
      else
      {
        var uploadParams = new RawUploadParams()
        {
          File = new FileDescription(file.FileName, file.OpenReadStream()),
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error == null)
        {
          var RowsAffected = await _fileService.SaveFileToDb(file, userId, rawUploadResult: result);

          if (RowsAffected > 0)
          {
            return Ok(new { detail = "File uploaded successfully" });
          }
          else if (RowsAffected == 0)
          {
            return Problem(detail: "Uploaded file successfully but error occurred when storing file info in database", statusCode: StatusCodes.Status400BadRequest);
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

    }


    [HttpGet("user-files")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<List<UserFileDto>> GetAllFilesForUser()
    {

      string authHeader = Request.Headers.Authorization;

      if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
      {
        return Unauthorized("No valid JWT token found in the request headers.");
      }

      // Extract the raw token by removing the "Bearer " prefix
      string token = authHeader.Substring("Bearer ".Length).Trim();

      string userId = _jwtService.ExtractUserIdFromJwt(token);

      var userFiles = _fileService.GetUserFiles(userId);
      if (userFiles == null)
      {
        return Problem(detail: $"No files for this user with id = {userId}", statusCode: StatusCodes.Status404NotFound);
      }

      return Ok(userFiles);

    }

    [HttpGet("file-users")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<List<UserFileSharedDto>> GetAllUsersForFile([FromQuery] string fileId)
    {
      string authHeader = Request.Headers.Authorization;

      if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
      {
        return Unauthorized("No valid JWT token found in the request headers.");
      }

      var fileUsers = _fileService.GetUsersForFileByFileId(fileId);
      if (fileUsers == null)
      {
        return Problem(detail: $"No users for this file with id = {fileId}", statusCode: StatusCodes.Status404NotFound);
      }

      return Ok(fileUsers);

    }


    [HttpPost("delete")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteAsset([FromBody] FileDeletionBackendDto fileDeletionBackendDto)
    {
      // Validating token
      string authHeader = Request.Headers.Authorization;

      if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
      {
        return Unauthorized("No valid JWT token found in the request headers.");
      }

      string token = authHeader.Substring("Bearer ".Length).Trim();

      string userId = _jwtService.ExtractUserIdFromJwt(token);

      // Get File From Db
      var fileFromDb = await _context.Files.FirstOrDefaultAsync((f) => f.Id == fileDeletionBackendDto.FileId);

      if (fileFromDb == null)
      {
        return Problem(detail: "File is not found", statusCode: StatusCodes.Status404NotFound);
      }

      // Check if user has access to the file
      var userFileFromDb = await _context.UsersFiles.FirstOrDefaultAsync((uf) => (uf.FileId == fileDeletionBackendDto.FileId) && (uf.UserId == userId));

      if (userFileFromDb == null)
      {
        return Problem(detail: "You can not delete this file", statusCode: StatusCodes.Status400BadRequest);
      }

      //Check if user has access to the file
      var ownerFileFromDb = await _context.OwnersFiles.FirstOrDefaultAsync((of) => (of.FileId == fileDeletionBackendDto.FileId) && (of.UserId == userId));

      if (ownerFileFromDb == null)
      {
        return Problem(detail: "You can not delete this file", statusCode: StatusCodes.Status400BadRequest);
      }

      // Extracting public Id and resource type
      string publicIdWithoutExtension = _fileService.ExtractPublicId(fileDeletionBackendDto.FileUrl);

      if (publicIdWithoutExtension == null)
      {
        return Problem(detail: "Invalid url", statusCode: StatusCodes.Status400BadRequest);
      }

      string? fileExtensionFromDb = await _fileService.GetFileExtensionNameByFileExtensionId(fileFromDb.ExtensionId);

      if (fileExtensionFromDb == null)
      {
        return Problem(detail: "Extension is not found", statusCode: StatusCodes.Status400BadRequest);
      }


      FileTypeResult FileTypeResultObject = _fileService.GetFileType(fileFromDb.Name);
      ResourceType resourceType = ResourceType.Auto;
      string publicId = "";

      if (FileTypeResultObject.Type == "image")
      {
        publicId = publicIdWithoutExtension;
        resourceType = ResourceType.Image;
      }
      else if (FileTypeResultObject.Type == "video")
      {
        publicId = $"{publicIdWithoutExtension}.{fileExtensionFromDb}";
        resourceType = ResourceType.Video;
      }
      else
      {
        publicId = $"{publicIdWithoutExtension}.{fileExtensionFromDb}";
        resourceType = ResourceType.Raw;
      }

      // Search for the asset
      SearchResult searchResult = _cloudinaryExtensionService.ExecuteSearch($"public_id:{publicId}");

      if (searchResult.Resources.Count <= 0)
      {
        return Problem(detail: "File is not found in storage", statusCode: StatusCodes.Status404NotFound);
      }

      // Delete from storage
      var deletionParams = new DeletionParams(publicId)
      {
        // purging cached versions from the CDN.
        Invalidate = true,
        ResourceType = resourceType,
        PublicId = publicId,
      };

      var result = await _cloudinary.DestroyAsync(deletionParams);

      if (result.Error != null)
      {
        return Problem(detail: result.Error.Message, statusCode: StatusCodes.Status400BadRequest);
      }

      // Delete from Db
      if (!await _fileService.DeleteFileFromDb(fileFromDb, ownerFileFromDb, userFileFromDb))
      {
        return Problem(detail: "Unknown error occurred", statusCode: StatusCodes.Status400BadRequest);
      }

      return Ok(new { detail = "File deleted successfully" });

    }


    [HttpPost("rename")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RenameAsset([FromBody] FileRenameDto fileRenameDto)
    {
      string authHeader = Request.Headers.Authorization;

      if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
      {
        return Unauthorized("No valid JWT token found in the request headers.");
      }

      // Extract the raw token by removing the "Bearer " prefix
      string token = authHeader.Substring("Bearer ".Length).Trim();

      string userId = _jwtService.ExtractUserIdFromJwt(token);

      var fileFromDb = await _context.Files.FirstOrDefaultAsync((f) => f.Id == fileRenameDto.FileId);

      if (fileFromDb == null)
      {
        return Problem(detail: "File is not found", statusCode: StatusCodes.Status404NotFound);
      }

      var ownerFileFromDb = await _context.OwnersFiles.FirstOrDefaultAsync((owf) => (owf.FileId == fileRenameDto.FileId) && (owf.UserId == userId));

      if (ownerFileFromDb == null)
      {
        return Problem(detail: "You can not rename this file", statusCode: StatusCodes.Status400BadRequest);
      }

      try
      {
        fileFromDb.Name = fileRenameDto.FileName;
        await _context.SaveChangesAsync();
      }
      catch
      {
        return Problem(detail: "Unknown error occurred", statusCode: StatusCodes.Status400BadRequest);
      }

      return Ok(new { detail = "File renamed successfully" });

    }


    [HttpPost("share")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ShareAsset([FromBody] FileShareDto fileShareDto)
    {
      string authHeader = Request.Headers.Authorization;

      if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
      {
        return Unauthorized("No valid JWT token found in the request headers.");
      }

      // Extract the raw token by removing the "Bearer " prefix
      string token = authHeader.Substring("Bearer ".Length).Trim();

      string userId = _jwtService.ExtractUserIdFromJwt(token);

      var fileFromDb = await _context.Files.FirstOrDefaultAsync((f) => f.Id == fileShareDto.FileId);

      if (fileFromDb == null)
      {
        return Problem(detail: "File is not found", statusCode: StatusCodes.Status404NotFound);
      }

      var userFileFromDb = await _context.UsersFiles.FirstOrDefaultAsync((uf) => (uf.FileId == fileShareDto.FileId) && (uf.UserId == userId));

      if (userFileFromDb == null)
      {
        return Problem(detail: "You can not share this file", statusCode: StatusCodes.Status400BadRequest);
      }

      List<User> usersOfEmails = [];

      foreach (var email in fileShareDto.Emails)
      {
        var userFromEmails = await _context.Users.FirstOrDefaultAsync((u) => u.Email == email);
        if (userFromEmails != null)
        {
          usersOfEmails.Add(userFromEmails);
        }
      }

      if (usersOfEmails.Count() == 0)
      {
        return Problem(detail: "No valid emails were given", statusCode: StatusCodes.Status400BadRequest);
      }

      try
      {

        foreach (var validUser in usersOfEmails)
        {
          var RecordAlreadyExists = await _context.UsersFiles.FirstOrDefaultAsync((uf) => uf.UserId == validUser.Id && uf.FileId == fileShareDto.FileId);
          if (RecordAlreadyExists == null)
          {
            _context.UsersFiles.Add(new UserFile
            {
              FileId = fileShareDto.FileId,
              UserId = validUser.Id,
            });
          }

        }

        await _context.SaveChangesAsync();

      }
      catch
      {
        return Problem(detail: "Unknown error occurred", statusCode: StatusCodes.Status400BadRequest);
      }

      return Ok(new { detail = "File shared successfully" });

    }


    [HttpPost("share-remove")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ShareRemoveAsset([FromBody] FileShareRemoveDto fileShareRemoveDto)
    {
      string authHeader = Request.Headers.Authorization;

      if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
      {
        return Unauthorized("No valid JWT token found in the request headers.");
      }

      // Extract the raw token by removing the "Bearer " prefix
      string token = authHeader.Substring("Bearer ".Length).Trim();

      string userId = _jwtService.ExtractUserIdFromJwt(token);

      var fileFromDb = await _context.Files.FirstOrDefaultAsync((f) => f.Id == fileShareRemoveDto.FileId);

      if (fileFromDb == null)
      {
        return Problem(detail: "File is not found", statusCode: StatusCodes.Status404NotFound);
      }

      var userFileFromDb = await _context.UsersFiles.FirstOrDefaultAsync((uf) => (uf.FileId == fileShareRemoveDto.FileId) && (uf.UserId == userId));

      if (userFileFromDb == null)
      {
        return Problem(detail: "You can not remove this sharing", statusCode: StatusCodes.Status400BadRequest);
      }

      var userOfEmailExists = await _context.Users.FirstOrDefaultAsync((u) => u.Email == fileShareRemoveDto.UserEmailToRemove);
      if (userOfEmailExists == null)
      {
        return Problem(detail: "User is not found", statusCode: StatusCodes.Status404NotFound);
      }

      var userFileToRemove = await _context.UsersFiles.FirstOrDefaultAsync((uf) => (uf.FileId == fileShareRemoveDto.FileId) && (uf.UserId == userOfEmailExists.Id));

      if (userFileToRemove == null)
      {
        return Problem(detail: "User is not sharing this file", statusCode: StatusCodes.Status404NotFound);
      }

      try
      {
        _context.UsersFiles.Remove(userFileToRemove);

        await _context.SaveChangesAsync();

      }
      catch
      {
        return Problem(detail: "Unknown error occurred", statusCode: StatusCodes.Status400BadRequest);
      }

      return Ok(new { detail = "User removed from shared for file successfully" });

    }


  }
}