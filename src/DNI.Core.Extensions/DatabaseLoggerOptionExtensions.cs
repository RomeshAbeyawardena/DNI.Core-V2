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
        public static IServiceCollection RegisterDatabaseLogging<TDbContext>(IServiceCollection services, DatabaseLoggerOptions databaseLoggerOptions)
        {
            services.TryAddTransient(databaseLoggerOptions.GetGenericType());

            return services;
        }

         public static IQueryable Query(this DbContext context, string entityName) =>
            context.Query(context.Model.FindEntityType(entityName).ClrType);

        internal static readonly MethodInfo SetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set));

        public static IQueryable Query(this DbContext context, Type entityType) =>
            (IQueryable)SetMethod.MakeGenericMethod(entityType).Invoke(context, null);
    }
}
