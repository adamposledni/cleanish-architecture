using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Onion.Application.Domain.Exceptions;
using Onion.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onion.WebApi.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [Route("error")]
        public ErrorRes Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            string message;

            if (exception is NotFoundException)
            {
                Response.StatusCode = 404;
                message = exception.Message;
            }
            else if (exception is BadRequestException)
            {
                Response.StatusCode = 400;
                message = exception.Message;
            }
            else
            {
                Response.StatusCode = 500;
                message = "Server error has occured";
            }

            return new ErrorRes(message, null);
        }
    }
}
