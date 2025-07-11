using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using storeitbackend.Models;
using storeitbackend.Services;

namespace storeitbackend_tests.ServicesTests
{
  public class JWTServiceTests
  {
    private readonly JWTService _jwtService;
    public JWTServiceTests()
    {
      var configMock = new Mock<IConfiguration>();

      configMock.Setup(c => c["JWT:Key"])
                .Returns("super-secret-test-key-super-secret-test-key-super-secret-test-key-super-secret-test-key-super-secret-test-key");
      configMock.Setup(c => c["JWT:Issuer"])
                .Returns("TestIssuer");
      configMock.Setup(c => c["JWT:ExpiresInDays"])
                .Returns("15");

      _jwtService = new JWTService(configMock.Object);
    }

    [Fact]
    public void CreateJWT_ReturnsValidJWT_ForValidUser()
    {
      User user = new() { Id = "user-1", Email = "test@test.com", FirstName = "Test", LastName = "Test" };

      var validJwt = _jwtService.CreateJWT(user);
      var tokenValidationObject = _jwtService.ValidateJwt(validJwt);
      Assert.True(tokenValidationObject.isValid);
    }

    [Fact]
    public void ValidateJwt_ReturnsFalse_ForInvalidJWT()
    {
      var tokenValidationObject = _jwtService.ValidateJwt("invalid-jwt");
      Assert.False(tokenValidationObject.isValid);
    }


    [Fact]
    public void ExtractUserIdFromJwt_ReturnsUserId_ForValidJwt()
    {
      User user = new() { Id = "user-1", Email = "test@test.com", FirstName = "Test", LastName = "Test" };

      var validJwt = _jwtService.CreateJWT(user);
      var userId = _jwtService.ExtractUserIdFromJwt(validJwt);
      Assert.Equal(user.Id, userId);
    }

    [Fact]
    public void ExtractUserIdFromJwt_ReturnsNull_ForInvalidJwt()
    {
      var userId = _jwtService.ExtractUserIdFromJwt("invalid-jwt");
      Assert.Null(userId);
    }
  }
}