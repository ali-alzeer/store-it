using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using storeitbackend.Data;
using storeitbackend.Dtos.File;
using storeitbackend.Models;
using storeitbackend.Services;

namespace storeitbackend_tests.ServicesTests
{
  public class FileServiceTests
  {
    private readonly FileService _fileService;
    private readonly Mock<IAppDbContext> _dbContextMock;
    public FileServiceTests()
    {
      _dbContextMock = new Mock<IAppDbContext>();
      _fileService = new FileService(_dbContextMock.Object);
    }

    [Fact]
    public async Task GetFileTypeId_ReturnsFileTypeId_ForValidTypeName()
    {
      var fileTypes = new List<FileType> { new FileType { Id = "type-1", Name = "document" } }.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.FileTypes).Returns(fileTypes.Object);

      var validTypeId = await _fileService.GetFileTypeId("document");
      Assert.Equal("type-1", validTypeId);
    }

    [Fact]
    public async Task GetFileTypeId_ReturnsNull_ForInvalidTypeName()
    {
      var fileTypes = new List<FileType> { new FileType { Id = "type-1", Name = "document" } }.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.FileTypes).Returns(fileTypes.Object);

      var InvalidTypeId = await _fileService.GetFileTypeId("invalid-typename");
      Assert.Null(InvalidTypeId);
    }


    [Fact]
    public async Task GetFileExtensionId_ReturnsFileExtensionId_ForValidExtensionName()
    {
      var fileExtensions = new List<FileExtension> { new FileExtension { Id = "extension-1", Name = "pdf" } }.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.FileExtensions).Returns(fileExtensions.Object);

      var validExtensionId = await _fileService.GetFileExtensionId("pdf");
      Assert.Equal("extension-1", validExtensionId);
    }

    [Fact]
    public async Task GetFileExtensionId_ReturnsNull_ForInvalidExtensionName()
    {
      var fileExtensions = new List<FileExtension> { new FileExtension { Id = "extension-1", Name = "pdf" } }.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.FileExtensions).Returns(fileExtensions.Object);

      var InvalidExtensionId = await _fileService.GetFileExtensionId("invalid-extensionname");
      Assert.Null(InvalidExtensionId);
    }



    [Fact]
    public async Task GetOwnerByFileId_ReturnsOwnerOfTheFile_ForValidFileId()
    {
      string fileId = "file-1";
      string userId = "user-1";
      var ownersFiles = new List<OwnerFile> { new OwnerFile { FileId = fileId, UserId = userId } }.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.OwnersFiles).Returns(ownersFiles.Object);

      var users = new List<User> { new User { Id = userId } }.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.Users).Returns(users.Object);

      var ownerOfTheFile = await _fileService.GetOwnerByFileId(fileId);
      Assert.Equal(userId, ownerOfTheFile?.Id);
    }

    [Fact]
    public async Task GetOwnerByFileId_ReturnsNull_ForInvalidFileId()
    {
      string fileId = "file-1";
      string userId = "user-1";
      string invalidFileId = "invalid-fileId";
      var ownersFiles = new List<OwnerFile> { new OwnerFile { FileId = fileId, UserId = userId } }.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.OwnersFiles).Returns(ownersFiles.Object);

      var users = new List<User> { new User { Id = userId } }.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.Users).Returns(users.Object);

      var ownerOfTheFile = await _fileService.GetOwnerByFileId(invalidFileId);
      Assert.Null(ownerOfTheFile);
    }


    [Fact]
    public async Task GetFileExtensionNameByFileExtensionId_ReturnsFileTypeId_ForValidExtensionId()
    {
      var fileExtensions = new List<FileExtension> { new FileExtension { Id = "extension-1", Name = "pdf" } }.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.FileExtensions).Returns(fileExtensions.Object);

      var validExtensionName = await _fileService.GetFileExtensionNameByFileExtensionId("extension-1");
      Assert.Equal("pdf", validExtensionName);
    }

    [Fact]
    public async Task GetFileExtensionNameByFileExtensionId_ReturnsNull_ForInvalidExtensionId()
    {
      var fileExtensions = new List<FileExtension> { new FileExtension { Id = "extension-1", Name = "pdf" } }.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.FileExtensions).Returns(fileExtensions.Object);

      var InvalidExtensionName = await _fileService.GetFileExtensionNameByFileExtensionId("invalid-extensionid");
      Assert.Null(InvalidExtensionName);
    }

    [Theory]
    [InlineData("this-is-a-document.pdf", "document")]
    [InlineData("this-is-an-image.png", "image")]
    [InlineData("this-is-an-audio.mp3", "audio")]
    [InlineData("this-is-a-video.mp4", "video")]
    [InlineData("this-is-an-other.zip", "other")]
    public void GetFileType_ReturnsFileType_ForFileName(string fileName, string expected)
    {
      var fileType = _fileService.GetFileType(fileName);
      Assert.Equal(expected, fileType.Type);
    }

    [Fact]
    public async Task CreateFileEntityAsync_ReturnsFileEntity_ForValidFormFile()
    {
      var fileTypes = new List<FileType> { new FileType { Id = "type-1", Name = "document" } }.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.FileTypes).Returns(fileTypes.Object);

      var fileExtensions = new List<FileExtension> { new FileExtension { Id = "extension-1", Name = "pdf" } }.AsQueryable().BuildMockDbSet();
      _dbContextMock.Setup(c => c.FileExtensions).Returns(fileExtensions.Object);

      _dbContextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

      var stream = new MemoryStream([1, 2, 3]);
      FormFile formFile = new(stream, 0, stream.Length, "some-doc", "some-doc.pdf");


      var actualfileEntity = await _fileService.CreateFileEntityAsync(formFile, "valid-url");
      var expectedFileEntity = new storeitbackend.Models.File { Id = actualfileEntity?.Id, Name = formFile.FileName, Url = "valid-url", Size = (int)formFile.Length, ExtensionId = "extension-1", TypeId = "type-1", CreatedAt = actualfileEntity.CreatedAt };

      actualfileEntity.Should().BeEquivalentTo(expectedFileEntity);
    }


    [Theory]
    [InlineData("https://res.cloudinary.com/test-cloud-name/image/upload/v1/test-public-id.png", "test-public-id")]
    [InlineData("invalid-url", null)]
    public void ExtractPublicId_ReturnsPublicId_ForValidUrl(string url, string? expected)
    {
      var actualPublicId = _fileService.ExtractPublicId(url);

      Assert.Equal(expected, actualPublicId);
    }

  }
}