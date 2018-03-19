using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KeyFresh
{
    /// <summary>
    /// Maintains a refreshable instance of a client
    /// </summary>
    /// <remarks>Implementations must be thread-safe</remarks>
    /// <typeparam name="TClient"></typeparam>
    public interface IClientMaintainer<TClient>
        where TClient : class
    {
        /// <summary>
        /// Gets current instance of client.
        /// </summary>
        /// <returns></returns>
        TClient GetClient();

        /// <summary>
        /// Refreshes to a new instance of the client
        /// </summary>
        void RefreshClient();
    }
}
