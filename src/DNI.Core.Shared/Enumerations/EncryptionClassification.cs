using System;

namespace DNI.Core.Shared.Enumerations
{
    [Flags]
    public enum EncryptionClassification
    {
        Personal = 4,
        Common = 2,
        Shared = 1,
        Unclassified = 0
    }
}
