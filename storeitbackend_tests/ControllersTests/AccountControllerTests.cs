using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using storeitbackend.Controllers;
using storeitbackend.Data;
using storeitbackend.Dtos.Account;
using storeitbackend.Interfaces;
using storeitbackend.Models;
using storeitbackend.Services;


namespace storeitbackend_tests.ControllersTests
{
  public class AccountControllerTests
  {
    private readonly AccountController _controller;
    private readonly Mock<ICloudinary> _cloudinaryMock;
    private readonly Mock<ICloudinaryExtensionService> _cloudinaryExtensionServiceMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly Mock<IAppDbContext> _dbContextMock;
    private readonly Mock<IJWTService> _jwtServiceMock;
    private readonly Mock<IFileService> _fileServiceMock;

    public AccountControllerTests()
    {
      var fakeAccount = new Account("demoCloud", "demoKey", "demoSecret");
      _cloudinaryMock = new Mock<ICloudinary>();
      _cloudinaryExtensionServiceMock = new Mock<ICloudinaryExtensionService>();

      var userStoreMock = new Mock<IUserStore<User>>();
      _userManagerMock = new Mock<UserManager<User>>(
        userStoreMock.Object,
        Options.Create(new IdentityOptions()),
        new PasswordHasher<User>(),
        Array.Empty<IUserValidator<User>>(),
        Array.Empty<IPasswordValidator<User>>(),
        new UpperInvariantLookupNormalizer(),
        new IdentityErrorDescriber(),
        null, null
      );

      var contextAccessor = new Mock<IHttpContextAccessor>();
      var userClaimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
      _signInManagerMock = new Mock<SignInManager<User>>(
        _userManagerMock.Object,
        contextAccessor.Object,
        userClaimsFactory.Object,
        null, null, null, null
      );

      var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(Guid.NewGuid().ToString())
    .Options;

      _dbContextMock = new Mock<IAppDbContext>();
      var configMock = new Mock<IConfiguration>();

      configMock.Setup(c => c["JWT:Key"])
                .Returns("super-secret-test-key");
      configMock.Setup(c => c["JWT:Issuer"])
                .Returns("TestIssuer");
      configMock.Setup(c => c["JWT:ExpiresInDays"])
                .Returns("15");


      _jwtServiceMock = new Mock<IJWTService>();
      _fileServiceMock = new Mock<IFileService>();

      _controller = new AccountController(
        _cloudinaryMock.Object,
        _cloudinaryExtensionServiceMock.Object,
        _dbContextMock.Object,
        _jwtServiceMock.Object,
        _fileServiceMock.Object,
        _signInManagerMock.Object,
        _userManagerMock.Object
      );
    }

    [Fact]
    public async Task SignIn_ReturnsOk_WithUserDto()
    {
      var dto = new SignInDto { Email = "alialzeer@example.com", Password = "very-strong-passsword" };
      var fakeUser = new User { FirstName = "Ali", LastName = "Alzeer", Email = "alialzeer@example.com" };

      _jwtServiceMock.Setup(j => j.CreateJWT(fakeUser)).Returns("user-jwt");

      _userManagerMock
        .Setup(m => m.FindByEmailAsync(It.Is<string>(email => email == dto.Email)))
        .ReturnsAsync(fakeUser);

      _signInManagerMock
        .Setup(m => m.CheckPasswordSignInAsync(
            It.Is<User>(u => u.Email == fakeUser.Email),
            It.Is<string>(pwd => pwd == dto.Password),
            false))
        .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

      var actionResult = await _controller.SignIn(dto);

      var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

      var userDto = Assert.IsType<UserDto>(okResult.Value);
      Console.WriteLine("Signed in as: " + userDto.Email);
      Assert.Equal("Ali", userDto.FirstName);
      Assert.Equal(dto.Email, userDto.Email);

    }

