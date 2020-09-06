using DNI.Core.Contracts;
using DNI.Core.Domains;
using System;

namespace DNI.Core.Services.Builders
{
    public static class EncryptionProfileBuilder
    {
        public static IEncryptionProfile BuildProfile(Action<IEncryptionProfile> buildProfile)
        {
            var encryptionProfile = new EncryptionProfile();
            buildProfile(encryptionProfile);
            return encryptionProfile;
        }

    }
}
