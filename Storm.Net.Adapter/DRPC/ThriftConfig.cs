using System.Text;

namespace Storm.DRPC
{
    public class ThriftConfig
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 服务端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 传输编码
        /// </summary>
        public Encoding Encode { get; set; }
        /// <summary>
        /// 是否启用压缩
        /// </summary>
        public bool Zipped { get; set; }
        /// <summary>
        /// 连接超时
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// 可以从缓存池中分配对象的最大数量
        /// </summary>
        public int MaxActive { get; set; }
        /// <summary>
        /// 缓存池中最大空闲对象数量
        /// </summary>
        public int MaxIdle { get; set; }
        /// <summary>
        /// 缓存池中最小空闲对象数量
        /// </summary>
        public int MinIdle { get; set; }
        /// <summary>
        /// 阻塞的最大数量
        /// </summary>
        public int MaxWait { get; set; }
        /// <summary>
        /// 是否重新连接
        /// </summary>
        public bool ReConnect { get; set; }
        /// <summary>
        /// 从缓存池中分配对象时是否验证对象
        /// </summary>
        public bool ValidateOnBorrow { get; set; }
        /// <summary>
        /// 从缓存池中归还对象时是否验证对象
        /// </summary>
        public bool ValidateOnReturn { get; set; }
        /// <summary>
        /// 从缓存池中挂起对象时是否验证对象
        /// </summary>
        public bool ValidateWhiledIdle { get; set; }
    }
}
