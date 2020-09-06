using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Shared.Enumerations;
using System.Collections;
using System.Collections.Generic;

namespace DNI.Core.Services.Managers
{
    internal class EncryptionProfileManager : IEncryptionProfileManager
    {
        public EncryptionProfileManager(IEnumerable<KeyValuePair<EncryptionClassification, IEncryptionProfile>> keyValuePairs)
        {
            dictionary = new Dictionary<EncryptionClassification, IEncryptionProfile>(keyValuePairs);
        }

        IEncryptionProfile IReadOnlyDictionary<EncryptionClassification, IEncryptionProfile>.this[EncryptionClassification key] => dictionary[key];

        IEnumerable<EncryptionClassification> IReadOnlyDictionary<EncryptionClassification, IEncryptionProfile>.Keys => dictionary.Keys;

        IEnumerable<IEncryptionProfile> IReadOnlyDictionary<EncryptionClassification, IEncryptionProfile>.Values => dictionary.Values;

        int IReadOnlyCollection<KeyValuePair<EncryptionClassification, IEncryptionProfile>>.Count => dictionary.Count;

        bool IReadOnlyDictionary<EncryptionClassification, IEncryptionProfile>.ContainsKey(EncryptionClassification key)
        {
            return dictionary.ContainsKey(key);
        }

        IEnumerator<KeyValuePair<EncryptionClassification, IEncryptionProfile>> IEnumerable<KeyValuePair<EncryptionClassification, IEncryptionProfile>>.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        bool IReadOnlyDictionary<EncryptionClassification, IEncryptionProfile>.TryGetValue(EncryptionClassification key, out IEncryptionProfile value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        private readonly IDictionary<EncryptionClassification, IEncryptionProfile> dictionary;
    }
}
