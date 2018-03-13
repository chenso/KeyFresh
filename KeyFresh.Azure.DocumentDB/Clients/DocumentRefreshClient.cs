using System;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace KeyFresh.Azure.DocumentDB
{
    public class DocumentRefreshClient : RefreshClient<DocumentClient>
    {
        public DocumentRefreshClient(IClientProvider<DocumentClient> clientProvider) 
            : base(clientProvider, RefreshPolicy.HandleException<DocumentClientException>(
                x => x.StatusCode == HttpStatusCode.Forbidden, clientProvider.RefreshClient))
        {

        }
    }
}
