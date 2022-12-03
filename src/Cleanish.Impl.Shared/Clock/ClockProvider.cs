using Cleanish.Shared.Clock;

namespace Cleanish.Impl.Shared.Clock;

public class ClockProvider : IClockProvider
{
    public DateTime Now => DateTime.UtcNow;
}
