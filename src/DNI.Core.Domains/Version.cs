using System;

namespace DNI.Core.Domains
{
    public struct Version
    {
        public static Version Zero => new Version();

        public static bool operator ==(Version left, Version right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Version left, Version right)
        {
            return !(left == right);
        }

        public static bool TryParse(string value, out Version version)
        {
            try
            {
                version = new Version(value);
                return true;
            }
            catch (FormatException ex)
            {
                version = Zero;
                return false;
            }
        }

        public static bool IsInRange(Version version, Version minimumVersion, Version maximumVersion)
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

        public bool Equals(ref Version version)
        {
            return Major == version.Major 
                && Minor == version.Minor;
        }

        public override bool Equals(object obj)
        {
            if(!(obj is Version version))
            {
                throw new InvalidOperationException();
            }

            return Equals(ref version);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Major, Minor);
        }

        public override string ToString()
        {
            return string.Format("Major:{0}, Minor:{1}, String:\"{0}\",\"{1}\"", Major, Minor);
        }
    }
}
