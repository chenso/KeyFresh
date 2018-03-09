using System;
using Xunit;
using KeyFresh;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

namespace KeyFresh.UnitTests
{
    public class IntegrationTests
    {
        [Fact(Skip = "Define own keyvault")]
        public async void RefreshClient_ExecuteAsync_Retry_Success()
        {
            var refreshKey = new RefreshKey(new Uri("https://google.com"), new KeyVaultMock());
            var blobClientProvider = new BlobClientProvider(refreshKey);
            var refreshClient = new CloudBlobRefreshClient(blobClientProvider);
            var result = await refreshClient.ExcecuteAsync(x => x.ListContainersSegmentedAsync(null));
        }
    }
}
