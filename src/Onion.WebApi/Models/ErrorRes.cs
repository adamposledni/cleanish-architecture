using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onion.WebApi.Models
{
    public class ErrorRes
    {
        public ErrorRes(string message, string details)
        {
            Message = message;
            Details = details;
        }

        public string Message { get; set; }
        public string Details { get; set; }
    }
}
