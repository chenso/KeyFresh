using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeTiger;

namespace KeyFresh
{
    /// <summary>
    /// Wraps a client with a RefreshPolicy
    /// </summary>
    /// <typeparam name="TClient">Client type to wrap</typeparam>
    public abstract class AbstractRefreshClient<TClient> where TClient : class
    {
        protected readonly IClientMaintainer<TClient> ClientProvider;
        protected readonly RefreshPolicy RefreshHandler;

        /// <summary>
        /// Creates new instance of <see cref="AbstractRefreshClient{TClient}"/>
        /// </summary>
        /// <param name="clientProvider">Provider for a refreshable instance of the client</param>
        /// <param name="refreshPolicy">Policy for actions to take on request failure</param>
        public AbstractRefreshClient(IClientMaintainer<TClient> clientProvider, RefreshPolicy refreshPolicy)
        {
            Guard.ArgumentIsNotNull(nameof(clientProvider), clientProvider);
            Guard.ArgumentIsNotNull(nameof(refreshPolicy), refreshPolicy);

            ClientProvider = clientProvider;
            RefreshHandler = refreshPolicy;
        }

        public TResponse Execute<TResponse>(Func<TClient, TResponse> lambda)
        {
            return RefreshHandler.Excecute(() => lambda(ClientProvider.GetClient()));
        }

        public void Execute(Action<TClient> lambda)
        {
            RefreshHandler.Excecute(() => lambda(ClientProvider.GetClient()));
        }

        public Task<TResponse> ExecuteAsync<TResponse>(Func<TClient, Task<TResponse>> lambda)
        {
            return RefreshHandler.ExcecuteAsync(() => lambda(ClientProvider.GetClient()));
        }

        public Task ExecuteAsync(Func<TClient, Task> lambda)
        {
            return RefreshHandler.ExcecuteAsync(() => lambda(ClientProvider.GetClient()));
        }
    }
}
