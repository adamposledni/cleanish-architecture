using System.Reflection;

namespace Onion.Shared.Extensions;

public static class AssemblyExtensions
{
    public static IEnumerable<Type> GetDerivedTypes<T>(this Assembly assembly, bool excludeSelf = true)
    {
        var baseType = typeof(T);
        var derivedTypes = assembly.GetTypes().Where(t =>
        {
            return baseType.IsAssignableFrom(t) &&
                   (!excludeSelf || t != baseType);
        });
        return derivedTypes;
    }
}
