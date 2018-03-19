using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace KeyFresh
{
    /// <summary>
    /// interface for retrieving keys
    /// </summary>
    public interface IKeyProvider
    {
        /// <summary>
        /// Gets SecureString key at the given uri
        /// </summary>
        /// <param name="keyUri">vault uri for the key</param>
        /// <returns></returns>
        SecureString GetSecureKey(Uri keyUri);

        /// <summary>
        /// Gets key at the given uri
        /// </summary>
        /// <param name="keyUri">vault uri for the key</param>
        /// <returns></returns>
        string GetKey(Uri keyUri);
    }
}
