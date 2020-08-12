using DNI.Core.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

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
