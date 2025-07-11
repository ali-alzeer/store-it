
#nullable disable

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using storeitbackend.Dtos.Account;
using storeitbackend.Interfaces;
using storeitbackend.Models;

namespace storeitbackend.Services
{
  public class JWTService : IJWTService
  {
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _jwtKey;
    public JWTService(IConfiguration configuration)
    {
      _configuration = configuration;
      _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
    }

    public string CreateJWT(User user)
    {
      var userClaims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
            };

      var credentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(userClaims),
        Expires = DateTime.UtcNow.AddDays(int.Parse(_configuration["JWT:ExpiresInDays"])),
        SigningCredentials = credentials,
        Issuer = _configuration["JWT:Issuer"]
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var jwt = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(jwt);
    }

    public TokenValidation ValidateJwt(string jwt)
    {
      try
      {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
          ValidateIssuerSigningKey = true,
          ValidIssuer = _configuration["JWT:Issuer"],
          ValidateIssuer = true,
          ValidateAudience = false,
          ClockSkew = new TimeSpan(int.Parse(_configuration["JWT:ExpiresInDays"]), 0, 0, 0),
          ValidateLifetime = true,
        };

        tokenHandler.ValidateToken(jwt, validationParameters, out var validatedToken);

        return new TokenValidation
        {
          isValid = true,
          tokenValidationErrorType = null
        };
      }
      catch (SecurityTokenExpiredException)
      {

        return new TokenValidation
        {
          isValid = false,
          tokenValidationErrorType = TokenValidationErrorType.Expired
        };
      }
      catch (Exception)
      {

        return new TokenValidation
        {
          isValid = false,
          tokenValidationErrorType = TokenValidationErrorType.Invalid
        };
      }

    }

    public string? ExtractUserIdFromJwt(string token)
    {
      try
      {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier || claim.Type == "nameid")?.Value;
        if (userId == null) return null;
        return userId;
      }
      catch
      {
        return null;
      }



    }
  }
}