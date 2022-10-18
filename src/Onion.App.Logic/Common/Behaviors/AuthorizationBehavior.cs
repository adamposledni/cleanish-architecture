using MediatR;
using Onion.App.Logic.Common.Attributes;
using Onion.App.Logic.Common.Security;
using Onion.Shared.Exceptions;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Onion.App.Logic.Common.Mediator.Behaviors;

internal class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ISecurityContextProvider _securityContextProvider;

    public AuthorizationBehavior(ISecurityContextProvider securityContextProvider)
    {
        _securityContextProvider = securityContextProvider;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        // no authorization
        if (!authorizeAttributes.Any())
        {
            return await next();
        }

        // no identity
        if (_securityContextProvider.SecurityContext == null)
        {
            throw new NotAuthenticatedException();
        }

        //var authorizeAttributesWithRoles = authorizeAttributes.Where(a => a.Roles.Any());
        //if (authorizeAttributesWithRoles.Any())
        //{
        //    bool authorized = false;

        //    foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles))
        //    {
        //        foreach (var role in roles)
        //        {
        //            var isInRole = );
        //            if (isInRole)
        //            {
        //                authorized = true;
        //                break;
        //            }
        //        }
        //    }

        //    // Must be a member of at least one role in roles
        //    if (!authorized)
        //    {
        //        throw new NotAuthorizedException();
        //    }
        //}

        return await next();
    }
}
