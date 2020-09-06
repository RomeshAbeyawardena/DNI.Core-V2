using DNI.Core.Services.Abstractions;
using DNI.Core.Services.Extensions;

namespace DNI.Core.Services.Implementations
{
    internal class DefaultServiceBroker : ServiceBroker
    {
        public DefaultServiceBroker() : base(definitions => definitions.DescribeAssembly<ServiceRegistration>())
        {
        }
    }
}
