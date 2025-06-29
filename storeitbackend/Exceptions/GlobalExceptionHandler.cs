#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace storeitbackend.Exceptions
{
  internal sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
  {
    public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
    {
      httpContext.Response.StatusCode = exception switch
      {
        SecurityTokenExpiredException ste => StatusCodes.Status410Gone,
        ApplicationException e => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status500InternalServerError
      };

      return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
      {
        HttpContext = httpContext,
        Exception = exception,
        ProblemDetails = new HttpValidationProblemDetails
        {
          Type = exception.GetType().Name,
          Title = "Unknown error occurred",
          Detail = exception.Message,
        }
      });
    }
  }
}