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
    public abstract class RefreshClient<TClient> where TClient : class
    {
        protected readonly IClientProvider<TClient> ClientProvider;
        protected readonly RefreshPolicy RefreshHandler;

        protected volatile TClient Client;

        /// <summary>
        /// Creates new instance of <see cref="RefreshClient{TClient}"/>
        /// </summary>
        /// <param name="clientProvider">Provider for a refreshable instance of the client</param>
        /// <param name="refreshPolicy">Policy for actions to take on request failure</param>
        public RefreshClient(IClientProvider<TClient> clientProvider, RefreshPolicy refreshPolicy)
        {
            Guard.ArgumentIsNotNull(nameof(clientProvider), clientProvider);
            Guard.ArgumentIsNotNull(nameof(refreshPolicy), refreshPolicy);

            ClientProvider = clientProvider;
            RefreshHandler = refreshPolicy;
            Client = clientProvider.GetClient();
        }

        public TResponse Excecute<TResponse>(Func<TClient, TResponse> lambda)
        {
            return RefreshHandler.Excecute(() => lambda(ClientProvider.GetClient()));
        }

        public void Execute(Action<TClient> lambda)
        {
            RefreshHandler.Excecute(() => lambda(ClientProvider.GetClient()));
        }

        public Task<TResponse> ExcecuteAsync<TResponse>(Func<TClient, Task<TResponse>> lambda)
        {
            return RefreshHandler.ExcecuteAsync(() => lambda(ClientProvider.GetClient()));
        }

        public Task ExecuteAsync(Func<TClient, Task> lambda)
        {
            return RefreshHandler.ExcecuteAsync(() => lambda(ClientProvider.GetClient()));
        }
    }
}
