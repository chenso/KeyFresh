using System;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace KeyFresh.Azure.DocumentDB
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DocumentRefreshClient : AbstractRefreshClient<DocumentClient>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DocumentRefreshClient"/>
        /// This will refersh on HttpStatusCode 401(Unauthorized)
        /// </summary>
        /// <param name="clientProvider"></param>
        public DocumentRefreshClient(IClientMaintainer<DocumentClient> clientProvider) 
            : base(clientProvider, RefreshPolicy.HandleException<DocumentClientException>(
                x => x.StatusCode == HttpStatusCode.Unauthorized, clientProvider.RefreshClient))
        {

        }
    }
}
