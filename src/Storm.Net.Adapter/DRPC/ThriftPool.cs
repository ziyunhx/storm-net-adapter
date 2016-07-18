using System;
using System.Collections.Generic;
using System.Threading;
using Thrift.Transport;

namespace Storm.DRPC
{
    public class ThriftPool
    {
        /// <summary>
        /// the thrift config
        /// </summary>
        private ThriftConfig config;
        /// <summary>
        /// the thrift connection pool.
        /// </summary>
        private static Stack<TTransport> objectPool { get; set; }
        /// <summary>
        /// auto reset event.
        /// </summary>
        private static AutoResetEvent resetEvent;
        /// <summary>
        /// actived object count.
        /// </summary>
        private static volatile int activedCount = 0;
        /// <summary>
        /// locker on borrow.
        /// </summary>
        private static object locker = new object();


        /// <summary>
        /// init method.
        /// </summary>
        /// <param name="thriftConfig"></param>
        public ThriftPool(ThriftConfig thriftConfig)
        {
            config = thriftConfig;
            CreateResetEvent();
            CreateThriftPool();
        }

        /// <summary>
        /// borrow an instance on pool.
        /// </summary>
        /// <returns></returns>
        public TTransport BorrowInstance()
        {
            lock (locker)
            {
                
                if (objectPool.Count == 0)
                {
                    if (activedCount == config.MaxActive)
                        resetEvent.WaitOne();
                    else
                        PushObject(CreateInstance());
                }

                TTransport transport = objectPool.Pop();
                if (transport == null)
                    throw new Exception("Thrift Pool Error.");
                activedCount++;

                if (objectPool.Count < config.MinIdle && activedCount < config.MaxActive)
                    PushObject(CreateInstance());
                if (config.ValidateOnBorrow)
                    ValidateOnBorrow(transport);
                return transport;
            }
        }

        /// <summary>
        /// return an instance.
        /// </summary>
        /// <param name="instance"></param>
        public void ReturnInstance(TTransport instance)
        {
            if (objectPool.Count >= config.MaxIdle)
                DestoryInstance(instance);
            else
            {
                if (config.ValidateOnReturn)
                    ValidateOnReturn(instance);

                PushObject(instance);
                activedCount--;
                resetEvent.Set();
            }
        }

        /// <summary>
        /// create an auto reset event.
        /// </summary>
        private void CreateResetEvent()
        {
            if (resetEvent == null)
            {
                resetEvent = new AutoResetEvent(false);
            }
        }

        /// <summary>
        /// create the thrift pool.
        /// </summary>
        private void CreateThriftPool()
        {
            if (objectPool == null)
            {
                objectPool = new Stack<TTransport>();
            }
        }

        /// <summary>
        /// push an instance to object pool.
        /// </summary>
        /// <param name="transport"></param>
        private void PushObject(TTransport transport)
        {
            objectPool.Push(transport);
        }

        /// <summary>
        /// create an instance
        /// </summary>
        /// <returns></returns>
        private TTransport CreateInstance()
        {
            TSocket socket = new TSocket(config.Host, config.Port);
            if (config.Timeout > 0)
                socket.Timeout = config.Timeout;

            TTransport transport = new TFramedTransport(socket);
            transport.Open();
            return transport;
        }

        /// <summary>
        /// validate the status on borrow.
        /// </summary>
        private void ValidateOnBorrow(TTransport instance)
        {
            if (!instance.IsOpen)
            {
                instance.Open();
            }
        }

        /// <summary>
        /// validate the status on return.
        /// </summary>
        private void ValidateOnReturn(TTransport instance)
        {
            if (instance.IsOpen)
            {
                instance.Close();
            }
        }

        /// <summary>
        /// destory the instance.
        /// </summary>
        /// <param name="instance"></param>
        private void DestoryInstance(TTransport instance)
        {
            instance.Flush();
            if (instance.IsOpen)
            {
                instance.Close();
            }
            instance.Dispose();
        }
    }
}