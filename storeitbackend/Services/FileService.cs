using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using storeitbackend.Data;
using storeitbackend.Dtos.Account;
using storeitbackend.Dtos.File;
using storeitbackend.Interfaces;
using storeitbackend.Models;

namespace storeitbackend.Services
{

  public class FileTypeResult
  {
    public string Type { get; set; } = "";
    public string Extension { get; set; } = "";
  }

  public class FileService : IFileService
  {
    private readonly AppDbContext _context;

    public FileService(AppDbContext context)
    {
      _context = context;
    }

    public async Task<string?> GetFileTypeId(string FileType)
    {
      var fileType = await _context.FileTypes.FirstOrDefaultAsync(ft =>
      ft.Name == FileType.ToLower()
      );

      if (fileType == null)
      {
        return null;
      }

      return fileType.Id;
    }

    public async Task<string?> GetFileExtensionId(string FileExtension)
    {
      var fileExtension = await _context.FileExtensions.FirstOrDefaultAsync(ft =>
      ft.Name == FileExtension.ToLower()
      );

      if (fileExtension == null)
      {
        return null;
      }

      return fileExtension.Id;
    }

    public async Task<User?> GetOwnerByFileId(string FileId)
    {
      var ownerFileObject = await _context.OwnersFiles.FirstOrDefaultAsync(owf =>
      owf.FileId.ToLower() == FileId.ToLower()
      );

      if (ownerFileObject == null)
      {
        return null;
      }

      var owner = await _context.Users.FirstOrDefaultAsync(u =>
            u.Id.ToLower() == ownerFileObject.UserId.ToLower()
            );

      if (owner == null)
      {
        return null;
      }

      return owner;
    }


    public async Task<string?> GetFileExtensionNameByFileExtensionId(string fileExtensionId)
    {
      var fileExtension = await _context.FileExtensions.FirstOrDefaultAsync(ft =>
      ft.Id.ToLower() == fileExtensionId.ToLower()
      );

      if (fileExtension == null)
      {
        return null;
      }

      return fileExtension.Name;
    }


    public FileTypeResult GetFileType(string fileName)
    {
      if (string.IsNullOrWhiteSpace(fileName))
      {
        return new FileTypeResult { Type = "other", Extension = "" };
      }

      // Extract the extension by splitting on the period.
      string[] parts = fileName.Split('.');
      if (parts.Length < 2)
      {
        return new FileTypeResult { Type = "other", Extension = "" };
      }

      string extension = parts.Last().ToLower();

      // Define the sets of valid extensions.
      HashSet<string> documentExtensions = new HashSet<string>
        {
            "pdf", "doc", "docx", "txt", "xls", "xlsx", "csv", "rtf",
            "ods", "ppt", "odp", "md", "html", "htm", "epub", "pages",
            "fig", "psd", "ai", "indd", "xd", "sketch", "afdesign", "afphoto"
        };

      HashSet<string> imageExtensions = new HashSet<string>
        {
            "jpg", "jpeg", "png", "gif", "bmp", "svg", "webp"
        };

      HashSet<string> videoExtensions = new HashSet<string>
        {
            "mp4", "avi", "mov", "mkv", "webm"
        };

      HashSet<string> audioExtensions = new HashSet<string>
        {
            "mp3", "wav", "ogg", "flac", "m4a"
        };

      // Check each category.
      if (documentExtensions.Contains(extension))
      {
        return new FileTypeResult { Type = "document", Extension = extension };
      }
      if (imageExtensions.Contains(extension))
      {
        return new FileTypeResult { Type = "image", Extension = extension };
      }
      if (videoExtensions.Contains(extension))
      {
        return new FileTypeResult { Type = "video", Extension = extension };
      }
      if (audioExtensions.Contains(extension))
      {
        return new FileTypeResult { Type = "audio", Extension = extension };
      }

      return new FileTypeResult { Type = "other", Extension = extension };
    }

    public async Task<Models.File?> CreateFileEntityAsync(IFormFile formFile, string cloudinaryUrl)
    {
      if (formFile == null || formFile.Length <= 0)
      {
        return null;
      }

      // Extract the original file name and extension.
      string originalFileName = Path.GetFileName(formFile.FileName);

      string fileExtension = Path.GetExtension(originalFileName).TrimStart('.');
      string? fileExtensionId = await GetFileExtensionId(fileExtension);
      if (fileExtensionId == null)
      {
        // Add new file extension to db
        await _context.FileExtensions.AddAsync(new FileExtension
        {
          Name = fileExtension.ToLower()
        });

        int AffectedColumns = await _context.SaveChangesAsync();

        if (AffectedColumns <= 0)
        {
          return null;
        }

        fileExtensionId = await GetFileExtensionId(fileExtension);
      }

      string fileType = GetFileType(formFile.FileName).Type.ToLower();
      string? fileTypeId = await GetFileTypeId(fileType);
      if (fileTypeId == null)
      {
        // Add new file extension to db
        await _context.FileTypes.AddAsync(new FileType
        {
          Name = fileType.ToLower()
        });

        int AffectedColumns = await _context.SaveChangesAsync();

        if (AffectedColumns <= 0)
        {
          return null;
        }

        fileTypeId = await GetFileTypeId(fileType);
      }

      string fileUrl = cloudinaryUrl;

      // Create the File entity instance.
      var fileEntity = new Models.File
      {
        Name = originalFileName,
        Url = fileUrl,
        Size = (int)formFile.Length,
        ExtensionId = fileExtensionId,
        TypeId = fileTypeId,
        CreatedAt = DateTime.UtcNow
      };

      return fileEntity;
    }

