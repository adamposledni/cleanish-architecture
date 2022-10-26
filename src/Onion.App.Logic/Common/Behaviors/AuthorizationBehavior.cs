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

        // role security
        var authorizeAttributeWithRoles = authorizeAttributes.Where(a => a.Roles != null && a.Roles.Any()).FirstOrDefault();
        if (authorizeAttributeWithRoles != null)
        {
            bool authorized = false;
            foreach (var role in authorizeAttributeWithRoles.Roles)
            {
                if (role == _securityContextProvider.SecurityContext.Role)
                {
                    authorized = true;
                    break;
                }
            }

            // invalid role
            if (!authorized)
            {
                throw new NotAuthorizedException();
            }
        }

        return await next();
    }
}
