using System.Text;

namespace Storm.DRPC
{
    /// <summary>
    /// the thrift config class.
    /// </summary>
    public class ThriftConfig
    {
        /// <summary>
        /// the server host.
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// the server port.
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// the timeout.
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// the max active number of pool.
        /// </summary>
        public int MaxActive { get; set; }
        /// <summary>
        /// the max idle number of pool.
        /// </summary>
        public int MaxIdle { get; set; }
        /// <summary>
        /// the min idle number of pool.
        /// </summary>
        public int MinIdle { get; set; }
        /// <summary>
        /// reconnect the thrift connect.
        /// </summary>
        public bool ReConnect { get; set; }
        /// <summary>
        /// validate on borrow.
        /// </summary>
        public bool ValidateOnBorrow { get; set; }
        /// <summary>
        /// validate on return.
        /// </summary>
        public bool ValidateOnReturn { get; set; }
        /// <summary>
        /// validate whiled idle.
        /// </summary>
        public bool ValidateWhiledIdle { get; set; }
    }
}
