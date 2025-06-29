#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace storeitbackend.Models
{
  public class File
  {
    [Required, Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; } = Guid.NewGuid().ToString().ToUpper();

    [Required, MaxLength(256)]
    public string Name { get; set; }

    [Required, MaxLength(256)]
    public string Url { get; set; }

    [Required, ForeignKey("Type")]
    public string TypeId { get; set; }

    [Required]
    public FileType Type { get; set; }


    [Required, ForeignKey("Extension")]
    public string ExtensionId { get; set; }

    [Required]
    public FileExtension Extension { get; set; }

    [Required]
    public int Size { get; set; }

    public List<UserFile> UsersFiles { get; set; }
    public List<OwnerFile> OwnersFiles { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  }
}
