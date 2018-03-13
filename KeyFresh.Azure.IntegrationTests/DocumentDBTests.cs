using System;
using Xunit;
using KeyFresh.Azure.DocumentDB.Providers;
using KeyFresh.Azure.DocumentDB;
using Microsoft.Azure.Documents;

namespace KeyFresh.Azure.IntegrationTests
{
    public class DocumentDBTests
    {
        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "DocumentClientData")]
        public async void DocumentRefreshClient_ExecuteAsync_SwitchToRightKey_Success(string dbUri, string wrongCs, string rightCs)
        {
            var refreshKey = GetDocumentClientRefreshKey(wrongCs, rightCs);
            var documentClientProvider = new DocumentClientProvider(new Uri(dbUri), refreshKey, 0);
            var documentRefreshClient = new DocumentRefreshClient(documentClientProvider);

            await documentRefreshClient.ExecuteAsync(x => x.OpenAsync()).ConfigureAwait(false);
        }

        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "DocumentClientData")]
        public async void DocumentRefreshClient_ExecuteAsync_WrongKey_Exception(string dbUri, string wrongCs, string rightCs)
        {
            var refreshKey = GetDocumentClientRefreshKey(wrongCs, wrongCs);
            var documentClientProvider = new DocumentClientProvider(new Uri(dbUri), refreshKey, 0);
            var documentRefreshClient = new DocumentRefreshClient(documentClientProvider);

            await Assert.ThrowsAsync<DocumentClientException>(
                () => documentRefreshClient.ExecuteAsync(x => x.OpenAsync())).ConfigureAwait(false);
        }

        private RefreshKey GetDocumentClientRefreshKey(string wrongCs, string rightCs)
        {
            return new RefreshKey(new Uri("https://abc.xyz"), new KeyVaultMock(wrongCs, rightCs));
        }
    }
}
