using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace KeyFresh.Azure.Storage.Clients
{
    public sealed class CloudQueueRefreshClient : AbstractRefreshClient<CloudQueueClient>
    {
        public CloudQueueRefreshClient(IClientMaintainer<CloudQueueClient> clientProvider) 
            : base(
                  clientProvider, 
                  RefreshPolicy.HandleException<StorageException>(
                        x => x.RequestInformation?.HttpStatusCode == (int) HttpStatusCode.Forbidden,
                        clientProvider.RefreshClient))
        { }
    }
}
