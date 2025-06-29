using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storeitbackend.Dtos.Account
{
  public class TokenValidation
  {
    public bool isValid { get; set; }
    public TokenValidationErrorType? tokenValidationErrorType { get; set; }
  }
}