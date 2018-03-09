using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace KeyFresh
{
    public interface IKeyProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyUri">vault uri for the key</param>
        /// <remarks>Implementations should have some sort of backoff to prevent the vault service from being spammed
        /// on bad keys</remarks>
        /// <returns></returns>
        SecureString GetSecureKey(Uri keyUri);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyUri"></param>
        /// <returns></returns>
        string GetKey(Uri keyUri);
    }
}
