using System;
using Xunit;
using KeyFresh;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.Collections.Generic;
using KeyFresh.Azure.Storage.Maintainers;
using KeyFresh.Azure.Storage.Clients;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;

namespace KeyFresh.Azure.IntegrationTests
{
    public class StorageTests
    {
        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "StorageData")]
        public async void CloudBlobRefreshClient_ExecuteAsync_Parallel_Success(string wrongCs, string rightCs)
        {
            var refreshKey = BuildStorageRefreshKey(wrongCs, rightCs);
            var blobClientMaintainer = new BlobClientMaintainer(refreshKey, 0);
            var refreshClient = new CloudBlobRefreshClient(blobClientMaintainer);
            var tasks = new List<Task<ContainerResultSegment>>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(refreshClient.ExecuteAsync(x => x.ListContainersSegmentedAsync(null)));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "StorageData")]
        public async void CloudBlobRefreshClient_ExecuteAsync_WrongKey_ThrowsStorageException(string wrongCs, string rightCs)
        {
            var refreshKey = BuildStorageRefreshKey(wrongCs, wrongCs);
            var blobClientMaintainer = new BlobClientMaintainer(refreshKey, 0);
            var refreshClient = new CloudBlobRefreshClient(blobClientMaintainer);
            await Assert.ThrowsAsync<StorageException>(() => refreshClient.ExecuteAsync(x => x.ListContainersSegmentedAsync(null)));
        }

        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "StorageData")]
        public async void CloudTableRefreshClient_ExecuteAsync_Parallel_Success(string wrongCs, string rightCs)
        {
            var refreshKey = BuildStorageRefreshKey(wrongCs, rightCs);
            var blobClientMaintainer = new TableClientMaintainer(refreshKey, 0);
            var refreshClient = new CloudTableRefreshClient(blobClientMaintainer);
            var tasks = new List<Task<TableResultSegment>>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(refreshClient.ExecuteAsync(x => x.ListTablesSegmentedAsync(null)));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "StorageData")]
        public async void CloudTableRefreshClient_ExecuteAsync_WrongKey_ThrowsStorageException(string wrongCs, string rightCs)
        {
            var refreshKey = BuildStorageRefreshKey(wrongCs, wrongCs);
            var blobClientMaintainer = new TableClientMaintainer(refreshKey, 0);
            var refreshClient = new CloudTableRefreshClient(blobClientMaintainer);
            await Assert.ThrowsAsync<StorageException>(() => refreshClient.ExecuteAsync(x => x.ListTablesSegmentedAsync(null)));
        }

        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "StorageData")]
        public async void CloudQueueRefreshClient_ExecuteAsync_Parallel_Success(string wrongCs, string rightCs)
        {
            var refreshKey = BuildStorageRefreshKey(wrongCs, rightCs);
            var blobClientMaintainer = new QueueClientMaintainer(refreshKey, 0);
            var refreshClient = new CloudQueueRefreshClient(blobClientMaintainer);
            var tasks = new List<Task<QueueResultSegment>>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(refreshClient.ExecuteAsync(x => x.ListQueuesSegmentedAsync(null)));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "StorageData")]
        public async void CloudQueueRefreshClient_ExecuteAsync_WrongKey_ThrowsStorageException(string wrongCs, string rightCs)
        {
            var refreshKey = BuildStorageRefreshKey(wrongCs, wrongCs);
            var queueClientMaintainer = new QueueClientMaintainer(refreshKey, 0);
            var refreshClient = new CloudQueueRefreshClient(queueClientMaintainer);
            await Assert.ThrowsAsync<StorageException>(() => refreshClient.ExecuteAsync(x => x.ListQueuesSegmentedAsync(null)));
        }

        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "StorageData")]
        public void CloudBlobRefreshClient_ExecuteSynchronous_Success(string wrongCs, string rightCs)
        {
            var refreshKey = BuildStorageRefreshKey(wrongCs, wrongCs);
            var blobClientMaintainer = new BlobClientMaintainer(refreshKey, 0);
            var refreshClient = new CloudBlobRefreshClient(blobClientMaintainer);
            refreshClient.Execute(x => x.SetServicePropertiesAsync(null).GetAwaiter().GetResult());
            var result = refreshClient.Execute(x => x.ListContainersSegmentedAsync(null).GetAwaiter().GetResult());
        }

        private RefreshKey BuildStorageRefreshKey(string wrongCs, string rightCs)
        {
            return new RefreshKey(new Uri("https://abc.xyz"), new KeyVaultMock(wrongCs, rightCs));
        }
    }
}
