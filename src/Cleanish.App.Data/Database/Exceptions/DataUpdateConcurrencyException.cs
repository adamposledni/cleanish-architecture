using Cleanish.Shared.Exceptions;

namespace Cleanish.App.Data.Database.Exceptions;

public class DataUpdateConcurrencyException : BadLogicException
{
    public DataUpdateConcurrencyException() : base("DataUpdateConcurrency")
    {
    }
}
