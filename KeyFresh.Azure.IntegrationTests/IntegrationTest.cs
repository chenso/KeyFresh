using System;
using System.Collections.Generic;
using System.Text;
using KeyFresh.Azure.IntegrationTests.Extensions;
using Moq;

namespace KeyFresh.Azure.IntegrationTests
{
    public abstract class IntegrationTest
    {
        private bool wrongGiven = false;

        protected RefreshKey BuildRefreshKeyMock(string wrongCs, string rightCs)
        {
            var mockKeyVault = new Mock<IKeyProvider>();
            mockKeyVault.Setup(x => x.GetKey(It.IsAny<Uri>()))
                .Callback(() => mockKeyVault.Setup(x => x.GetKey(It.IsAny<Uri>()))
                    .Returns(rightCs))
                .Returns(wrongCs);
            mockKeyVault.Setup(x => x.GetSecureKey(It.IsAny<Uri>()))
                .Callback(() => mockKeyVault.Setup(x => x.GetSecureKey(It.IsAny<Uri>()))
                    .Returns(rightCs.ConvertToSecureString()))
                .Returns(wrongCs.ConvertToSecureString());
            return new RefreshKey(new Uri("https://abc.xyz"), mockKeyVault.Object);
        }
    }
}
