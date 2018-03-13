using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace KeyFresh.Azure.IntegrationTests
{
    public class KeyVaultMock : IKeyProvider
    {
        private string _wrongKey;
        private string _rightKey;

        private bool _wrongGiven = false;
        private object wrongGivenLock = new object();
        public KeyVaultMock(string wrongKey, string rightKey)
        {
            _wrongKey = wrongKey;
            _rightKey = rightKey;
        }


        public SecureString GetSecureKey(Uri keyUri)
        {
            return GetKey(keyUri).ConvertToSecureString();
        }

        public string GetKey(Uri keyUri)
        {
            if (!_wrongGiven)
            {
                _wrongGiven = true;
                return _wrongKey;
            }

            return _rightKey;
        }
    }
}
