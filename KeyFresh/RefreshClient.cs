using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KeyFresh
{
    public abstract class RefreshClient<TClient>
    {
        protected IClientProvider<TClient> ClientProvider;
        protected RefreshPolicy RefreshHandler;

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
