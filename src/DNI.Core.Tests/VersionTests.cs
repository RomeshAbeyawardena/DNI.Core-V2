using NUnit.Framework;
using Version = DNI.Core.Domains.Version;

namespace DNI.Core.Tests
{
    [TestFixture]
    public class VersionTests
    {
        [TestCase(1, 0, 1, 0, 2, 0)]
        [TestCase(1, 1, 1, 0, 2, 0)]
        [TestCase(1, 2, 1, 0, 2, 0)]
        [TestCase(1, 3, 1, 0, 2, 0)]
        [TestCase(1, 4, 1, 0, 2, 0)]
        [TestCase(1, 5, 1, 0, 2, 0)]
        [TestCase(1, 6, 1, 0, 2, 0)]
        [TestCase(1, 7, 1, 0, 2, 0)]
        [TestCase(1, 8, 1, 0, 2, 0)]
        [TestCase(1, 9, 1, 0, 2, 0)]
        [TestCase(2, 0, 1, 0, 2, 0)]
        public void Version_range_should_be_valid(int actualMajor, int actualMinor, int minimumMajor, int minimumMinor, int maximumMajor, int maximumMinor)
        {
            var actual = new Version(actualMajor, actualMinor);
            var minimum = new Version(minimumMajor, minimumMinor);
            var maximum = new Version(maximumMajor, maximumMinor);

            Assert.IsTrue(Version.IsInRange(actual, minimum, maximum));
        }

        [TestCase(0, 0, 1, 0, 2, 0)]
        [TestCase(3, 0, 1, 0, 2, 0)]
        [TestCase(3, 1, 1, 0, 2, 0)]
        [TestCase(3, 2, 1, 0, 2, 0)]
        [TestCase(3, 3, 1, 0, 2, 0)]
        [TestCase(3, 4, 1, 0, 2, 0)]
        [TestCase(2, 5, 1, 0, 2, 0)]
        [TestCase(2, 6, 1, 0, 2, 0)]
        [TestCase(2, 7, 1, 0, 2, 0)]
        [TestCase(2, 8, 1, 0, 2, 0)]
        [TestCase(2, 9, 1, 0, 2, 0)]
        [TestCase(2, 1, 1, 0, 2, 0)]
        public void Version_range_should_be_invalid(int actualMajor, int actualMinor, int minimumMajor, int minimumMinor, int maximumMajor, int maximumMinor)
        {
            var actual = new Version(actualMajor, actualMinor);
            var minimum = new Version(minimumMajor, minimumMinor);
            var maximum = new Version(maximumMajor, maximumMinor);

            Assert.IsFalse(Version.IsInRange(actual, minimum, maximum));
        }
    }
}
