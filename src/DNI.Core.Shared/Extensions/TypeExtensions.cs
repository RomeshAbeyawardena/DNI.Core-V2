using System;
using System.Linq;

namespace DNI.Core.Shared.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetUnderlyingSystemType(this Type type, object value)
        {
            var propertyInfo = type.GetProperty("UnderlyingSystemType");
            return (Type)propertyInfo.GetValue(value, null);
        }

        public static Type GetTypeByName(this string name)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
            {
                var tt = assembly.GetType(name);
                if (tt != null)
                {
                    return tt;
                }
            }

            return null;
        }
    }
}
