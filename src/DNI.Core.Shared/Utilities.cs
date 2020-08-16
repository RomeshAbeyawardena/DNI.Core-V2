using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Shared
{
    public static class Utilities
    {
        public static T Copy<T>(T original)
        {
            var type = typeof(T);
            var copy = (T)Activator.CreateInstance(typeof(T));

            foreach(var property in type.GetProperties())
            {
                var value = property.GetValue(original);

                if(value != null)
                {
                    property.SetValue(copy, value);
                }
            }

            return copy;
        }
    }
}
