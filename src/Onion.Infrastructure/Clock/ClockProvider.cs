using Onion.Core.Clock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Infrastructure.Clock
{
    public class ClockProvider : IClockProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
