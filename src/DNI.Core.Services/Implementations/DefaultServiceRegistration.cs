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
                .AddSingleton<RecyclableMemoryStreamManager>()
                .AddSingleton<ISecurityTokenValidator>(new JwtSecurityTokenHandler())
                .AddSingleton<ISystemClock, SystemClock>()
                .AddSingleton<IValueGeneratorManager>(serviceProvider =>  { 
                    var customValueGenerators = serviceProvider.GetService<IEnumerable<KeyValuePair<string, Type>>>(); 
                    var valueGeneratorManager = new ValueGeneratorManager(generatorKeyValuePairs); 
                    if(customValueGenerators != null)
                    {
                        valueGeneratorManager.AppendValueGenerators(customValueGenerators);
                    }
                    return valueGeneratorManager;
                })
                .Scan(scan => scan.FromAssemblyOf<RepositoryOptions>()
                .AddClasses(filter => filter.Where(HasAttribute), true)
                .AsMatchingInterface());

        }
    }
}
