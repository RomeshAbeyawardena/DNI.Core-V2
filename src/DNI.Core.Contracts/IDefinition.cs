using System.Collections.Generic;

namespace DNI.Core.Contracts
{
    public interface IDefinition<TDefinitionSubject> : IEnumerable<TDefinitionSubject>
    {
        IDefinition<TDefinitionSubject> Add(TDefinitionSubject subject);
        IEnumerable<TDefinitionSubject> Definitions { get; }
    }
}
