using System;
using System.Security;
using CodeTiger;

namespace KeyFresh
{
    /// <summary>
    /// Holds key uri and key provider
    /// </summary>
    public class RefreshKey
    {
        private readonly Uri _keyUri;
        private readonly IKeyProvider _keyProvider;

        /// <summary>
        /// Initialized new instance of RefreshKey
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
        /// Retrieves SecureString key at keyUri
        /// </summary>
        /// <returns>SecureString Key</returns>
        public SecureString RetrieveSecureKey()
        {
            return _keyProvider.GetSecureKey(_keyUri);
        }

        /// <summary>
        /// Retrieves plain string key found at keyUri
        /// </summary>
        /// <returns></returns>
        public string RetrieveKey()
        {
            return _keyProvider.GetKey(_keyUri);
        }
    }
}
