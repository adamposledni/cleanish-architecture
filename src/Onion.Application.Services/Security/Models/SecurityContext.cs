namespace Onion.Application.Services.Security.Models;

public class SecurityContext
{
    public Guid SubjectId { get; init; }
    public SecurityContextType Type { get; init; }

    public SecurityContext(Guid subjectId, SecurityContextType type)
    {
        SubjectId = subjectId;
        Type = type;
    }
}
