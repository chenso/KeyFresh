using System;
using Xunit;
using KeyFresh.Azure.DocumentDB.Maintainers;
using KeyFresh.Azure.DocumentDB;
using Microsoft.Azure.Documents;

namespace KeyFresh.Azure.IntegrationTests
{
    public class DocumentDBTests : IntegrationTest
    {
        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "DocumentClientData")]
        public async void DocumentRefreshClient_ExecuteAsync_SwitchToRightKey_Success(string dbUri, string wrongCs, string rightCs)
        {
            var refreshKey = BuildRefreshKeyMock(wrongCs, rightCs);
            var documentClientProvider = new DocumentClientMaintainer(new Uri(dbUri), refreshKey, 0);
            var documentRefreshClient = new DocumentRefreshClient(documentClientProvider);

            await documentRefreshClient.ExecuteAsync(x => x.OpenAsync()).ConfigureAwait(false);
        }

        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "DocumentClientData")]
        public async void DocumentRefreshClient_ExecuteAsync_WrongKey_ExceptionThrown(string dbUri, string wrongCs, string rightCs)
        {
            var refreshKey = BuildRefreshKeyMock(wrongCs, wrongCs);
            var documentClientProvider = new DocumentClientMaintainer(new Uri(dbUri), refreshKey, 0);
            var documentRefreshClient = new DocumentRefreshClient(documentClientProvider);

            await Assert.ThrowsAsync<DocumentClientException>(
                () => documentRefreshClient.ExecuteAsync(x => x.OpenAsync())).ConfigureAwait(false);
        }

        [Theory(Skip = "Define connections.json parameters")]
        [JsonFileData("connections.json", "DocumentClientData")]
        public void DocumentRefreshClient_ExecuteSynchronous_Success(string dbUri, string wrongCs, string rightCs)
        {
            var refreshKey = BuildRefreshKeyMock(wrongCs, rightCs);
            var documentClientProvider = new DocumentClientMaintainer(new Uri(dbUri), refreshKey, 0);
            var documentRefreshClient = new DocumentRefreshClient(documentClientProvider);
            var dbAccount = documentRefreshClient.Execute(x => x.GetDatabaseAccountAsync().GetAwaiter().GetResult());
            documentRefreshClient.Execute(x => x.OpenAsync().GetAwaiter().GetResult());
        }
    }
}
