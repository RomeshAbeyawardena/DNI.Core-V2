using System;
using DNI.Core.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Shared
{
    
    public static class Converter
    {
        
        public static IConvertValue<TValue> TryConvert<TValue>(object value)
        {
            try
            {
                if(Convert.ChangeType(value, typeof(TValue)) is TValue convertedValue)
                {
                    return GetValue(convertedValue);
                }

                throw new InvalidCastException();
            }
            catch(Exception ex)
            {
                if(ex is InvalidCastException 
                    || ex is FormatException
                    || ex is OverflowException
                    || ex is ArgumentNullException)
                {
                    GetValue<TValue>(ex);
                }
                throw;
            }
        }

        public static IConvertValue<TValue> GetValue<TValue>(TValue value)
        {
            return new ConvertedValue<TValue>(value);
        }

        
        public static IConvertValue<TValue> GetValue<TValue>(Exception exception)
        {
            return new ConvertedValue<TValue>(exception);
        }

        public static Type DetermineType(object value)
        {
            if(value == null)
                return null;

            var stringValue = value.ToString();

            if (DateTimeOffset.TryParse(stringValue, out var dateValue))
            { 
                return typeof(DateTimeOffset);
            }

            if (stringValue.All(str => char.IsNumber(str)) 
                && long.TryParse(stringValue, out var longValue))
            { 
                return typeof(long);
            }

            if (stringValue.Any(str => str == '.') 
                && decimal.TryParse(stringValue, out var decimalValue))
            { 
                return typeof(long);
            }

            return typeof(string);
        }
    }
}
