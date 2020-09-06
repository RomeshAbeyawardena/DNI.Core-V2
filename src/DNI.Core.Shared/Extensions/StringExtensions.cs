using System.Collections.Generic;
using System.Text;

namespace DNI.Core.Shared.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<byte> GetBytes(this string value, Encoding encoding = null)
        {
            if(encoding == null)
            {
                encoding = Encoding.Default;
            }

            return encoding.GetBytes(value);
        }
    }
}
