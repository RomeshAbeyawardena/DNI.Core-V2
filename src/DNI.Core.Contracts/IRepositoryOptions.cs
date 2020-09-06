namespace DNI.Core.Contracts
{
    public interface IRepositoryOptions
    {
        bool EnableTracking { get; set; }
        bool UseDbContextPools { get; set; }
        int PoolSize { get; set; }
        bool SingulariseTableNames { get; set; }
    }
}
