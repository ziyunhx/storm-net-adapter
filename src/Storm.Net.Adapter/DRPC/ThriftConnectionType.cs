#if !NETSTANDARD13
using System.Collections.Generic;

namespace Storm.DRPC
{
    /// <summary>
    /// the thrift connection type
    /// </summary>
    public enum ThriftConnectionType
    {
        /// <summary>
        /// NIMBUS
        /// </summary>
        NIMBUS = 1,
        /// <summary>
        /// DRPC
        /// </summary>
        DRPC = 2,
        /// <summary>
        /// DRPC_INVOCATIONS
        /// </summary>
        DRPC_INVOCATIONS = 3,
    }
}
#endif