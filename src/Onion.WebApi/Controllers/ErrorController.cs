using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Onion.Application.Services.Exceptions;
using Onion.WebApi.Models;
using Onion.WebApi.Resources;

namespace Onion.WebApi.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly IStringLocalizer<Resource> _localizer;

        public ErrorController(IStringLocalizer<Resource> localizer)
        {
            _localizer = localizer;
        }

        [Route("error")]
        public ErrorRes Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            string message;

            if (exception is NotFoundException notFoundException)
            {
                Response.StatusCode = 404;
                message = _localizer[notFoundException.MessageKey, notFoundException.Id];
            }
            else if (exception is BadRequestException)
            {
                Response.StatusCode = 400;
                message = exception.Message;
            }
            else
            {
                Response.StatusCode = 500;
                message = _localizer["ServerErrorMessage"];
            }

            return new ErrorRes(message);
        }
    }
}
