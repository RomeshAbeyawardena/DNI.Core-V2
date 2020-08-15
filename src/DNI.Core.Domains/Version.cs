using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Domains
{
    public struct Version
    {
        public static bool operator ==(Version left, Version right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Version left, Version right)
        {
            return !(left == right);
        }

        public static bool IsInRange(ref Version version, ref Version minimumVersion, ref Version maximumVersion)
        {
            static int getVersionSum(Version ver)
            {
                var versionSum = ver.Major * 1000 + ver.Minor * 10;
                return versionSum;
            }

            var currentSum = getVersionSum(version);
            var minimumSum = getVersionSum(minimumVersion);
            var maximumSum = getVersionSum(maximumVersion);

            return currentSum >= minimumSum && currentSum <= maximumSum;
        }

        public Version(string version)
        {
            var versionValues = version.Split(".", StringSplitOptions.RemoveEmptyEntries);

            if (versionValues.Length != 2
                || !int.TryParse(versionValues[0], out var major)
                || !int.TryParse(versionValues[1], out var minor))
            {
                throw new FormatException();
            }

            Major = major;
            Minor = minor;
        }

        public Version(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }

        public int Major { get; }
        public int Minor { get; }

        public override bool Equals(object obj)
        {
            if(!(obj is Version version))
            {
                throw new InvalidOperationException();
            }

            return Major == version.Major 
                && Minor == version.Minor;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Major, Minor);
        }
    }
}
