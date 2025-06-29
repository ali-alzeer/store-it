#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace storeitbackend.Models
{
  public class FileType
  {
    [Required, Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; } = Guid.NewGuid().ToString().ToUpper();  // Ensure unique string ID

    [Required, MaxLength(50)]
    public string Name { get; set; }
  }
}