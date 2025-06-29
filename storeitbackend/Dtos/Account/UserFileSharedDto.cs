#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace storeitbackend.Dtos.Account
{
  public class UserFileSharedDto
  {

    [Required, MaxLength(36)]
    public string Id { get; set; }
    [Required, MaxLength(15)]
    public string FirstName { get; set; }
    [Required, MaxLength(15)]
    public string LastName { get; set; }
    [Required, MaxLength(50)]
    public string Email { get; set; }
    [MaxLength(256)]
    public string ImageUrl { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }

  }
}