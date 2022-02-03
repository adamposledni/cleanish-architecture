namespace Onion.Core.Helpers;

public static class Guard
{
    public static void NotNull(object argument, string argumentName)
    {
        if (argument == null)
            throw new ArgumentNullException(argumentName);
    }

    public static void NotNullOrEmpty<T>(IEnumerable<T> argument, string argumentName)
    {
        NotNull(argument, argumentName);
        NotEmpty(argument, argumentName);
    }

    private static void NotEmpty<T>(IEnumerable<T> argument, string argumentName)
    {
        if (!argument.Any())
            throw new ArgumentException("Collection is empty", argumentName);
    }

    public static void NotNullOrEmptyOrWhiteSpace(string argument, string argumentName)
    {
        NotNull(argument, argumentName);

        if (string.IsNullOrWhiteSpace(argument))
            throw new ArgumentException("String is empty or consists only of white-space characters"); 
    }

    public static void Max(int argument, int maxValue, string argumentName)
    {
        if (argument > maxValue)
            throw new ArgumentException($"Number is greater than {maxValue}", argumentName);
    }

    public static void Min(int argument, int minValue, string argumentName)
    {
        if (argument < minValue)
            throw new ArgumentException($"Number is smaller than {minValue}", argumentName);
    }

    public static void Between(int argument, int minValue, int maxValue, string argumentName)
    {
        Min(argument, minValue, argumentName);
        Max(argument, maxValue, argumentName);
    }
}