using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace KeyFresh.Azure.Storage.Clients
{
    public sealed class CloudBlobRefreshClient : AbstractRefreshClient<CloudBlobClient>
    {
        public CloudBlobRefreshClient(IClientMaintainer<CloudBlobClient> clientProvider) 
            : base(
                  clientProvider, 
                  RefreshPolicy.HandleException<StorageException>(
                        x => x.RequestInformation?.HttpStatusCode == (int) HttpStatusCode.Forbidden,
                        clientProvider.RefreshClient))
        { }
    }
}
