using DNI.Core.Contracts;
using DNI.Core.Services.Abstractions;
using DNI.Core.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
