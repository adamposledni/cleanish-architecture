using Onion.Core.Clock;

namespace Onion.Infrastructure.Clock;

public class ClockProvider : IClockProvider
{
    public DateTime Now => DateTime.UtcNow;
}
