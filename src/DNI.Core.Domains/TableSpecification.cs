using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

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
