using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
