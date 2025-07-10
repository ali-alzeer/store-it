using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using storeitbackend.Controllers;
using storeitbackend.Data;
using storeitbackend.Dtos.Account;
using storeitbackend.Interfaces;
using storeitbackend.Models;


namespace storeitbackend_tests.ControllersTests
{
  public class AccountControllerTests
  {
    private readonly AccountController _controller;
    private readonly Mock<Cloudinary> _cloudinaryMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly Mock<AppDbContext> _dbContextMock;
    private readonly Mock<IJWTService> _jwtServiceMock;
    private readonly Mock<IFileService> _fileServiceMock;

    public AccountControllerTests()
    {
      var fakeAccount = new Account("demoCloud", "demoKey", "demoSecret");
      _cloudinaryMock = new Mock<Cloudinary>(fakeAccount);

      var userStoreMock = new Mock<IUserStore<User>>();
      _userManagerMock = new Mock<UserManager<User>>(
        userStoreMock.Object,
        Options.Create(new IdentityOptions()),
        new PasswordHasher<User>(),
        Array.Empty<IUserValidator<User>>(),
        Array.Empty<IPasswordValidator<User>>(),
        new UpperInvariantLookupNormalizer(),
        new IdentityErrorDescriber(),
        null, null
      );

      var contextAccessor = new Mock<IHttpContextAccessor>();
      var userClaimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
      _signInManagerMock = new Mock<SignInManager<User>>(
        _userManagerMock.Object,
        contextAccessor.Object,
        userClaimsFactory.Object,
        null, null, null, null
      );

      var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(Guid.NewGuid().ToString())
    .Options;

      _dbContextMock = new Mock<AppDbContext>(options);
      var configMock = new Mock<IConfiguration>();

      configMock.Setup(c => c["JWT:Key"])
                .Returns("super-secret-test-key");
      configMock.Setup(c => c["JWT:Issuer"])
                .Returns("TestIssuer");
      configMock.Setup(c => c["JWT:ExpiresInDays"])
                .Returns("15");


      _jwtServiceMock = new Mock<IJWTService>();
      _fileServiceMock = new Mock<IFileService>();

      _controller = new AccountController(
        _cloudinaryMock.Object,
        _dbContextMock.Object,
        _jwtServiceMock.Object,
        _fileServiceMock.Object,
        _signInManagerMock.Object,
        _userManagerMock.Object
      );
    }

    [Fact]
    public async Task SignIn_ReturnsOk_WithUserDto()
    {
      var dto = new SignInDto { Email = "alialzeer@example.com", Password = "very-strong-passsword" };
      var fakeUser = new User { FirstName = "Ali", LastName = "Alzeer", Email = "alialzeer@example.com" };

      _jwtServiceMock.Setup(j => j.CreateJWT(fakeUser)).Returns("user-jwt");

      _userManagerMock
        .Setup(m => m.FindByEmailAsync(It.Is<string>(email => email == dto.Email)))
        .ReturnsAsync(fakeUser);

      _signInManagerMock
        .Setup(m => m.CheckPasswordSignInAsync(
            It.Is<User>(u => u.Email == fakeUser.Email),
            It.Is<string>(pwd => pwd == dto.Password),
            false))
        .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

      var actionResult = await _controller.SignIn(dto);

      var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

      var userDto = Assert.IsType<UserDto>(okResult.Value);
      Console.WriteLine("Signed in as: " + userDto.Email);
      Assert.Equal("Ali", userDto.FirstName);
      Assert.Equal(dto.Email, userDto.Email);

    }

    [Theory]
    [InlineData("bad-email", "irrelevant")]
    [InlineData("unknown@foo.com", "irrelevant")]
    [InlineData("alialzeer@example.com", "wrongpass")]
    public async Task SignIn_Returns401_ForInvalidCredentials(
      string email, string password)
    {
      var dto = new SignInDto { Email = email, Password = password };

      if (Regex.IsMatch(email, "^\\w+@[a-zA-Z0-9-]+\\.[a-zA-Z]{2,}$"))
      {
        _userManagerMock
          .Setup(m => m.FindByEmailAsync(email))
          .ReturnsAsync(null as User);
      }

      if (email == "alialzeer@example.com")
      {
        var fakeUser = new User { Email = email };
        _userManagerMock
          .Setup(m => m.FindByEmailAsync(email))
          .ReturnsAsync(fakeUser);
        _signInManagerMock
          .Setup(m => m.CheckPasswordSignInAsync(fakeUser, password, false))
          .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);
      }

      var actionResult = await _controller.SignIn(dto);

      var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
      Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);
      var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
      Assert.Equal("Invalid email or password", problem.Detail);
    }

  }
}