    public async Task<int> SaveFileToDb(IFormFile file, string userId, ImageUploadResult? imageUploadResult = null, VideoUploadResult? videoUploadResult = null, RawUploadResult? rawUploadResult = null)
    {
      Models.File? fileToSaveToDb = null;

      if (imageUploadResult != null)
      {
        fileToSaveToDb = await CreateFileEntityAsync(file, imageUploadResult.Url.ToString());
      }
      else if (videoUploadResult != null)
      {
        fileToSaveToDb = await CreateFileEntityAsync(file, videoUploadResult.Url.ToString());
      }
      else if (rawUploadResult != null)
      {
        fileToSaveToDb = await CreateFileEntityAsync(file, rawUploadResult.Url.ToString());
      }
      else
      {
        return -1;
      }

      if (fileToSaveToDb == null)
      {
        return -1;
      }
      else
      {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
          await _context.Files.AddAsync(fileToSaveToDb);

          int rowsAffectedFromFilesTable = await _context.SaveChangesAsync();
          if (rowsAffectedFromFilesTable > 0)
          {
            await transaction.CreateSavepointAsync("1");
            FileTypeResult FileTypeResultObject = GetFileType(file.FileName);
            var filesExistsInDb = await _context.Files.Where(f => f.Name == Path.GetFileName(file.FileName) && f.Type.Name.ToLower() == FileTypeResultObject.Type.ToLower() && f.Extension.Name.ToLower() == FileTypeResultObject.Extension.ToLower()).ToListAsync();
            if (filesExistsInDb.Count() > 0)
            {

              foreach (var fileInDb in filesExistsInDb)
              {
                OwnerFile? owner = await _context.OwnersFiles.FirstOrDefaultAsync(owf => owf.FileId == fileInDb.Id && owf.UserId == userId);
                if (owner != null)
                {
                  await transaction.RollbackAsync();
                  return 0;
                }
                else
                {
                  await _context.OwnersFiles.AddAsync(new OwnerFile
                  {
                    UserId = userId,
                    FileId = fileInDb.Id,
                  });
                  await _context.SaveChangesAsync();
                  await transaction.CreateSavepointAsync("2");
                  break;
                }

              }


              foreach (var fileInDb in filesExistsInDb)
              {
                UserFile? user = await _context.UsersFiles.FirstOrDefaultAsync(uf => uf.FileId == fileInDb.Id && uf.UserId == userId);
                if (user != null)
                {
                  await transaction.RollbackAsync();
                  return 0;
                }
                else
                {
                  await _context.UsersFiles.AddAsync(new UserFile
                  {
                    UserId = userId,
                    FileId = fileInDb.Id,
                  });
                  await _context.SaveChangesAsync();
                  await transaction.CreateSavepointAsync("3");
                  break;
                }
              }

              await transaction.CommitAsync();
              return 3;

            }
            else
            {
              await transaction.RollbackAsync();
              return 0;
            }
          }
          else
          {
            await transaction.RollbackAsync();
            return 0;
          }
        }
        catch
        {
          await transaction.RollbackAsync();
          return 0;
        }

      }


    }

    public List<UserFileDto>? GetUserFiles(string userId)
    {
      return _context.Set<UserFileDto>()
    .FromSqlInterpolated($"EXEC dbo.SP_GetUserFiles @UserId = {userId}")
    .AsNoTracking()
    .ToList();

    }


    public List<UserFileSharedDto>? GetUsersForFileByFileId(string fileId)
    {
      return _context.Set<UserFileSharedDto>()
    .FromSqlInterpolated($"EXEC dbo.SP_GetUsersForFileByFileId @FileId = {fileId}")
    .AsNoTracking()
    .ToList();
    }


    public string? ExtractPublicId(string url)
    {
      // Remove any query parameters.
      string cleanUrl = url.Split('?')[0];

      // Regular expression breakdown:
      // - /upload/ : Matches the literal "/upload/"
      // - (?:[^/]+/)*? : Optionally matches any intermediate folder path (e.g., transformations). Non-greedy.
      // - (?:v\d+/)? : Optionally matches a version string like "v1312461204/"
      // - (.+?) : Captures the publicId (as few characters as possible) until the next part.
      // - \.[^./]+$ : Matches the file extension (e.g., .jpg, .png) at the end of the URL.
      string pattern = @"/upload/(?:[^/]+/)*?(?:v\d+/)?(.+?)\.[^./]+$";

      Match match = Regex.Match(cleanUrl, pattern);
      if (match.Success && match.Groups.Count > 1)
      {
        return match.Groups[1].Value;
      }

      return null;
    }

  }

}
