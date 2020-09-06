using DNI.Core.Contracts;

namespace DNI.Core.Domains
{
    public class RepositoryOptions : IRepositoryOptions
    {
        public bool EnableTracking { get; set; }
        public bool UseDbContextPools { get ; set ; }
        public int PoolSize { get ; set ; }
        public bool SingulariseTableNames { get; set; }
    }
}
