using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace KeyFresh
{
    internal static class StringExtensions
    {
        public static SecureString ConvertToSecureString(this string unsecureString)
        {
            var secure = new SecureString();
            if (unsecureString == null) throw new ArgumentNullException(nameof(unsecureString));

            return unsecureString.Aggregate(new SecureString(), AppendChar, MakeReadOnly);
        }

        private static SecureString MakeReadOnly(SecureString ss)
        {
            ss.MakeReadOnly();
            return ss;
        }

        private static SecureString AppendChar(SecureString ss, char c)
        {
            ss.AppendChar(c);
            return ss;
        }
    }
}
