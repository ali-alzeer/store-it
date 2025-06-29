using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storeitbackend.Dtos.File
{
  public class UserFileDto
  {
    public string FileId { get; set; }
    public string FileName { get; set; }
    public string Url { get; set; }
    public string TypeId { get; set; }
    public string FileTypeName { get; set; }
    public string ExtensionId { get; set; }
    public string FileExtensionName { get; set; }
    public int Size { get; set; }
    public DateTime CreatedAt { get; set; }
    public string OwnerId { get; set; }
    public string OwnerFirstName { get; set; }
    public string OwnerLastName { get; set; }
    public string? OwnerImageUrl { get; set; }

  }
}