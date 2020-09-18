﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DNI.Core.Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static IDictionary<string, string> ToDictionary(this object value)
        {
            var dictionary = new Dictionary<string, string>();

            var valueType = value.GetType();

            var properties = valueType.GetProperties();

            foreach(var property in properties)
            {
                dictionary.Add(property.Name, property.GetValue(value)?.ToString());
            }

            return dictionary;
        }

        public static Type DetermineType(this object value)
        {
            if(value == null)
                return null;

            var stringValue = value.ToString();

            if(bool.TryParse(stringValue, out var booleanValue))
            {
                return (typeof(bool));
            }

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

        public static bool IsDefault(this object value)
        {
            var determinedType = DetermineType(value);

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
