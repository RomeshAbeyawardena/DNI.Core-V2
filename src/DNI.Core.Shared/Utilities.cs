﻿using System;

namespace DNI.Core.Shared
{
    public static class Utilities
    {
        public static T Copy<T>(T original)
        {
            if(original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

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
