using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Managers
{
    public interface IEncryptionProfileManager : IReadOnlyDictionary<Shared.Enumerations.EncryptionClassification, IEncryptionProfile>
    {
        
    }
}
