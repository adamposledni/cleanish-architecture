using Microsoft.AspNetCore.Mvc;
using Cleanish.Pres.WebApi.Atributes;
using Cleanish.Pres.WebApi.Exceptions;

namespace Cleanish.Pres.WebApi.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ApiFallbackController : BaseApiController
{
    [ApiRoute("{**rest}")]
    public IActionResult Fallback() => throw new PathNotFoundException();
}
