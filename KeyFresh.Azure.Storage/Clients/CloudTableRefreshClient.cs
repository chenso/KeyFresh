using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace KeyFresh.Azure.Storage.Clients
{
    public sealed class CloudTableRefreshClient : AbstractRefreshClient<CloudTableClient>
    {
        public CloudTableRefreshClient(IClientMaintainer<CloudTableClient> clientProvider) 
            : base(
                  clientProvider, 
                  RefreshPolicy.HandleException<StorageException>(
                        x => x.RequestInformation?.HttpStatusCode == (int) HttpStatusCode.Forbidden,
                        clientProvider.RefreshClient))
        { }
    }
}
