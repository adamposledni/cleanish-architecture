using Cleanish.App.Data.Database.Entities.Fields;

namespace Cleanish.App.Logic.Common.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
internal class AuthorizeAttribute : Attribute
{
    public AuthorizeAttribute()
    {
    }

    public UserRole[] Roles { get; set; }
}
