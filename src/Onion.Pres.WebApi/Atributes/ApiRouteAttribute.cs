using Microsoft.AspNetCore.Mvc;

namespace Onion.Pres.WebApi.Atributes;

internal class ApiRouteAttribute : RouteAttribute
{
    public ApiRouteAttribute(string template) : base($"api/{template}")
    {
    }

    public ApiRouteAttribute() : base("api")
    {
    }
}
