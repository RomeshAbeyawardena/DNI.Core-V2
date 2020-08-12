using DNI.Core.Contracts;
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

    internal class RepositoryOptions : IRepositoryOptions
    {
        public bool EnableTracking { get; set; }
    }
}
