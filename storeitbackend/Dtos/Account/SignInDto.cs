#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace storeitbackend.Dtos.Account
{
  public class SignInDto
  {
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
  }
}