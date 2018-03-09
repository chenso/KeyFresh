using System;
using Xunit;
using KeyFresh;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

namespace KeyFresh.UnitTests
{
    public class Tests
    {
        [Fact]
        public async void Test1()
        {
            var refreshKey = new RefreshKey(new Uri("https://google.com"), new KeyVaultMock());
            var blobClientProvider = new BlobClientProvider(refreshKey);
            var refreshClient = new CloudBlobRefreshClient(blobClientProvider);
            var result = await refreshClient.ExcecuteAsync(x => x.ListContainersSegmentedAsync(null));
        }
    }
}
