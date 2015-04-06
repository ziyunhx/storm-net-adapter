using System;
using System.Collections.Generic;
using System.Linq;

namespace Storm
{
    internal class BoltContext : Context
    {

        private static int batchSize = 100;
        //private string _processKey;
        private bool _enableAck;
        public override void Emit(List<object> values)
        {
            this.Emit("default", values);
        }
        public override void Emit(string streamId, List<object> values)
        {



            //ProxyMessage item = base.generateProxyMessage(streamId, string.Empty, ProxyEvent.DEFAULT, values);
            //this.msgQueue.Enqueue(item);
            //if (this.msgQueue.Count >= BoltContext.batchSize)
            //{
            //    base.FlushMsgQueue();
            //}
        }
        public override void Emit(string streamId, List<object> values, long seqId)
        {
            throw new Exception("[BoltContext] Only Non-Tx Spout can call this function!");
        }
        public override void Emit(string streamId, IEnumerable<StormTuple> anchors, List<object> values)
        {
            string tupleId = string.Empty;
            if (anchors != null && anchors.Count<StormTuple>() > 0)
            {
                string[] value = (
                    from p in anchors
                    select p.GetTupleId()).ToArray<string>();
                tupleId = string.Join(",", value);
            }
            ProxyMessage item = base.generateProxyMessage(streamId, tupleId, ProxyEvent.DEFAULT, values);
            this.msgQueue.Enqueue(item);
            if (this.msgQueue.Count >= BoltContext.batchSize)
            {
                base.FlushMsgQueue();
            }
        }
        public override void Ack(StormTuple tuple)
        {
            if (!this._enableAck)
            {
                throw new Exception("[BoltContext.Ack()] nontransactional.ack.enabled is not enabled!");
            }
            ProxyMessage item = base.generateProxyMessage("default", tuple.GetTupleId(), ProxyEvent.ACK_TUPLE, null);
            this.msgQueue.Enqueue(item);
            if (this.msgQueue.Count >= BoltContext.batchSize)
            {
                base.FlushMsgQueue();
            }
        }
        public override void Fail(StormTuple tuple)
        {
            if (!this._enableAck)
            {
                throw new Exception("[BoltContext.Fail()] nontransactional.ack.enabled is not enabled!");
            }
            ProxyMessage item = base.generateProxyMessage("default", tuple.GetTupleId(), ProxyEvent.FAIL_TUPLE, null);
            this.msgQueue.Enqueue(item);
            if (this.msgQueue.Count >= BoltContext.batchSize)
            {
                base.FlushMsgQueue();
            }
        }
        internal BoltContext(bool enableAck = true)
        {
            this._enableAck = enableAck;
        }
    }
}