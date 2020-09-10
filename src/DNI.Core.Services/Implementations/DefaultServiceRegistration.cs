using DNI.Core.Contracts.Managers;
using DNI.Core.Services.Abstractions;
using DNI.Core.Services.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using DNI.Core.Shared.Attributes;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.IO;
using DNI.Core.Shared.Enumerations;
using DNI.Core.Contracts.Services;
using DNI.Core.Services.Implementations.Cache;
using Microsoft.Extensions.Caching.Distributed;
using DNI.Core.Services.Builders;
using System.Reactive.Subjects;
using DNI.Core.Shared.Extensions;

namespace DNI.Core.Services.Implementations
{
    public class DefaultServiceRegistration : ServiceRegistration
    {
        public override void RegisterServices(IServiceCollection services)
        {
            
            bool HasAttribute(Type type)
            {
                var ignoreScanningAttributeType = typeof(IgnoreScanningAttribute);
                var attributes = type.CustomAttributes;
                Debug.WriteLine("Inspecting {0}...", args: type.Name);
                foreach(var attribute in attributes)
                {
                    Debug.WriteLine("{0}: {1}", type.Name, attribute.AttributeType.Name);
                }
                
                var result = attributes.Count() == 0 
                    || attributes.Any(attribute => attribute.AttributeType != ignoreScanningAttributeType);
                return result;
            }
            
            var generatorKeyValuePairs = Extensions.ServiceCollectionExtensions.ScanAndRegisterGenerators<RepositoryOptions>(services);

            services
                .AddSingleton(typeof(ISubject<>), typeof(Subject<>))
                .AddSingleton(TypeCollector.Default)
                .AddSingleton<RecyclableMemoryStreamManager>()
                .AddSingleton<DistributedCacheService>()
                .AddSingleton<ISecurityTokenValidator>(new JwtSecurityTokenHandler())
                .AddSingleton<ISystemClock, SystemClock>()
                .AddSingleton<IEnumerable<KeyValuePair<CacheType, Type>>>(serviceProvider => {                    
                    return new CacheManager(serviceProvider, new ListBuilder<KeyValuePair<CacheType, Type>>()
                        .Add(KeyValuePair.Create(CacheType.DistributedCache, typeof(DistributedCacheService)))
                        .ToArray());
                })
                .AddSingleton<IValueGeneratorManager>(serviceProvider =>  { 
                    var customValueGenerators = serviceProvider.GetService<IEnumerable<KeyValuePair<string, Type>>>(); 

                    if(customValueGenerators != null)
                    { 
                        generatorKeyValuePairs = generatorKeyValuePairs.Append(customValueGenerators);
                    }

                    var valueGeneratorManager = new ValueGeneratorManager(serviceProvider, generatorKeyValuePairs); 
                    return valueGeneratorManager;
                })
                .Scan(scan => scan.FromAssemblyOf<RepositoryOptions>()
                .AddClasses(filter => filter.Where(HasAttribute), true)
                .AsMatchingInterface());

        }
    }
}
