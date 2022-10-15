namespace Onion.Shared.Exceptions;

public class ValidationException : Exception
{
    public string MessageKey { get; init; } = "FailedValidation";
    public string Details { get; set; }

    public ValidationException(string details) : base()
    {
        Details = details;
    }
}
