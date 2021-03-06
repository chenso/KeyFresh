﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents.Client;
using CodeTiger;

namespace KeyFresh.Azure.DocumentDB.Maintainers
{
    public sealed class DocumentClientMaintainer : AbstractClientMaintainer<DocumentClient>
    {
        private readonly Uri _serviceEndpoint;

        /// <summary>
        /// Initializes new instance of <see cref="DocumentClientMaintainer"/>
        /// </summary>
        /// <param name="serviceEndpoint"></param>
        /// <param name="refreshKey"></param>
        /// <param name="refershIntervalSeconds"></param>
        public DocumentClientMaintainer(Uri serviceEndpoint, RefreshKey refreshKey, int refershIntervalSeconds = 5) 
            : base(refreshKey, refershIntervalSeconds)
        {
            Guard.ArgumentIsNotNull(nameof(serviceEndpoint), serviceEndpoint);

            _serviceEndpoint = serviceEndpoint;
        }

        protected override DocumentClient GetFreshClient()
        {
            return new DocumentClient(_serviceEndpoint, RefreshKey.RetrieveSecureKey());
        }
    }
}
