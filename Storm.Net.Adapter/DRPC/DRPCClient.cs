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
        /// <summary>
        /// excute the method
        /// </summary>
        /// <param name="functionName">function name</param>
        /// <param name="funcArgs">function args</param>
        /// <returns></returns>
        public string execute(string functionName, string funcArgs)
        {
            transport.Open();
            string result = client.execute(functionName, funcArgs);
            transport.Close();
            return result;
        }

        private TTransport transport;
        private DistributedRPC.Client client;
        private string _host = "localhost";
        private int _port = 3772;
        private int _timeout = 0;

        /// <summary>
        /// Init method
        /// </summary>
        /// <param name="host">the host</param>
        /// <param name="port">the port</param>
        /// <param name="timeout">timeout of DRPC</param>
        public DRPCClient(string host, int port, int timeout = 0)
        {
            this._host = host;
            this._port = port;
            this._timeout = timeout;


            TSocket socket = new TSocket(_host, _port);
            if (_timeout > 0)
            {
                socket.Timeout = _timeout;
            }

            transport = new TFramedTransport(socket);

            TProtocol protocol = new TBinaryProtocol(transport);
            client = new DistributedRPC.Client(protocol);

            
        }

        /// <summary>
        /// Get the host.
        /// </summary>
        /// <returns></returns>
        public String GetHost()
        {
            return _host;
        }

        /// <summary>
        /// Get the port.
        /// </summary>
        /// <returns></returns>
        public int GetPort()
        {
            return _port;
        }

        /// <summary>
        /// Get the client.
        /// </summary>
        /// <returns></returns>
        public DistributedRPC.Client GetClient()
        {
            return client;
        }
    }
}