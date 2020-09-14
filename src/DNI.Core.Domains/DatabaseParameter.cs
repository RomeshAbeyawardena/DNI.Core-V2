using System.Data;

namespace DNI.Core.Domains
{
    public class DatabaseParameter
    {
        public DatabaseParameter(string name, object value, ParameterDirection direction = default, bool useInWhereClause = false)
        {
            Name = name;
            Value = value;
            Direction = direction == default ? ParameterDirection.Input : direction;
            UseInWhereClause = useInWhereClause;
        }

        public string Name { get; }
        public object Value { get; }
        public bool UseInWhereClause { get; }
        public ParameterDirection Direction { get; }
    }
}
