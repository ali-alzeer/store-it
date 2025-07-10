using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using storeitbackend.Dtos.Account;
using storeitbackend.Models;

namespace storeitbackend.Interfaces
{
  public interface IJWTService
  {
    string CreateJWT(User user);
    string ExtractUserIdFromJwt(string token);
    TokenValidation ValidateJwt(string jwt);
  }
}