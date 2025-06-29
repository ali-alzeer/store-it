#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace storeitbackend.Dtos.Account
{
  public class SignUpDto
  {
    [Required]
    [StringLength(15, MinimumLength = 2, ErrorMessage = "First name must be at least {2}, and maximum {1} characters")]
    public string FirstName { get; set; }
    [Required]
    [StringLength(15, MinimumLength = 2, ErrorMessage = "Last name must be at least {2}, and maximum {1} characters")]
    public string LastName { get; set; }
    [Required]
    [RegularExpression("^\\w+@[a-zA-Z0-9-]+\\.[a-zA-Z]{2,}$")]
    public string Email { get; set; }
    [Required]
    [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be at least {2}, and maximum {1} characters")]
    public string Password { get; set; }
  }
}