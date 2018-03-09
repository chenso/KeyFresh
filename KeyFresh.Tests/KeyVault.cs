using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace KeyFresh.UnitTests
{
    public class KeyVaultMock : IKeyProvider
    {
        private int count = 0;

        public SecureString GetSecureKey(Uri keyUri)
        {
            throw new NotImplementedException();
        }

        public string GetKey(Uri keyUri)
        {
            throw new NotImplementedException();
        }
    }
}
