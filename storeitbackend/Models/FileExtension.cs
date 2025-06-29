#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace storeitbackend.Models
{
  public class FileExtension
  {
    [Required, Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = Guid.NewGuid().ToString().ToUpper();  // Ensure unique string ID

    [Required, MaxLength(50)]
    public string Name { get; set; }
  }
}