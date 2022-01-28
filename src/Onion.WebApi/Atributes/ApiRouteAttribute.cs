using Microsoft.AspNetCore.Mvc;

namespace Onion.WebApi.Atributes;

public class ApiRouteAttribute : RouteAttribute
{
    public ApiRouteAttribute(string template) : base($"api/{template}")
    {
    }

    public ApiRouteAttribute() : base("api")
    {
    }
}
