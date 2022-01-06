using Onion.Core.Clock;
using System;

namespace Onion.Infrastructure.Clock
{
    public class ClockProvider : IClockProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
