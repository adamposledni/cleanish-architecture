using Onion.Shared.Exceptions;

namespace Onion.App.Data.Database.Exceptions;

public class DataUpdateConcurrencyException : BadLogicException
{
    public DataUpdateConcurrencyException() : base("DataUpdateConcurrency")
    {
    }
}
