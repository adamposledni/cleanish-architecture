namespace Onion.App.Logic.Common.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
internal class AuthorizeAttribute : Attribute
{
    public AuthorizeAttribute()
    {
    }

    //public UserRole[] Roles { get; set; }
}
