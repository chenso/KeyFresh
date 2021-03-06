﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace KeyFresh.Azure.Storage.Maintainers
{
    public sealed class BlobClientMaintainer : AbstractClientMaintainer<CloudBlobClient>
    {
        public BlobClientMaintainer(RefreshKey refreshKey, int refreshIntervalInSeconds) 
            : base(refreshKey, refreshIntervalInSeconds) { }

        protected override CloudBlobClient GetFreshClient()
        {
            var cs = CloudStorageAccount.Parse(RefreshKey.RetrieveKey());
            return cs.CreateCloudBlobClient();
        }
    }
}
