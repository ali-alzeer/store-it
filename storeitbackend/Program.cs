#nullable disable

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using storeitbackend.Data;
using storeitbackend.Exceptions;
using storeitbackend.Interfaces;
using storeitbackend.Models;
using storeitbackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(o =>
{
  o.CustomizeProblemDetails = context =>
  {
    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
  };


});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddIdentityCore<User>(options =>
{
  options.Password.RequiredLength = 6;
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequireDigit = false;
  options.Password.RequireLowercase = false;
  options.Password.RequireUppercase = false;

  options.SignIn.RequireConfirmedEmail = false;
})
.AddRoles<IdentityRole>()
.AddRoleManager<RoleManager<IdentityRole>>()
.AddSignInManager<SignInManager<User>>()
.AddUserManager<UserManager<User>>()
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
  options.TokenValidationParameters = new TokenValidationParameters
  {
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["JWT:Issuer"],
    ValidateIssuer = true,
    ValidateAudience = false,
    ClockSkew = new TimeSpan(int.Parse(builder.Configuration["JWT:ExpiresInDays"]), 0, 0, 0),
    ValidateLifetime = true,
  };

});

builder.Services.AddSingleton(new CloudinaryDotNet.Cloudinary(new CloudinaryDotNet.Account(
    builder.Configuration["Cloudinary:CloudName"],
    builder.Configuration["Cloudinary:ApiKey"],
    builder.Configuration["Cloudinary:ApiSecret"]
)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference();
}

app.UseCors(o =>
{
  o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseStatusCodePages();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
