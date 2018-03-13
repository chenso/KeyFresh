using System;
using Xunit;
using KeyFresh;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.Collections.Generic;
using KeyFresh.Azure.Storage.Providers;
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
            var blobClientProvider = new BlobClientProvider(refreshKey, 0);
            var refreshClient = new CloudBlobRefreshClient(blobClientProvider);
            var tasks = new List<Task<ContainerResultSegment>>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(refreshClient.ExcecuteAsync(x => x.ListContainersSegmentedAsync(null)));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "StorageData")]
        public async void CloudBlobRefreshClient_ExecuteAsync_WrongKey_ThrowsStorageException(string wrongCs, string rightCs)
        {
            var refreshKey = BuildStorageRefreshKey(wrongCs, wrongCs);
            var blobClientProvider = new BlobClientProvider(refreshKey, 0);
            var refreshClient = new CloudBlobRefreshClient(blobClientProvider);
            await Assert.ThrowsAsync<StorageException>(() => refreshClient.ExcecuteAsync(x => x.ListContainersSegmentedAsync(null)));
        }

        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "StorageData")]
        public async void CloudTableRefreshClient_ExecuteAsync_Parallel_Success(string wrongCs, string rightCs)
        {
            var refreshKey = BuildStorageRefreshKey(wrongCs, rightCs);
            var blobClientProvider = new TableClientProvider(refreshKey, 0);
            var refreshClient = new CloudTableRefreshClient(blobClientProvider);
            var tasks = new List<Task<TableResultSegment>>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(refreshClient.ExcecuteAsync(x => x.ListTablesSegmentedAsync(null)));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "StorageData")]
        public async void CloudTableRefreshClient_ExecuteAsync_WrongKey_ThrowsStorageException(string wrongCs, string rightCs)
        {
            var refreshKey = BuildStorageRefreshKey(wrongCs, wrongCs);
            var blobClientProvider = new TableClientProvider(refreshKey, 0);
            var refreshClient = new CloudTableRefreshClient(blobClientProvider);
            await Assert.ThrowsAsync<StorageException>(() => refreshClient.ExcecuteAsync(x => x.ListTablesSegmentedAsync(null)));
        }

        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "StorageData")]
        public async void CloudQueueRefreshClient_ExecuteAsync_Parallel_Success(string wrongCs, string rightCs)
        {
            var refreshKey = BuildStorageRefreshKey(wrongCs, rightCs);
            var blobClientProvider = new QueueClientProvider(refreshKey, 0);
            var refreshClient = new CloudQueueRefreshClient(blobClientProvider);
            var tasks = new List<Task<QueueResultSegment>>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(refreshClient.ExcecuteAsync(x => x.ListQueuesSegmentedAsync(null)));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "StorageData")]
        public async void CloudQueueRefreshClient_ExecuteAsync_WrongKey_ThrowsStorageException(string wrongCs, string rightCs)
        {
            var refreshKey = BuildStorageRefreshKey(wrongCs, wrongCs);
            var blobClientProvider = new QueueClientProvider(refreshKey, 0);
            var refreshClient = new CloudQueueRefreshClient(blobClientProvider);
            await Assert.ThrowsAsync<StorageException>(() => refreshClient.ExcecuteAsync(x => x.ListQueuesSegmentedAsync(null)));
        }

        private RefreshKey BuildStorageRefreshKey(string wrongCs, string rightCs)
        {
            return new RefreshKey(new Uri("https://abc.xyz"), new KeyVaultMock(wrongCs, rightCs));
        }
    }
}
