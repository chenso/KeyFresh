using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace KeyFresh
{
    internal static class SecureStringExtensions
    {
        /// <summary>
        /// Convert secure string to plain text
        /// </summary>
        /// <param name="secureText"></param>
        /// <returns></returns>
        public static string ConvertToUnsecureString(this SecureString secureText)
        {
            if (secureText == null)
                throw new ArgumentNullException(nameof(secureText));

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureText);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
