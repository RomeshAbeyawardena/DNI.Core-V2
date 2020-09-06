using DNI.Core.Services.Abstractions;
using DNI.Core.Services.Extensions;

namespace TestWebApp
{
    public class AppServiceBroker : ServiceBroker
    {
        public AppServiceBroker() 
            : base(definition => definition.DescribeAssembly<AppServiceBroker>())
        {
        }
    }
}
