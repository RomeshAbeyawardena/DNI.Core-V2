﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> value)
        {
            return value == null || !value.Any();
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> items, IEnumerable<T> itemsToAppend)
        {
            var itemList = new List<T>(items);
            itemList.AddRange(itemsToAppend);
            return itemList.ToArray();
        }
    }
}
