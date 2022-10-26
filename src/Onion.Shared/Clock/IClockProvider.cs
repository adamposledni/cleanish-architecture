namespace Onion.Shared.Clock;

public interface IClockProvider
{
    DateTime Now { get; }
}