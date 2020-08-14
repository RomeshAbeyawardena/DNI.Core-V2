﻿using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Services.Managers;
using DNI.Core.Services.Implementations.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using System.Reflection;
using AutoMapper;
using MediatR;
using DNI.Core.Shared.Extensions;
using DNI.Core.Contracts.Providers;
using DNI.Core.Services.Providers;

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

        public static IServiceCollection RegisterServices(
            this IServiceCollection services, 
            IEnumerable<KeyValuePair<string, Type>> generatorKeyValuePairs = null)
        {
            var internalGeneratorKeyValuePairs = ScanAndRegisterGenerators<RepositoryOptions>(services);
            if(generatorKeyValuePairs == null)
            {
                generatorKeyValuePairs = internalGeneratorKeyValuePairs;
            }
            else
            {
                generatorKeyValuePairs = generatorKeyValuePairs.Append(internalGeneratorKeyValuePairs);
            }


            return services
                .AddSingleton<ISystemClock, SystemClock>()
                .AddSingleton<IValueGeneratorManager>(serviceProvider => new ValueGeneratorManager(generatorKeyValuePairs))
                .Scan(scan => scan.FromAssemblyOf<RepositoryOptions>().AddClasses().AsImplementedInterfaces());
        }

        public static IEnumerable<KeyValuePair<string, Type>> ScanAndRegisterGenerators<T>(IServiceCollection services)
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

        public static IServiceCollection RegisterAutoMapperProviders(
            this IServiceCollection services, 
            Action<IAssemblyDefinition> obtainAssemblyDefinitions,
            Action<IServiceProvider, IMapperConfigurationExpression> configureAutomapper = null)
        {
            services.AddSingleton<IMapperProvider, AutoMapperProvider>();

            var assemblyDefinitions = new AssemblyDefinition();
            obtainAssemblyDefinitions(assemblyDefinitions);

            return services.AddAutoMapper(configureAutomapper,assemblyDefinitions.Assemblies);
        }

        public static IServiceCollection RegisterMediatrProviders(
            this IServiceCollection services,
            Action<IAssemblyDefinition> obtainAssemblyDefinitions,
            Action<MediatRServiceConfiguration> configuremediatRServiceConfiguration = null)
        {
            services.AddSingleton<IMediatorProvider, MediatrProvider>();

            var assemblyDefinitions = new AssemblyDefinition();
            obtainAssemblyDefinitions(assemblyDefinitions);
            return services.AddMediatR(
                assemblyDefinitions.Assemblies.ToArray(), 
                configuremediatRServiceConfiguration);
        }
    }
}
