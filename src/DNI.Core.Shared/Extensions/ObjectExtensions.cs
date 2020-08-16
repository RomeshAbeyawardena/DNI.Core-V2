using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsInt()
        {
            throw new NotSupportedException();
        }

        public static bool IsDefault(this object value)
        {
            var determinedType = Converter.DetermineType(value);

            if(determinedType == typeof(bool))
            { 
                return (bool)value == default;
            }

            if(determinedType == typeof(DateTimeOffset))
            { 
                return (DateTimeOffset)value == default;
            }

            if(determinedType == typeof(long))
            { 
                return Convert.ToInt32(value) == default;
            }

            if(determinedType == typeof(decimal))
            { 
                return (decimal)value == default;
            }

            return false;
        }
    }
}
