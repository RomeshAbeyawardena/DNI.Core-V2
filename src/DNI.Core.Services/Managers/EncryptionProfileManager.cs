using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Services.Abstractions;
using DNI.Core.Shared.Enumerations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DNI.Core.Services.Managers
{
    internal class EncryptionProfileManager : ImplementationFactoryBase<EncryptionClassification, IEncryptionProfile>, IEncryptionProfileManager
    {
        public EncryptionProfileManager(IEnumerable<KeyValuePair<EncryptionClassification, IEncryptionProfile>> keyValuePairs)
            : base(keyValuePairs)
        {
            
        }

    }
}
