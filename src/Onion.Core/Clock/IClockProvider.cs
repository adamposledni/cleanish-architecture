using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Core.Clock
{
    public interface IClockProvider
    {
        DateTime Now { get; }
    }
}
