using Cleanish.Shared.Exceptions;

namespace Cleanish.Pres.WebApi.Exceptions;

internal class PathNotFoundException : NotFoundException
{
    public PathNotFoundException() : base("PathNotFound")
    {
    }
}
