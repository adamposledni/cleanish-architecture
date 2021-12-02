using Onion.Application.Services.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.Services.Common
{
    public interface ISecurityContextProvider
    {
        SecurityContext SecurityContext { get; }
    }
}
