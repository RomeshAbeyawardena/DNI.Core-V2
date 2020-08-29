using DNI.Core.Contracts;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    public static class RepositoryOptionsBuilder
    {
        public static IRepositoryOptions Build(Action<IRepositoryOptions> builderAction)
        {
            var repositoryOptions = new RepositoryOptions();

            builderAction(repositoryOptions);

            return repositoryOptions;
        }

        public static Action<IRepositoryOptions> Default => options => { options.EnableTracking = false; };
    }

    [IgnoreScanning]
    internal class RepositoryOptions : IRepositoryOptions
    {
        public bool EnableTracking { get; set; }
        public bool UseDbContextPools { get ; set ; }
        public int PoolSize { get ; set ; }
        public bool SingulariseTableNames { get; set; }
    }
}
