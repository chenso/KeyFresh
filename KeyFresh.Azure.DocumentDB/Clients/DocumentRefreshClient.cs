using System;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace KeyFresh.Azure.DocumentDB
{
    public sealed class DocumentRefreshClient : RefreshClient<DocumentClient>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DocumentRefreshClient"/>
        /// This will refersh on HttpStatusCode 401(Unauthorized)
        /// </summary>
        /// <param name="clientProvider"></param>
        public DocumentRefreshClient(IClientProvider<DocumentClient> clientProvider) 
            : base(clientProvider, RefreshPolicy.HandleException<DocumentClientException>(
                x => x.StatusCode == HttpStatusCode.Unauthorized, clientProvider.RefreshClient))
        {

        }
    }
}
