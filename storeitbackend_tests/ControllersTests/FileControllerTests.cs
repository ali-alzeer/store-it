using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
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
using storeitbackend.Dtos.File;
using storeitbackend.Interfaces;
using storeitbackend.Models;
using storeitbackend.Services;


namespace storeitbackend_tests.ControllersTests
{
  public class FileControllerTests
  {
    private readonly FileController _controller;
    private readonly Mock<ICloudinary> _cloudinaryMock;
    private readonly Mock<ICloudinaryExtensionService> _cloudinaryExtensionServiceMock;
    private readonly Mock<IAppDbContext> _dbContextMock;
    private readonly Mock<IJWTService> _jwtServiceMock;
    private readonly Mock<IFileService> _fileServiceMock;

    public FileControllerTests()
    {
      var fakeAccount = new Account("demoCloud", "demoKey", "demoSecret");
      _cloudinaryMock = new Mock<ICloudinary>();
      _cloudinaryExtensionServiceMock = new Mock<ICloudinaryExtensionService>();

      var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(Guid.NewGuid().ToString())
    .Options;

      _dbContextMock = new Mock<IAppDbContext>();
      var configMock = new Mock<IConfiguration>();

      configMock.Setup(c => c.GetSection("MaxFileSizeInBytes").Value)
                .Returns("5242880");


      _jwtServiceMock = new Mock<IJWTService>();
      _fileServiceMock = new Mock<IFileService>();

      _controller = new FileController(
        _cloudinaryMock.Object,
        _cloudinaryExtensionServiceMock.Object,
        _dbContextMock.Object,
        configMock.Object,
        _fileServiceMock.Object,
        _jwtServiceMock.Object
      );
    }

    [Fact]
    public async Task UploadFile_ReturnsOk_WithValidFile()
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

      var stream = new MemoryStream(new byte[] { 1, 2, 3 });
      var formFile = new FormFile(stream, 0, stream.Length, "img", "my-image.png")
      {
        Headers = new HeaderDictionary(),
        ContentType = "image/png"
      };
      var files = new List<storeitbackend.Models.File>
       {
           new() { Id = "f-1", Name = "my-image.png" , Extension = new FileExtension{ Id = "ext-1", Name = "png"}, Type = new FileType{ Id = "type-1", Name = "image"}},
       };

      var filesDbSetMock = files.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.Files)
                         .Returns(filesDbSetMock.Object);
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

      _fileServiceMock
        .Setup(f => f.SaveFileToDb(formFile, user.Id, cloudResult, null, null))
        .ReturnsAsync(1);

      _dbContextMock
        .Setup(c => c.SaveChangesAsync(default))
        .ReturnsAsync(1);

      var result = await _controller.UploadFile(formFile);

      var okResult = Assert.IsType<OkObjectResult>(result);
      Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task UploadFile_Returns401_WithUnauthorizedUser()
    {
      const string userId = "user-1";
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };

      var user = new User { Id = userId, Email = "test@example.com", ImageUrl = null };

      var stream = new MemoryStream(new byte[] { });

      var result = await _controller.UploadFile(new FormFile(stream, 0, 3, "my-image", "my-image.png"));

