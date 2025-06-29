#nullable disable

using System.ComponentModel.DataAnnotations;

namespace storeitbackend.Models
{
  public class OwnerFile
  {
    [Required]
    public string UserId { get; set; }
    [Required]
    public User User { get; set; }

    [Required]
    public string FileId { get; set; }
    [Required]
    public File File { get; set; }

  }
}