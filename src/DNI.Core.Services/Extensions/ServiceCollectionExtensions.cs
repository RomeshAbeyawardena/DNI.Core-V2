using DNI.Core.Contracts;
using DNI.Core.Contracts.Factories;
using DNI.Core.Services.Factories;
using DNI.Core.Services.Implementations.Data;
using Microsoft.EntityFrameworkCore;
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
            params Type[] entityTypes)
        {
            services.AddSingleton(repositoryOptions);

            var repositoryType = typeof(IRepository<>);
            var asyncRepositoryType = typeof(IAsyncRepository<>);
            var concreteRepositoryType = typeof(EntityFrameworkRepository<,>);
            var concreteAsyncRepositoryType = typeof(AsyncEntityFrameworkRepository<,>);

            foreach(var entityType in entityTypes)
            {
                var entityRepositoryType = repositoryType.MakeGenericType(entityType);
                var concreteEntityRepositoryType = concreteRepositoryType.MakeGenericType(typeof(TDbContext), entityType);

                var asyncEntityRepositoryType = asyncRepositoryType.MakeGenericType(entityType);
                var concreteAsyncEntityRepositoryType = concreteAsyncRepositoryType.MakeGenericType(typeof(TDbContext), entityType);

                services.AddScoped(entityRepositoryType, concreteEntityRepositoryType);
                services.AddScoped(asyncEntityRepositoryType, concreteAsyncEntityRepositoryType);
            }

            return services;
        }

        public static IServiceCollection RegisterRepositories<TDbContext>(
            this IServiceCollection services, 
            Action<IRepositoryOptions> repositoryOptions = null)
            where TDbContext : DbContext
        {
            var genericTypeDbSet = typeof(DbSet<>);
            var dbContextType = typeof(TDbContext);
            var properties = dbContextType.GetProperties();
            var dbSetProperties = properties.Where(property => property.PropertyType.IsGenericType);

            var entities = dbSetProperties.Select(property => property.PropertyType.GetGenericArguments().FirstOrDefault());

            return RegisterRepositories<TDbContext>(
                services, 
                RepositoryOptionsBuilder.Build(repositoryOptions ?? RepositoryOptionsBuilder.Default),
                entities.ToArray());
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IValueGeneratorFactory>(serviceProvider => new ValueGeneratorFactory(ScanGenerators<RepositoryOptions>(services)))
                .Scan(scan => scan.FromAssemblyOf<RepositoryOptions>().AddClasses().AsImplementedInterfaces());
        }

        public static IEnumerable<KeyValuePair<string, Type>> ScanGenerators<T>(IServiceCollection services)
        {
            var valueGeneratorConcreteTypes = typeof(T)
                .Assembly.GetTypes()
                .Where(type => type.GetInterface(nameof(IValueGenerator)) != null);

            foreach(var valueGeneratorConcreteType in valueGeneratorConcreteTypes)
            {
                services.AddSingleton(valueGeneratorConcreteType);
            }

            return valueGeneratorConcreteTypes
                .Select(valueGenerator => new KeyValuePair<string, Type>(valueGenerator.FullName, valueGenerator));

        }
    }
}
