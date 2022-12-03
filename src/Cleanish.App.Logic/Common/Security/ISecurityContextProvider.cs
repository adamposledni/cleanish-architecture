namespace Cleanish.App.Logic.Common.Security;

public interface ISecurityContextProvider
{
    SecurityContext SecurityContext { get; }

    Guid GetSubjectId();
}
