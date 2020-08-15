using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IDefinition<TDefinitionSubject>
    {
        IDefinition<TDefinitionSubject> Add(TDefinitionSubject subject);
        IEnumerable<TDefinitionSubject> Definitions { get; }
    }
}
