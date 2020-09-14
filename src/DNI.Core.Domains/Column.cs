using System;

namespace DNI.Core.Domains
{
    public class Column
    {
        public Column(string columnName, Type type, bool isKey = default, bool isIdentity = default)
        {
            IsIdentity = isIdentity;
            ColumnName = columnName;
            Type = type;
            IsKey = isKey;
        }

        public Type Type { get; set; }
        public string ColumnName { get; set; }
        public bool IsKey { get; set; }
        public bool IsIdentity { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is Column column)
            {
                return Equals(column);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected virtual bool Equals(Column column)
        {
            return IsIdentity == column.IsIdentity
                && ColumnName == column.ColumnName
                && Type == column.Type
                && IsKey == column.IsKey;
        }
    }
}
