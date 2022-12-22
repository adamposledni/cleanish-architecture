namespace Cleanish.Shared.Exceptions;

public class ValidationException : BaseApplicationException
{
    public string Details { get; init; }

    public ValidationException(string details) : base("FailedValidation")
    {
        Details = details;
    }
}
