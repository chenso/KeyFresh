using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace KeyFresh.Azure.Storage.Providers
{
    public sealed class TableClientProvider : AbstractClientProvider<CloudTableClient>
    {
        public TableClientProvider(RefreshKey refreshKey, int refreshIntervalInSeconds) 
            : base(refreshKey, refreshIntervalInSeconds) { }

        protected override CloudTableClient GetFreshClient()
        {
            var cs = CloudStorageAccount.Parse(RefreshKey.RetrieveKey());
            return cs.CreateCloudTableClient();
        }
    }
}
