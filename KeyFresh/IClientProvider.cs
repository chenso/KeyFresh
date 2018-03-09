using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KeyFresh
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <typeparam name="TClient"></typeparam>
    public interface IClientProvider<TClient>
    {
        /// <summary>
        /// Gets current instance of client
        /// </summary>
        /// <returns></returns>
        TClient GetClient();

        /// <summary>
        /// Refreshes to a new instance of the client
        /// </summary>
        void RefreshClient();

        /// <summary>
        /// Refreshes to a new instance of the client
        /// </summary>
        /// <returns></returns>
        Task RefreshClientAsync();
    }
}
