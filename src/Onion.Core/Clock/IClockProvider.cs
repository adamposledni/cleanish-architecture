namespace Onion.Core.Clock;

public interface IClockProvider
{
    DateTime Now { get; }
}
