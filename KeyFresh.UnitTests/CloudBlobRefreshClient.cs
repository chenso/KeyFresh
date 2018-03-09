using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace KeyFresh.UnitTests
{
    public class CloudBlobRefreshClient : RefreshClient<CloudBlobClient>
    {
        public CloudBlobRefreshClient(IClientProvider<CloudBlobClient> clientProvider) 
            : base(clientProvider, RefreshPolicy.AsyncHandleException<StorageException>(
                x => x.RequestInformation?.HttpStatusCode == (int) HttpStatusCode.Forbidden,
                clientProvider.RefreshClientAsync))
        { }
    }
}
