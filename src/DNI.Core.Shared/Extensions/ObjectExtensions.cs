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

            if(determinedType == typeof(DateTimeOffset))
            { 
                var convertorResult = Converter.TryConvert<decimal>(value);
                return convertorResult.IsSuccessful && convertorResult.Value == default;
            }

            if(determinedType == typeof(long))
            { 
                var convertorResult = Converter.TryConvert<long>(value);
                return convertorResult.IsSuccessful && convertorResult.Value == default;
            }

            if(determinedType == typeof(decimal))
            { 
                var convertorResult =  Converter.TryConvert<decimal>(value);
                return convertorResult.IsSuccessful && convertorResult.Value == default;
            }

            return false;
        }
    }
}
