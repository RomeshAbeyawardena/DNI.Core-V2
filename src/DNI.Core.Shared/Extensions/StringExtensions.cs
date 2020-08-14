using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
