using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeTiger;

namespace KeyFresh
{
    /// <summary>
    /// Thread-safe client maintianer
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    public abstract class AbstractClientMaintainer<TClient> : IClientMaintainer<TClient>
        where TClient : class
    {
        protected readonly RefreshKey RefreshKey;
        private volatile TClient _client;
        private object _refreshLock = new object();
        
        private readonly int _refreshIntervalSeconds;
        private DateTimeOffset _disableRefreshUntil = DateTimeOffset.MinValue;

        /// <summary>
        /// Initializes new instance of <see cref="AbstractClientMaintainer{TClient}"/>
        /// </summary>
        /// <param name="refreshKey"></param>
        /// <param name="refreshIntervalSeconds"></param>
        protected AbstractClientMaintainer(RefreshKey refreshKey, int refreshIntervalSeconds = 1)
        {
            Guard.ArgumentIsNotNull(nameof(refreshKey), refreshKey);

            RefreshKey = refreshKey;
            _refreshIntervalSeconds = refreshIntervalSeconds;
        }

        /// <summary>
        /// Gets current instance of the client
        /// </summary>
        /// <returns></returns>
        public virtual TClient GetClient()
        {
            if (_client == null)
            {
                RefreshClient();
            }

            return _client;
        }

        /// <summary>
        /// Replaces client with a new instance
        /// </summary>
        public virtual void RefreshClient()
        {
            if (Monitor.TryEnter(_refreshLock))
            {
                try
                {
                    if (DateTimeOffset.UtcNow > _disableRefreshUntil)
                    {
                        _client = GetFreshClient();
                        _disableRefreshUntil = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(_refreshIntervalSeconds);
                    }
                }
                finally
                {
                    Monitor.Exit(_refreshLock);
                }
            }
            else
            {
                // Wait until lock is released - client has just refreshed, so no-op
                WaitRefreshLock();
            }
        }

        /// <summary>
        /// Gets a new instance of the client
        /// </summary>
        /// <returns></returns>
        protected abstract TClient GetFreshClient();

        private void WaitRefreshLock()
        {
            Monitor.Enter(_refreshLock);
            Monitor.Exit(_refreshLock);
        }
    }
}
