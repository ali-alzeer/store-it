#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace storeitbackend.Models
{
  public class UserFile
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