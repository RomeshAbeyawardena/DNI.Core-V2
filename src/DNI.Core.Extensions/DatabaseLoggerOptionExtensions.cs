using DNI.Core.Contracts.Managers;
using DNI.Core.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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


            if(databaseLoggerOptions.LogStatusTableType != null)
            { 
                
                var databaseLogStatusManagerInterfaceType = typeof(IDatabaseLogStatusManager<>);
                var genericDatabaseLogStatusManagerInterfaceType = databaseLogStatusManagerInterfaceType
                        .MakeGenericType(databaseLoggerOptions.LogStatusTableType);
                    services.TryAddTransient(genericDatabaseLogStatusManagerInterfaceType, databaseLoggerOptions.DatabaseLogStatusManagerType);
            }

            return services;
        }
    }
}
