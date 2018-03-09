using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KeyFresh
{
    /// <summary>
    /// Wraps a client with a RefreshPolicy
    /// </summary>
    /// <typeparam name="TClient">Client type to wrap</typeparam>
    public abstract class RefreshClient<TClient>
    {
        protected IClientProvider<TClient> ClientProvider;
        protected RefreshPolicy RefreshHandler;

        /// <summary>
        /// Creates a refreshable client wrapper
        /// </summary>
        /// <param name="clientProvider">Provider for a refreshable instance of the client</param>
        /// <param name="refreshPolicy">Policy for actions to take on request failure</param>
        public RefreshClient(IClientProvider<TClient> clientProvider, RefreshPolicy refreshPolicy)
        {
            Ensure.ArgumentNotNull(clientProvider, nameof(clientProvider));
            Ensure.ArgumentNotNull(refreshPolicy, nameof(refreshPolicy));

            ClientProvider = clientProvider;
            RefreshHandler = refreshPolicy;
        }

        public TResponse Excecute<TResponse>(Func<TClient, TResponse> lambda)
        {
            return RefreshHandler.Excecute(() => lambda(ClientProvider.GetClient()));
        }

        public Task<TResponse> ExcecuteAsync<TResponse>(Func<TClient, Task<TResponse>> lambda)
        {
            return RefreshHandler.ExcecuteAsync(() => lambda(ClientProvider.GetClient()));
        }
    }
}
