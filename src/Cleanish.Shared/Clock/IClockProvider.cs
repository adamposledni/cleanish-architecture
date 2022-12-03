namespace Cleanish.Shared.Clock;

public interface IClockProvider
{
    DateTime Now { get; }
}