#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace storeitbackend.Dtos.Account
{
  public class UserDto
  {

    [Required]
    public string Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string ImageUrl { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    [Required]
    public string JWT { get; set; }
  }
}