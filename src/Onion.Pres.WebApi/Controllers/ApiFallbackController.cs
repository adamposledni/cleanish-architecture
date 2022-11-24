using Microsoft.AspNetCore.Mvc;
using Onion.Pres.WebApi.Atributes;
using Onion.Pres.WebApi.Exceptions;

namespace Onion.Pres.WebApi.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ApiFallbackController : BaseController
{
    [ApiRoute("{**rest}")]
    public IActionResult Fallback() => throw new PathNotFoundException();
}
