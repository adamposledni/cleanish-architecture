namespace Onion.Core.Utils;

public static class Guard
{
    public static void IsNotNull(object argument, string argumentName)
    {
        if (argument == null)
            throw new ArgumentNullException(argumentName);
    }

    public static void IsNotNullOrEmpty(IEnumerable<object> argument, string argumentName)
    {
        IsNotNull(argument, argumentName);
        IsNotEmpty(argument, argumentName);
    }

    public static void IsNotEmpty(IEnumerable<object> argument, string argumentName)
    {
        if (!argument.Any())
            throw new ArgumentException("Argument is empty collection", argumentName);
    }

    public static void IsNotNullOrWhiteSpace(string argument, string argumentName)
    {
        IsNotNull(argument, argumentName);
        if (string.IsNullOrWhiteSpace(argument))
            throw new ArgumentException("Argument is empty or consists only of white-space characters", argumentName);
    }
}