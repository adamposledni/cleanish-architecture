using Onion.Core.Clock;

namespace Onion.Infrastructure.Core.Clock;

public class ClockProvider : IClockProvider
{
    public DateTime Now => DateTime.UtcNow;
}
