using System;
using System.Collections.Generic;
using System.Text;

namespace Onion.Core.Utils
{
    public static class Guard
    {
        public static void NotNull<T>(T value, string paramName)
        {
            if (value == null) 
                throw new ArgumentNullException(paramName);
        }

        public static void StringNotNullOrEmpty(string value, string paramName)
        {
            if (string.IsNullOrEmpty(value)) 
                throw new ArgumentException("String value is null or empty", paramName);
        }

        public static void CollectionNotNullOrEmpty<T>(ICollection<T> value, string paramName)
        {
            if (value == null || value.Count <= 0)
                throw new ArgumentException("Collection is null or empty", paramName);
        }
    }
}
