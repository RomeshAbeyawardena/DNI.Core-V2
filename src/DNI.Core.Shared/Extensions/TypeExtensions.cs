using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Shared.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetUnderlyingSystemType(this Type type, object value)
        {
            var propertyInfo = type.GetProperty("UnderlyingSystemType");
            return (Type)propertyInfo.GetValue(value, null);
        }
    }
}
