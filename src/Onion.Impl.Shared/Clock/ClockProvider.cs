using Onion.Shared.Clock;

namespace Onion.Impl.Shared.Clock;

public class ClockProvider : IClockProvider
{
    public DateTime Now => DateTime.UtcNow;
}
