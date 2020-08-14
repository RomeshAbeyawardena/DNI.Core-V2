using DNI.Core.Contracts;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

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
