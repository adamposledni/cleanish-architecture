using Onion.App.Data.Database.Entities.Fields;

namespace Onion.App.Logic.Common.Security;

public class SecurityContext
{
    public Guid SubjectId { get; init; }
    public UserRole[] Roles { get; init; }

    public SecurityContext(Guid subjectId)
    {
        SubjectId = subjectId;
    }
}
