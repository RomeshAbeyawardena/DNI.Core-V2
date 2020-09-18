using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Domains;
using DNI.Core.Extensions.Managers;
using DNI.Core.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DNI.Core.Extensions
{
    public static class DatabaseLoggerOptionExtensions
    {
        public static Type GetGenericType(this DatabaseLoggerOptions databaseLoggerOptions)
        {
            var loggerType = typeof(DatabaseLogger<>);
            return loggerType.MakeGenericType(databaseLoggerOptions.LoggingDbContext);
        }

        public static IServiceCollection RegisterDatabaseLogging<TDbContext>(
            this IServiceCollection services, 
            DatabaseLoggerOptions databaseLoggerOptions,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            services.TryAddSingleton(databaseLoggerOptions);
            services.TryAdd(databaseLoggerOptions.GetGenericType(), serviceLifetime);

            var databaseLogManagerInterfaceType = typeof(IDatabaseLogManager<>);
            var genericDatabaseLogManagerInterfaceType = databaseLogManagerInterfaceType
                    .MakeGenericType(databaseLoggerOptions.LogTableType);

            services
                .TryAdd(genericDatabaseLogManagerInterfaceType, databaseLoggerOptions.DatabaseLogManagerType, serviceLifetime);


            if (databaseLoggerOptions.LogStatusTableType != null)
            {

                var databaseLogStatusManagerInterfaceType = typeof(IDatabaseLogStatusManager<>);
                var genericDatabaseLogStatusManagerInterfaceType = databaseLogStatusManagerInterfaceType
                        .MakeGenericType(databaseLoggerOptions.LogStatusTableType);
                services.TryAdd(genericDatabaseLogStatusManagerInterfaceType, databaseLoggerOptions.DatabaseLogStatusManagerType, serviceLifetime);
            }

            if(databaseLoggerOptions.RegisterDefaultLogStatusService 
                || databaseLoggerOptions.LogStatusManagerConfiguration != null)
            {
                if(databaseLoggerOptions.RegisterDefaultLogStatusService)
                    services.TryAddSingleton(typeof(ILogStatusConfiguration), databaseLoggerOptions.ConfigurationType);
                else 
                    services.TryAddSingleton(serviceProvider => 
                        databaseLoggerOptions.LogStatusManagerConfiguration(serviceProvider));
                
                var defaultLogStatusManagerType = typeof(IDatabaseLogStatusManager<>);
                var defaultLogStatusManagerImplementationType = typeof(DefaultConfigurationLogStatusManager<>);

                var genericDefaultLogStatusManagerType = defaultLogStatusManagerType
                    .MakeGenericType(databaseLoggerOptions.ConfigurationType);

                var genericDefaultLogStatusManagerImplementationType = defaultLogStatusManagerImplementationType
                    .MakeGenericType(databaseLoggerOptions.ConfigurationType);
                services.TryAdd(genericDefaultLogStatusManagerType, genericDefaultLogStatusManagerImplementationType, serviceLifetime);
            }

            foreach (var service in services)
            {
                Debug.WriteLine(service.ServiceType.FullName);
            }

            return services;
        }

        /// <summary>
        /// Adds a database logger to the current list of loggers
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddDatabase<TDbContext>(
            this ILoggingBuilder builder, 
            Action<DatabaseLoggerOptions> configure,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            var databaseLoggerOptions = new DatabaseLoggerOptions();
            databaseLoggerOptions.ConfigureLoggingDbContext<TDbContext>();
            configure(databaseLoggerOptions);

            RegisterDatabaseLogging<TDbContext>(builder.Services, databaseLoggerOptions, serviceLifetime);

            return AddProvider<DatabaseLoggerProvider>(builder);
        }

        public static IServiceCollection RegisterDapperContexts(this IServiceCollection services)
        {
            services.TryAddScoped(typeof(IDapperContext<>), typeof(DapperContext<>));
            return services;
        }

        public static ILoggingBuilder AddProvider<T>(this ILoggingBuilder builder)
            where T : class, ILoggerProvider
        {
            builder.Services.AddTransient<ILoggerProvider, T>();
            return builder;
        }

    }
}
