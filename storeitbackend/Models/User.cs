#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Identity;

namespace storeitbackend.Models
{
  public class User : IdentityUser
  {
    [Required, MinLength(2), MaxLength(15)]
    public string FirstName { get; set; }

    [Required, MinLength(2), MaxLength(15)]
    public string LastName { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<UserFile> UsersFiles { get; set; }
    public List<OwnerFile> OwnersFiles { get; set; }
    public string ImageUrl { get; set; }
  }
}