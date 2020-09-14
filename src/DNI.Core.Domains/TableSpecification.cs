using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data;

namespace DNI.Core.Domains
{
    public class TableSpecification
    {
        public string Schema { get; set; }
        public string TableName { get; set; }
        public IEnumerable<DbColumn> Columns { get; set; }
        public IEnumerable<ColumnMapping> ColumnMappings { get; set; }

        public DbColumn GetColumn(string key)
        {
            return ColumnMappings
                .FirstOrDefault(columnMapping => columnMapping.Key == key)?.Column;
        }
    }
}
