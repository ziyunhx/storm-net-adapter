using System;
using Thrift.Protocol;
using Thrift.Transport;

namespace Storm.DRPC
{
    /// <summary>
    /// The DRPC Client
    /// </summary>
    public class DRPCClient : DistributedRPC.Iface
    {
        private ThriftConfig thriftConfig = new ThriftConfig();
        private ThriftPool thriftPool = null;
        private bool _reconnect = false;

        /// <summary>
        /// excute the method
        /// </summary>
        /// <param name="functionName">function name</param>
        /// <param name="funcArgs">function args</param>
        /// <returns></returns>
        public string execute(string functionName, string funcArgs)
        {
            TTransport transport = thriftPool.BorrowInstance();
            TProtocol protocol = new TBinaryProtocol(transport);
            DistributedRPC.Client client = new DistributedRPC.Client(protocol);

            string result = client.execute(functionName, funcArgs);

            if (_reconnect)
                transport.Close();
            else
                thriftPool.ReturnInstance(transport);

            return result;
        }

        /// <summary>
        /// Init method
        /// </summary>
        /// <param name="host">the host</param>
        /// <param name="port">the port</param>
        /// <param name="timeout">timeout of DRPC</param>
        public DRPCClient(string host, int port, int timeout = 0, bool reconnect = false, int maxIdle = 100)
        {            
            thriftConfig.Host = host;
            thriftConfig.Port = port;
            thriftConfig.Timeout = timeout;
            thriftConfig.MaxIdle = maxIdle;
            thriftConfig.ReConnect = reconnect;
            thriftConfig.MaxActive = maxIdle;
            thriftConfig.MinIdle = 0;
            thriftConfig.ValidateOnBorrow = false;
            thriftConfig.ValidateOnReturn = false;
            thriftConfig.ValidateWhiledIdle = false;

            thriftPool = new ThriftPool(thriftConfig);
        }

        /// <summary>
        /// Get the host.
        /// </summary>
        /// <returns></returns>
        public String GetHost()
        {
            return thriftConfig.Host;
        }

        /// <summary>
        /// Get the port.
        /// </summary>
        /// <returns></returns>
        public int GetPort()
        {
            return thriftConfig.Port;
        }

        /// <summary>
        /// Get the client.
        /// </summary>
        /// <returns></returns>
        public DistributedRPC.Client GetClient()
        {
            TTransport transport = thriftPool.BorrowInstance();
            TProtocol protocol = new TBinaryProtocol(transport);
            return new DistributedRPC.Client(protocol);
        }
    }
}