    [Theory]
    [InlineData("bad-email", "irrelevant")]
    [InlineData("unknown@foo.com", "irrelevant")]
    [InlineData("alialzeer@example.com", "wrongpass")]
    public async Task SignIn_Returns401_ForInvalidCredentials(
      string email, string password)
    {
      var dto = new SignInDto { Email = email, Password = password };

      if (Regex.IsMatch(email, "^\\w+@[a-zA-Z0-9-]+\\.[a-zA-Z]{2,}$"))
      {
        _userManagerMock
          .Setup(m => m.FindByEmailAsync(email))
          .ReturnsAsync(null as User);
      }

      if (email == "alialzeer@example.com")
      {
        var fakeUser = new User { Email = email };
        _userManagerMock
          .Setup(m => m.FindByEmailAsync(email))
          .ReturnsAsync(fakeUser);
        _signInManagerMock
          .Setup(m => m.CheckPasswordSignInAsync(fakeUser, password, false))
          .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);
      }

      var actionResult = await _controller.SignIn(dto);

      var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
      Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);
      var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
      Assert.Equal("Invalid email or password", problem.Detail);
    }


    [Theory]
    [InlineData("alialzeer@example.com")]
    public async Task SignUp_ReturnsOk_WithValidUniqueEmail(string email)
    {
      var dto = new SignUpDto { Email = email, Password = "good-password", FirstName = "Ali", LastName = "Alzeer" };
      var fakeUser = new User { Email = email, UserName = email, FirstName = dto.FirstName, LastName = dto.LastName };

      _userManagerMock
        .Setup(m => m.FindByEmailAsync(email))
        .ReturnsAsync(null as User);

      _userManagerMock
        .Setup(m => m.CreateAsync(
            It.Is<User>(u => u.Email == email.ToLower()),
            dto.Password))
        .ReturnsAsync(IdentityResult.Success);

      var actionResult = await _controller.SignUp(dto);

      var objectResult = Assert.IsType<OkObjectResult>(actionResult);
      Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
    }

    [Theory]
    [InlineData("alialzeer@example.com")]
    public async Task SignUp_Returns400_WithExistingEmail(string email)
    {
      var dto = new SignUpDto { Email = email, Password = "good-password", FirstName = "Ali", LastName = "Alzeer" };
      var fakeUser = new User { Email = email, UserName = email, FirstName = dto.FirstName, LastName = dto.LastName };

      _userManagerMock
        .Setup(m => m.FindByEmailAsync(email))
        .ReturnsAsync(fakeUser);

      _userManagerMock
        .Setup(m => m.CreateAsync(
            It.Is<User>(u => u.Email == email.ToLower()),
            dto.Password))
        .ReturnsAsync(IdentityResult.Success);

      var actionResult = await _controller.SignUp(dto);

      var objectResult = Assert.IsType<ObjectResult>(actionResult);
      Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
    }


    [Fact]
    public void ValidateJwt_ReturnsOk_WithValidToken()
    {
      var token = "valid-token";
      var fakeTokenValidationObject = new TokenValidation { isValid = true, tokenValidationErrorType = null };

      _jwtServiceMock.Setup(m => m.ValidateJwt(token)).Returns(fakeTokenValidationObject);

      var actionResult = _controller.ValidateJwt(new JwtDto { Token = token });

      var objectResult = Assert.IsType<OkObjectResult>(actionResult);
      Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-token")]
    public void ValidateJwt_Returns400_WithInvalidOrEmptyToken(string token)
    {
      var fakeTokenValidationObject = new TokenValidation { isValid = false, tokenValidationErrorType = TokenValidationErrorType.Invalid };

      _jwtServiceMock.Setup(m => m.ValidateJwt(token)).Returns(fakeTokenValidationObject);

      var actionResult = _controller.ValidateJwt(new JwtDto { Token = token });

      var objectResult = Assert.IsType<ObjectResult>(actionResult);
      Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
    }


    [Fact]
    public async Task ChangeImage_ReturnsOk_ForValidRequest()
    {
      const string jwtToken = "valid.jwt.token";
      const string userId = "user-1";
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {jwtToken}";

      _jwtServiceMock
        .Setup(s => s.ExtractUserIdFromJwt(jwtToken))
        .Returns(userId);

      var user = new User { Id = userId, Email = "test@example.com", ImageUrl = null };
      _userManagerMock
        .Setup(m => m.FindByIdAsync(userId))
        .ReturnsAsync(user);

      var stream = new MemoryStream(new byte[] { 1, 2, 3 });
      var formFile = new FormFile(stream, 0, stream.Length, "img", "my-image.png")
      {
        Headers = new HeaderDictionary(),
        ContentType = "image/png"
      };

      _fileServiceMock
        .Setup(f => f.GetFileType("my-image.png"))
        .Returns(new FileTypeResult { Extension = "png", Type = "image" });

      var cloudResult = new ImageUploadResult
      {
        Url = new Uri("http://cloudinary.com/my-image"),
        Error = null
      };

      _jwtServiceMock
        .Setup(s => s.CreateJWT(user))
        .Returns("valid.jwt.token");

      _cloudinaryMock
        .Setup(c => c.UploadAsync(It.IsAny<ImageUploadParams>(), default))
        .ReturnsAsync(cloudResult);

      _dbContextMock
        .Setup(c => c.SaveChangesAsync(default))
        .ReturnsAsync(1);

      var result = await _controller.ChangeImage(formFile);

      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
      var dto = Assert.IsType<UserDto>(okResult.Value);
      Assert.Equal("http://cloudinary.com/my-image", dto.ImageUrl);

      _cloudinaryMock.Verify(c => c.UploadAsync(It.IsAny<ImageUploadParams>(), default), Times.Once);
      _dbContextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task ChangeImage_Returns401_ForUnauthorizedRequest()
    {
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };

      var stream = new MemoryStream(new byte[] { 1, 2, 3 });
      var formFile = new FormFile(stream, 0, stream.Length, "img", "my-image.png")
      {
        Headers = new HeaderDictionary(),
        ContentType = "image/png"
      };

      var result = await _controller.ChangeImage(formFile);

      var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
      Assert.Equal(StatusCodes.Status401Unauthorized, unauthorizedResult.StatusCode);
    }

    [Fact]
    public async Task ChangeImage_Returns415_ForUnsupportedFileType()
    {
      const string jwtToken = "valid.jwt.token";
      const string userId = "user-1";
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {jwtToken}";

      _jwtServiceMock
        .Setup(s => s.ExtractUserIdFromJwt(jwtToken))
        .Returns(userId);

      var user = new User { Id = userId, Email = "test@example.com", ImageUrl = null };
      _userManagerMock
        .Setup(m => m.FindByIdAsync(userId))
        .ReturnsAsync(user);

      var stream = new MemoryStream(new byte[] { 1, 2, 3 });
      var formFile = new FormFile(stream, 0, stream.Length, "pdfFile", "notImageFile.pdf")
      {
        Headers = new HeaderDictionary(),
        ContentType = "application/pdf"
      };

      _fileServiceMock
        .Setup(f => f.GetFileType("notImageFile.pdf"))
        .Returns(new FileTypeResult { Extension = "pdf", Type = "document" });

      var result = await _controller.ChangeImage(formFile);

      var unsupportedMediaTypeResult = Assert.IsType<ObjectResult>(result.Result);
      Assert.Equal(StatusCodes.Status415UnsupportedMediaType, unsupportedMediaTypeResult.StatusCode);
    }

    [Fact]
    public async Task ChangeImage_Returns400_ForEmptyImageFile()
    {
      const string jwtToken = "valid.jwt.token";
      const string userId = "user-1";
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {jwtToken}";

      _jwtServiceMock
        .Setup(s => s.ExtractUserIdFromJwt(jwtToken))
        .Returns(userId);

      var user = new User { Id = userId, Email = "test@example.com", ImageUrl = null };
      _userManagerMock
        .Setup(m => m.FindByIdAsync(userId))
        .ReturnsAsync(user);

      var stream = new MemoryStream(new byte[] { 1, 2, 3 });
      var formFile = new FormFile(stream, 0, 0, "empty", "emptyImage.png")
      {
        Headers = new HeaderDictionary(),
        ContentType = "image/png"
      };

      _fileServiceMock
        .Setup(f => f.GetFileType("emptyImage.png"))
        .Returns(new FileTypeResult { Extension = "png", Type = "image" });

      var result = await _controller.ChangeImage(formFile);

      var badRequestResult = Assert.IsType<ObjectResult>(result.Result);
      Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }


    [Fact]
    public async Task DeleteImage_ReturnsOk_ForValidRequest()
    {
      const string jwtToken = "valid.jwt.token";
      const string userId = "user-1";
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {jwtToken}";

      _jwtServiceMock
        .Setup(s => s.ExtractUserIdFromJwt(jwtToken))
        .Returns(userId);

      var user = new User { Id = userId, Email = "test@example.com", ImageUrl = "testurl" };
      _userManagerMock
        .Setup(m => m.FindByIdAsync(userId))
        .ReturnsAsync(user);

      var stream = new MemoryStream(new byte[] { 1, 2, 3 });
      var formFile = new FormFile(stream, 0, stream.Length, "img", "my-image.png")
      {
        Headers = new HeaderDictionary(),
        ContentType = "image/png"
      };

      var cloudResult = new DeletionResult
      {
        Error = null
      };

      _fileServiceMock
        .Setup(s => s.ExtractPublicId(user.ImageUrl))
        .Returns("publicid-123");

      _jwtServiceMock
        .Setup(s => s.CreateJWT(user))
        .Returns("valid.jwt.token");

      _cloudinaryMock
        .Setup(c => c.DestroyAsync(It.IsAny<DeletionParams>()))
        .ReturnsAsync(cloudResult);

      _dbContextMock
        .Setup(c => c.SaveChangesAsync(default))
        .ReturnsAsync(1);

      var result = await _controller.DeleteImage(new ImageDeleteDto { ImageUrl = "testurl" });

      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

      _cloudinaryMock.Verify(c => c.DestroyAsync(It.IsAny<DeletionParams>()), Times.Once);
      _dbContextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteImage_Returns401_ForUnauthorizedRequest()
    {
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };

      var stream = new MemoryStream(new byte[] { 1, 2, 3 });
      var formFile = new FormFile(stream, 0, stream.Length, "img", "my-image.png")
      {
        Headers = new HeaderDictionary(),
        ContentType = "image/png"
      };

      var result = await _controller.DeleteImage(new ImageDeleteDto { ImageUrl = "testurl" });

      var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
      Assert.Equal(StatusCodes.Status401Unauthorized, unauthorizedResult.StatusCode);
    }


    [Fact]
    public async Task DeleteImage_Returns400_GivenImageIsNotUserImage()
    {
      const string jwtToken = "valid.jwt.token";
      const string userId = "user-1";
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {jwtToken}";

      _jwtServiceMock
        .Setup(s => s.ExtractUserIdFromJwt(jwtToken))
        .Returns(userId);

      var user = new User { Id = userId, Email = "test@example.com", ImageUrl = "testurl" };
      _userManagerMock
        .Setup(m => m.FindByIdAsync(userId))
        .ReturnsAsync(user);

      var stream = new MemoryStream(new byte[] { 1, 2, 3 });
      var formFile = new FormFile(stream, 0, stream.Length, "img", "my-image.png")
      {
        Headers = new HeaderDictionary(),
        ContentType = "image/png"
      };

      var cloudResult = new DeletionResult
      {
        Error = null
      };

      _fileServiceMock
        .Setup(s => s.ExtractPublicId(user.ImageUrl))
        .Returns("publicid-123");

      _jwtServiceMock
        .Setup(s => s.CreateJWT(user))
        .Returns("valid.jwt.token");

      _cloudinaryMock
        .Setup(c => c.DestroyAsync(It.IsAny<DeletionParams>()))
        .ReturnsAsync(cloudResult);

      _dbContextMock
        .Setup(c => c.SaveChangesAsync(default))
        .ReturnsAsync(1);

      var result = await _controller.DeleteImage(new ImageDeleteDto { ImageUrl = "notthesameurl" });

      var badRequestResult = Assert.IsType<ObjectResult>(result.Result);
      Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }



    [Fact]
    public async Task ChangeName_ReturnsOk_ForValidRequest()
    {
      const string jwtToken = "valid.jwt.token";
      const string userId = "user-1";
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {jwtToken}";

      _jwtServiceMock
        .Setup(s => s.ExtractUserIdFromJwt(jwtToken))
        .Returns(userId);

      var user = new User { Id = userId, Email = "test@example.com", FirstName = "Ali", LastName = "Alzeer" };
      _userManagerMock
        .Setup(m => m.FindByIdAsync(userId))
        .ReturnsAsync(user);

      _jwtServiceMock
        .Setup(s => s.CreateJWT(user))
        .Returns("valid.jwt.token");

      _dbContextMock
        .Setup(c => c.SaveChangesAsync(default))
        .ReturnsAsync(1);

      var result = await _controller.ChangeName(new NameChangeDto { FirstName = user.FirstName, LastName = user.LastName });

      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }


    [Fact]
    public async Task DeleteAccount_Returns401_NonBearerHeader()
    {
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.Request.Headers.Authorization = "Basic xyz";

      var result = await _controller.DeleteAccount();

      Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task DeleteAccount_Returns404_UserIdNotFound()
    {
      const string jwtToken = "valid.jwt.token";
      const string userId = "user-1";
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {jwtToken}";

      _jwtServiceMock
        .Setup(j => j.ExtractUserIdFromJwt("valid.jwt.token"))
        .Returns(userId);

      _userManagerMock
        .Setup(u => u.FindByIdAsync(userId))
        .ReturnsAsync(null as User);

      var result = await _controller.DeleteAccount();

      var problem = Assert.IsType<ObjectResult>(result);
      Assert.Equal(StatusCodes.Status404NotFound, problem.StatusCode);
    }

    [Fact]
    public async Task DeleteAccount_ReturnsOk_DeletesUserAndFiles()
    {
      var fakeAccount = new Account("demoCloud", "demoKey", "demoSecret");
      const string jwtToken = "valid.jwt.token";
      User user = new() { Id = "user-1" };

      var ownersFiles = new List<storeitbackend.Models.OwnerFile>
       {
           new() { FileId = "f-1", UserId = user.Id },
           new()  { FileId = "f-2", UserId = user.Id },
       };

      var files = new List<storeitbackend.Models.File>
       {
           new() { Id = "f-1", Name = "file1.pdf" , Extension = new FileExtension{ Id = "ext-1", Name = "pdf"}, Type = new FileType{ Id = "type-1", Name = "document"}},
           new()  { Id = "f-2", Name = "file2.png", Extension = new FileExtension{ Id = "ext-2", Name = "png"}, Type = new FileType{ Id = "type-2", Name = "image"} }
       };
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {jwtToken}";

      _fileServiceMock.Setup(f => f.GetFileType(files[0].Name)).Returns(new FileTypeResult { Type = files[0].Type.Name, Extension = files[0].Extension.Name });
      _fileServiceMock.Setup(f => f.GetFileType(files[1].Name)).Returns(new FileTypeResult { Type = files[1].Type.Name, Extension = files[1].Extension.Name });

      _fileServiceMock.Setup(f => f.ExtractPublicId(It.IsAny<string>())).Returns("publicId");

      _cloudinaryExtensionServiceMock.Setup(c => c.ExecuteSearch(It.IsAny<string>())).Returns(new SearchResult() { Resources = [new() { PublicId = "publicId" }] });
      _cloudinaryMock.Setup(c => c.DestroyAsync(It.IsAny<DeletionParams>())).ReturnsAsync(new DeletionResult { Error = null });

      _jwtServiceMock.Setup(j => j.ExtractUserIdFromJwt(jwtToken)).Returns(user.Id);
      _userManagerMock.Setup(u => u.FindByIdAsync(user.Id)).ReturnsAsync(user);

      _userManagerMock.Setup(u => u.DeleteAsync(user))
                  .ReturnsAsync(IdentityResult.Success)
                  .Verifiable();


      var ownersFilesDbSetMock = ownersFiles.AsQueryable().BuildMockDbSet();
      var filesDbSetMock = files.AsQueryable().BuildMockDbSet();

      _dbContextMock.Setup(c => c.OwnersFiles)
                   .Returns(ownersFilesDbSetMock.Object);
      _dbContextMock.Setup(c => c.Files)
                   .Returns(filesDbSetMock.Object);

      var result = await _controller.DeleteAccount();

      var ok = Assert.IsType<OkObjectResult>(result);

      _userManagerMock.Verify();

    }


  }
}