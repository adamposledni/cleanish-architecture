using Onion.Shared.Clock;

namespace Onion.Impl.Shared.Clock;

internal class ClockProvider : IClockProvider
{
    public DateTime Now => DateTime.UtcNow;
}
