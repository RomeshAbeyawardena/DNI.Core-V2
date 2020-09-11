using DNI.Core.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace DNI.Core.Services.Extensions
{
    public static class EnumerableExtensions
    {
        public static IDictionary<string, string> ToDictionary(this IEnumerable<IParameter> parameters)
        {
            return parameters.ToDictionary(parameter => parameter.Name, parameter => parameter.Value);
        }
    }
}
