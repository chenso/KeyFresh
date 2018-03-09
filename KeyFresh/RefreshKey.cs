using System;
using System.Security;
using CodeTiger;

namespace KeyFresh
{
    /// <summary>
    /// 
    /// </summary>
    public class RefreshKey
    {
        private readonly Uri _keyUri;
        private readonly IKeyProvider _keyProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyUri"></param>
        /// <param name="keyProvider"></param>
        public RefreshKey(Uri keyUri, IKeyProvider keyProvider)
        {
            Guard.ArgumentIsNotNull(nameof(keyUri), keyUri);
            Guard.ArgumentIsNotNull(nameof(keyProvider), keyProvider);

            _keyUri = keyUri;
            _keyProvider = keyProvider;
        }

        /// <summary>
        /// Gets SecureString key at key's uri
        /// </summary>
        /// <returns>SecureString Key</returns>
        public SecureString GetSecureKey()
        {
            return _keyProvider.GetSecureKey(_keyUri);
        }

        public string GetKey()
        {
            return _keyProvider.GetKey(_keyUri);
        }
    }
}
