using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace KeyFresh.Azure.Storage.Clients
{
    public sealed class CloudBlobRefreshClient : RefreshClient<CloudBlobClient>
    {
        public CloudBlobRefreshClient(IClientProvider<CloudBlobClient> clientProvider) 
            : base(
                  clientProvider, 
                  RefreshPolicy.HandleException<StorageException>(
                        x => x.RequestInformation?.HttpStatusCode == (int) HttpStatusCode.Forbidden,
                        clientProvider.RefreshClient))
        { }
    }
}
