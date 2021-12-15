using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.DataAccess.Exceptions
{
    public class PageOutOfBoundsException : BadRequestException
    {
        private const string MESSAGE_KEY = "PageOutOfBounds";
        public PageOutOfBoundsException() : base(MESSAGE_KEY)
        {
        }
    }
}
