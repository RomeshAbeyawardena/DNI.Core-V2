using DNI.Core.Contracts;
using DNI.Core.Services.Implementations.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterRepositories<TDbContext>(
            this IServiceCollection services, 
            IRepositoryOptions repositoryOptions, 
            params object[] entities)
        {
            services.AddSingleton(repositoryOptions);

            var repositoryType = typeof(IRepository<>);
            var concreteRepositoryType = typeof(AsyncEntityFrameworkRepository<,>);

            foreach(var entity in entities)
            {
                var entityType = entity.GetType();
                var entityRepositoryType = repositoryType.MakeGenericType(entity.GetType());
                var concreteEntityRepositoryType = concreteRepositoryType.MakeGenericType(typeof(TDbContext), entityType);

                services.AddScoped(entityRepositoryType, concreteEntityRepositoryType);
            }

            return services;
        }
    }
}
