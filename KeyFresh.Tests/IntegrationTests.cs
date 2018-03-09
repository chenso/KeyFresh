using System;
using Xunit;
using KeyFresh;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KeyFresh.UnitTests
{
    public class IntegrationTests
    {
        [Fact(Skip = "Implement KeyVaultMock with own credentials")]
        public async void RefreshClient_ExecuteAsync_Retry_Success()
        {
            var refreshKey = new RefreshKey(new Uri("https://google.com"), new KeyVaultMock());
            var blobClientProvider = new BlobClientProvider(refreshKey);
            var refreshClient = new CloudBlobRefreshClient(blobClientProvider);
            var tasks = new List<Task<ContainerResultSegment>>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(refreshClient.ExcecuteAsync(x => x.ListContainersSegmentedAsync(null)));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}
