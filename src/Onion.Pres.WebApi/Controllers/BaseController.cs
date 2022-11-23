using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Onion.Pres.WebApi.Atributes;
using Onion.Shared.Exceptions;
using System.Threading;

namespace Onion.Pres.WebApi.Controllers;

[ApiController]
[ProducesErrorResponse(400)]
[ProducesErrorResponse(401)]
[ProducesErrorResponse(403)]
[ProducesErrorResponse(500)]
[Produces("application/json")]
[Consumes("application/json")]
internal abstract class BaseController : ControllerBase
{
    private ISender _mediator = null;
    
    protected ObjectResult Created(object value)
    {
        return StatusCode(201, value);
    }

    protected NoContentResult NoContent(object value)
    {
        return NoContent();
    }

    protected async Task<TResponse> Mediate<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var mediator = _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
        return await mediator.Send(request, cancellationToken);
    }

    protected void ValidateEquals(object o1, object o2)
    {
        if (!o1.Equals(o2))
        {
            throw new ValidationException("Invalid request");
        }
    }
}
