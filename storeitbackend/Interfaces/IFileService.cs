using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using storeitbackend.Dtos.Account;
using storeitbackend.Dtos.File;
using storeitbackend.Models;
using storeitbackend.Services;

namespace storeitbackend.Interfaces
{

  public interface IFileService
  {
    FileTypeResult GetFileType(string fileName);
    Task<Models.File?> CreateFileEntityAsync(IFormFile formFile, string cloudinaryUrl);
    string? ExtractPublicId(string url);
    Task<string?> GetFileExtensionId(string FileExtension);
    Task<string?> GetFileExtensionNameByFileExtensionId(string fileExtensionId);
    Task<string?> GetFileTypeId(string FileType);
    Task<User?> GetOwnerByFileId(string FileId);
    List<UserFileDto>? GetUserFiles(string userId);
    List<UserFileSharedDto>? GetUsersForFileByFileId(string fileId);
    Task<int> SaveFileToDb(IFormFile file, string userId, ImageUploadResult? imageUploadResult = null, VideoUploadResult? videoUploadResult = null, RawUploadResult? rawUploadResult = null);
  }

}