using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Onion.Pres.WebApi.Atributes;
using Onion.Shared.Exceptions;

namespace Onion.Pres.WebApi.Controllers;

// TODO: presentation contracts vs. business contracts
[ApiController]
[ProducesErrorResponse(400)]
[ProducesErrorResponse(401)]
[ProducesErrorResponse(403)]
[ProducesErrorResponse(500)]
[Produces("application/json")]
[Consumes("application/json")]
public abstract class BaseController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected ObjectResult Created(object value)
    {
        return StatusCode(201, value);
    }

    protected void ValidateEquals(object o1, object o2)
    {
        if (!o1.Equals(o2))
        {
            throw new ValidationException("Invalid request");
        }
    }
}
