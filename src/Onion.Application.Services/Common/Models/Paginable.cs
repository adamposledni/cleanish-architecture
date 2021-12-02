using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Application.Services.Common.Models
{
    public class Paginable<T>
    {
        public int Size { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int Number { get; set; }

        public IList<T> Data { get; set; }
    }
}
