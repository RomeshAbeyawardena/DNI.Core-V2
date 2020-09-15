using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Domains;
using DNI.Core.Extensions.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace DNI.Core.Extensions
{
    public static class DatabaseLoggerOptionExtensions
    {
        public static Type GetGenericType(this DatabaseLoggerOptions databaseLoggerOptions)
        {
            var loggerType = typeof(DatabaseLogger<>);
            return loggerType.MakeGenericType(databaseLoggerOptions.LoggingDbContext);
        }

        public static IServiceCollection RegisterDatabaseLogging<TDbContext>(this IServiceCollection services, DatabaseLoggerOptions databaseLoggerOptions)
        {
            services.TryAddSingleton(databaseLoggerOptions);
            services.TryAddTransient(databaseLoggerOptions.GetGenericType());

            var databaseLogManagerInterfaceType = typeof(IDatabaseLogManager<>);
            var genericDatabaseLogManagerInterfaceType = databaseLogManagerInterfaceType
                    .MakeGenericType(databaseLoggerOptions.LogTableType);

            services
                .TryAddTransient(genericDatabaseLogManagerInterfaceType, databaseLoggerOptions.DatabaseLogManagerType);


            if (databaseLoggerOptions.LogStatusTableType != null)
            {

                var databaseLogStatusManagerInterfaceType = typeof(IDatabaseLogStatusManager<>);
                var genericDatabaseLogStatusManagerInterfaceType = databaseLogStatusManagerInterfaceType
                        .MakeGenericType(databaseLoggerOptions.LogStatusTableType);
                services.TryAddTransient(genericDatabaseLogStatusManagerInterfaceType, databaseLoggerOptions.DatabaseLogStatusManagerType);
            }

            if(databaseLoggerOptions.LogStatusManagerConfiguration != null)
            {
                services.TryAddSingleton(serviceProvider => databaseLoggerOptions.LogStatusManagerConfiguration);

                var defaultLogStatusManagerType = typeof(IDatabaseLogStatusManager<>);
                var defaultLogStatusManagerImplementationType = typeof(DefaultConfigurationLogStatusManager<>);

                var genericDefaultLogStatusManagerType = defaultLogStatusManagerType
                    .MakeGenericType(databaseLoggerOptions.ConfigurationType);

                var genericDefaultLogStatusManagerImplementationType = defaultLogStatusManagerImplementationType
                    .MakeGenericType(databaseLoggerOptions.ConfigurationType);
                services.TryAddSingleton(genericDefaultLogStatusManagerType, genericDefaultLogStatusManagerImplementationType);
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
        public static ILoggingBuilder AddDatabase<TDbContext>(this ILoggingBuilder builder, Action<DatabaseLoggerOptions> configure)
        {
            var databaseLoggerOptions = new DatabaseLoggerOptions();
            databaseLoggerOptions.ConfigureLoggingDbContext<TDbContext>();
            configure(databaseLoggerOptions);

            RegisterDatabaseLogging<TDbContext>(builder.Services, databaseLoggerOptions);

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
