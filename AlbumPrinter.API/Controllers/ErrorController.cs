using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.API.Controllers
{
    [Route("error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ExcludeFromCodeCoverage]
    public class ErrorsController : ControllerBase
    {
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            Exception? exception = context.Error;

            // Handle specific exceptions

            // Handle unhandled ArgumentExceptions with BadRequest
            if (exception is ArgumentException)
            {
                return BadRequest("Please resolve the issues with request");
            }

            // Default behavior
            return StatusCode(500, "Something went wrong! Please try again...");
        }
    }
}