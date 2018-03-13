using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace KeyFresh.Azure.Storage.Providers
{
    public sealed class QueueClientProvider : AbstractClientProvider<CloudQueueClient>
    {
        public QueueClientProvider(RefreshKey refreshKey, int refreshIntervalInSeconds = 5) 
            : base(refreshKey, refreshIntervalInSeconds) { }

        protected override CloudQueueClient GetFreshClient()
        {
            var cs = CloudStorageAccount.Parse(RefreshKey.GetKey());
            return cs.CreateCloudQueueClient();
        }
    }
}
