using Microsoft.AspNetCore.Mvc;
using Onion.Application.DataAccess.Exceptions.Common;

namespace Onion.WebApi.Extensions;

public static class ActionContextExtensions
{
    public static IActionResult HandleModelValidationErrors(this ActionContext actionContext)
    {
        var errorMessages = actionContext.ModelState.Values
            .Where(v => v.Errors.Count > 0)
            .SelectMany(v => v.Errors)
            .Select(v => v.ErrorMessage);
        var errors = string.Join(" ", errorMessages);

        throw new InvalidRequestException(errors);
    }
}
