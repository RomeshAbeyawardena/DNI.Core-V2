using DNI.Core.Shared.Enumerations;
using System;

namespace DNI.Core.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EncryptAttribute : Attribute
    {
        public EncryptAttribute(EncryptionMethod encryptionMethod, EncryptionClassification encryptionClassification)
        {
            EncryptionMethod = encryptionMethod;
            EncryptionClassification = encryptionClassification;
        }

        public EncryptionMethod EncryptionMethod { get; }
        public EncryptionClassification EncryptionClassification { get; }
    }
}
