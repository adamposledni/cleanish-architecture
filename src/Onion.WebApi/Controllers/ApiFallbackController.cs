using Microsoft.AspNetCore.Mvc;
using Onion.WebApi.Atributes;
using Onion.WebApi.Exceptions;

namespace Onion.WebApi.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ApiFallbackController : BaseController
{
    [ApiRoute("{**rest}")]
    public IActionResult Fallback() => throw new PathNotFoundException();
}
