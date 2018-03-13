using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace KeyFresh.Azure.Storage.Clients
{
    public sealed class CloudTableRefreshClient : RefreshClient<CloudTableClient>
    {
        public CloudTableRefreshClient(IClientProvider<CloudTableClient> clientProvider) 
            : base(
                  clientProvider, 
                  RefreshPolicy.HandleException<StorageException>(
                        x => x.RequestInformation?.HttpStatusCode == (int) HttpStatusCode.Forbidden,
                        clientProvider.RefreshClient))
        { }
    }
}
