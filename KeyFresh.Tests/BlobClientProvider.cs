using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using KeyFresh;
using Microsoft.WindowsAzure.Storage;

namespace KeyFresh.UnitTests
{
    public class BlobClientProvider : IClientProvider<CloudBlobClient>
    {
        private volatile CloudBlobClient _client;
        private object clientLock = new object();

        private RefreshKey _key;

        public BlobClientProvider(RefreshKey key)
        {
            _key = key;
        }

        public CloudBlobClient GetClient()
        {
            if (_client == null) {
                lock(clientLock)
                {
                    if (_client == null)
                    {
                        RefreshClient();
                    }
                }
            };

            return _client;
        }

        public void RefreshClient()
        {
            var account = CloudStorageAccount.Parse(_key.GetKey());
            _client = account.CreateCloudBlobClient();
        }

        public Task RefreshClientAsync()
        {
            RefreshClient();
            return Task.FromResult(0);
        }
    }
}
