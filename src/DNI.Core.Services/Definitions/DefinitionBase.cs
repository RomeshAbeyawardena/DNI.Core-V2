using DNI.Core.Contracts;
using DNI.Core.Shared.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Definitions
{
    [IgnoreScanning]
    public abstract class DefinitionBase<TDefinition> : IDefinition<TDefinition>
    {
        protected DefinitionBase(IEnumerable<TDefinition> definitions)
        {
            if(definitions == null)
            {
                this.definitions = new List<TDefinition>();
            }
            else
            {
                this.definitions = new List<TDefinition>(definitions);
            }
        }

        public IEnumerable<TDefinition> Definitions => definitions.ToArray();

        public IDefinition<TDefinition> Add(TDefinition subject)
        {
            if(!definitions.Contains(subject))
            { 
                definitions.Add(subject);
            }

            return this;
        }

        IEnumerator<TDefinition> IEnumerable<TDefinition>.GetEnumerator()
        {
            return definitions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return definitions.GetEnumerator();
        }

        private readonly List<TDefinition> definitions;
    }
}