      var unauthorizedObjectResult = Assert.IsType<UnauthorizedObjectResult>(result);
      Assert.Equal(StatusCodes.Status401Unauthorized, unauthorizedObjectResult.StatusCode);
    }

    [Fact]
    public async Task UploadFile_Returns400_FileIsEmpty()
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
      var stream = new MemoryStream(new byte[] { });

      var result = await _controller.UploadFile(null);

      var badRequestResult = Assert.IsType<ObjectResult>(result);
      Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public void GetAllFilesForUser_ReturnsOk_WithValidRequest()
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
      var filesMock = new List<UserFileDto>
       {
           new() { FileId = "f-1"}
       };

      _fileServiceMock
        .Setup(f => f.GetUserFiles(userId))
        .Returns(filesMock);

      var result = _controller.GetAllFilesForUser();

      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public void GetAllFilesForUser_Returns401_WithUnacuthorizedUser()
    {
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };

      var result = _controller.GetAllFilesForUser();

      var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
      Assert.Equal(StatusCodes.Status401Unauthorized, unauthorizedResult.StatusCode);
    }
    [Fact]
    public void GetAllFilesForUser_Returns404_UserHasNoFiles()
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

      _fileServiceMock
        .Setup(f => f.GetUserFiles(userId))
        .Returns(null as List<UserFileDto>);

      var result = _controller.GetAllFilesForUser();

      var notfoundResult = Assert.IsType<ObjectResult>(result.Result);
      Assert.Equal(StatusCodes.Status404NotFound, notfoundResult.StatusCode);
    }


    [Fact]
    public void GetAllUsersForFile_ReturnsOk_WithValidRequest()
    {
      const string jwtToken = "valid.jwt.token";
      const string userId = "user-1";
      const string fileId = "file-1";
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {jwtToken}";

      _jwtServiceMock
        .Setup(s => s.ExtractUserIdFromJwt(jwtToken))
        .Returns(userId);

      var user = new User { Id = userId, Email = "test@example.com", ImageUrl = null };
      var usersSharedMock = new List<UserFileSharedDto>
       {
           new() { Id = "user-2"}
       };

      _fileServiceMock
        .Setup(f => f.GetUsersForFileByFileId(fileId))
        .Returns(usersSharedMock);

      var result = _controller.GetAllUsersForFile(fileId);

      var okResult = Assert.IsType<OkObjectResult>(result.Result);
      Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }
    [Fact]
    public void GetAllUsersForFile_Returns401_WithUnacuthorizedUser()
    {
      const string fileId = "file-1";
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };

      var result = _controller.GetAllUsersForFile(fileId);

      var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
      Assert.Equal(StatusCodes.Status401Unauthorized, unauthorizedResult.StatusCode);
    }
    [Fact]
    public void GetAllUsersForFile_Returns404_FileIsNotShared()
    {
      const string jwtToken = "valid.jwt.token";
      const string userId = "user-1";
      const string fileId = "file-1";
      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {jwtToken}";

      _jwtServiceMock
        .Setup(s => s.ExtractUserIdFromJwt(jwtToken))
        .Returns(userId);

      _fileServiceMock
        .Setup(f => f.GetUsersForFileByFileId(fileId))
        .Returns(null as List<UserFileSharedDto>);

      var result = _controller.GetAllUsersForFile(fileId);

      var notfoundResult = Assert.IsType<ObjectResult>(result.Result);
      Assert.Equal(StatusCodes.Status404NotFound, notfoundResult.StatusCode);
    }

    [Fact]
    public async Task DeleteAsset_ReturnsOk_WithValidRequest()
    {

      var fakeAccount = new Account("demoCloud", "demoKey", "demoSecret");
      const string jwtToken = "valid.jwt.token";
      User user = new() { Id = "user-1" };

      List<storeitbackend.Models.File> files = [new() { Id = "f-1", Name = "file1.pdf", Extension = new FileExtension { Id = "ext-1", Name = "pdf" }, Type = new FileType { Id = "type-1", Name = "document" } }];
      List<UserFile> usersFiles = [new() { FileId = "f-1", UserId = user.Id }];
      List<OwnerFile> ownersFiles = [new() { FileId = "f-1", UserId = user.Id }];

      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {jwtToken}";

      _jwtServiceMock.Setup(j => j.ExtractUserIdFromJwt(jwtToken)).Returns(user.Id);

      _fileServiceMock.Setup(f => f.GetFileType(files[0].Name)).Returns(new FileTypeResult { Type = files[0].Type.Name, Extension = files[0].Extension.Name });
      _fileServiceMock.Setup(f => f.GetFileExtensionNameByFileExtensionId(It.IsAny<string>())).ReturnsAsync(files[0].Extension.Name);
      _fileServiceMock.Setup(f => f.ExtractPublicId(It.IsAny<string>())).Returns("publicId");
      _fileServiceMock.Setup(f => f.DeleteFileFromDb(It.IsAny<storeitbackend.Models.File>(), It.IsAny<OwnerFile>(), It.IsAny<UserFile>())).ReturnsAsync(true);

      _cloudinaryExtensionServiceMock.Setup(c => c.ExecuteSearch(It.IsAny<string>())).Returns(new SearchResult() { Resources = [new() { PublicId = "publicId" }] });
      _cloudinaryMock.Setup(c => c.DestroyAsync(It.IsAny<DeletionParams>())).ReturnsAsync(new DeletionResult { Error = null });

      var ownersFilesDbSetMock = ownersFiles.AsQueryable().BuildMockDbSet();
      var filesDbSetMock = files.AsQueryable().BuildMockDbSet();
      var usersFilesDbSetMock = usersFiles.AsQueryable().BuildMockDbSet();

      _dbContextMock.Setup(c => c.UsersFiles)
                   .Returns(usersFilesDbSetMock.Object);
      _dbContextMock.Setup(c => c.OwnersFiles)
                   .Returns(ownersFilesDbSetMock.Object);
      _dbContextMock.Setup(c => c.Files)
                   .Returns(filesDbSetMock.Object);
      _dbContextMock.Setup(c => c.SaveChangesAsync(default))
                   .ReturnsAsync(1);

      var result = await _controller.DeleteAsset(new FileDeletionBackendDto { FileId = files[0].Id, FileUrl = "" });

      var ok = Assert.IsType<OkObjectResult>(result);
    }


    [Fact]
    public async Task RenameAsset_ReturnsOk_WithValidRequest()
    {

      var fakeAccount = new Account("demoCloud", "demoKey", "demoSecret");
      const string jwtToken = "valid.jwt.token";
      User user = new() { Id = "user-1" };

      List<storeitbackend.Models.File> files = [new() { Id = "f-1", Name = "file1.pdf", Extension = new FileExtension { Id = "ext-1", Name = "pdf" }, Type = new FileType { Id = "type-1", Name = "document" } }];
      List<OwnerFile> ownersFiles = [new() { FileId = "f-1", UserId = user.Id }];

      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {jwtToken}";

      _jwtServiceMock.Setup(j => j.ExtractUserIdFromJwt(jwtToken)).Returns(user.Id);

      var filesDbSetMock = files.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.Files)
                   .Returns(filesDbSetMock.Object);

      var ownersFilesDbSetMock = ownersFiles.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.OwnersFiles)
                   .Returns(ownersFilesDbSetMock.Object);

      _dbContextMock.Setup(c => c.SaveChangesAsync(default))
                   .ReturnsAsync(1);

      var result = await _controller.RenameAsset(new FileRenameDto { FileId = files[0].Id, FileName = files[0].Name });

      var ok = Assert.IsType<OkObjectResult>(result);
    }


    [Fact]
    public async Task ShareAsset_ReturnsOk_WithValidRequest()
    {

      var fakeAccount = new Account("demoCloud", "demoKey", "demoSecret");
      const string jwtToken = "valid.jwt.token";
      User user = new() { Id = "user-1", Email = "test@test.com" };
      User userToShare = new() { Id = "user-2", Email = "test2@test.com" };

      List<storeitbackend.Models.File> files = [new() { Id = "f-1", Name = "file1.pdf", Extension = new FileExtension { Id = "ext-1", Name = "pdf" }, Type = new FileType { Id = "type-1", Name = "document" } }];
      List<UserFile> usersFiles = [new() { FileId = "f-1", UserId = user.Id }];
      List<User> users = [userToShare];

      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {jwtToken}";

      _jwtServiceMock.Setup(j => j.ExtractUserIdFromJwt(jwtToken)).Returns(user.Id);

      var filesDbSetMock = files.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.Files)
                   .Returns(filesDbSetMock.Object);

      var usersFilesDbSetMock = usersFiles.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.UsersFiles)
                   .Returns(usersFilesDbSetMock.Object);

      var usersDbSetMock = users.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.Users)
                   .Returns(usersDbSetMock.Object);

      _dbContextMock.Setup(c => c.SaveChangesAsync(default))
                   .ReturnsAsync(1);

      var result = await _controller.ShareAsset(new FileShareDto { FileId = files[0].Id, Emails = [userToShare.Email] });

      var ok = Assert.IsType<OkObjectResult>(result);
    }


    [Fact]
    public async Task ShareRemoveAsset_ReturnsOk_WithValidRequest()
    {

      var fakeAccount = new Account("demoCloud", "demoKey", "demoSecret");
      const string jwtToken = "valid.jwt.token";
      User user = new() { Id = "user-1", Email = "test@test.com" };
      User userToShareRemove = new() { Id = "user-2", Email = "test2@test.com" };

      List<storeitbackend.Models.File> files = [new() { Id = "f-1", Name = "file1.pdf", Extension = new FileExtension { Id = "ext-1", Name = "pdf" }, Type = new FileType { Id = "type-1", Name = "document" } }];
      List<UserFile> usersFiles = [new() { FileId = "f-1", UserId = user.Id }, new() { FileId = "f-1", UserId = userToShareRemove.Id }];
      List<User> users = [userToShareRemove];

      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = new DefaultHttpContext()
      };
      _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {jwtToken}";

      _jwtServiceMock.Setup(j => j.ExtractUserIdFromJwt(jwtToken)).Returns(user.Id);

      var filesDbSetMock = files.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.Files)
                   .Returns(filesDbSetMock.Object);

      var usersFilesDbSetMock = usersFiles.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.UsersFiles)
                   .Returns(usersFilesDbSetMock.Object);

      var usersDbSetMock = users.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.Users)
                   .Returns(usersDbSetMock.Object);

      _dbContextMock.Setup(c => c.SaveChangesAsync(default))
                   .ReturnsAsync(1);

      var result = await _controller.ShareRemoveAsset(new FileShareRemoveDto { FileId = files[0].Id, UserEmailToRemove = userToShareRemove.Email });

      var ok = Assert.IsType<OkObjectResult>(result);
    }

  }
}