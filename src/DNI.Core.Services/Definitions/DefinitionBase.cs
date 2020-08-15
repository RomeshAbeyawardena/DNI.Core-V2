using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Definitions
{
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

        protected List<TDefinition> definitions;
    }
}
