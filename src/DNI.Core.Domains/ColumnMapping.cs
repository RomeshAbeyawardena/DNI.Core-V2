using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Domains
{
    public class ColumnMapping
    {
        public ColumnMapping(string key, ref DbColumn dbColumn)
        {
            Key = key;
            Column = dbColumn;
        }

        public string Key { get; }
        public DbColumn Column { get; }
        
        public static PropertyInfo GetProperty(ColumnMapping columnMapping, object instance)
        {
            var instanceType = instance.GetType();

            var property = instanceType.GetProperty(columnMapping.Key);

            return property;
        }
    }
}
