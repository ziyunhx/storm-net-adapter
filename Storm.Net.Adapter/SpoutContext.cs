using System;
using System.Collections.Generic;

namespace Storm
{
    internal class SpoutContext : Context
    {
        private static int batchSize = 100;
        //private string _processKey;
        private bool _enableAck;
        public override void Emit(List<object> values)
        {
            if (this._enableAck)
            {
                throw new Exception("[SpoutContext] Ack enabled, should call Emit() with seqId!");
            }
            this.Emit("default", values);
        }
        public override void Emit(string streamId, List<object> values)
        {
            ProxyMessage item = base.generateProxyMessage(streamId, string.Empty, ProxyEvent.DEFAULT, values);
            this.msgQueue.Enqueue(item);
            if (this.msgQueue.Count >= SpoutContext.batchSize)
            {
                base.FlushMsgQueue();
            }
        }
        public override void Emit(string streamId, List<object> values, long seqId)
        {
            ProxyMessage item = base.generateProxyMessage(streamId, seqId.ToString(), ProxyEvent.DEFAULT, values);
            this.msgQueue.Enqueue(item);
            if (this.msgQueue.Count >= SpoutContext.batchSize)
            {
                base.FlushMsgQueue();
            }
        }
        public override void Emit(string streamId, IEnumerable<StormTuple> anchors, List<object> tuple)
        {
            throw new Exception("[SpoutContext] Bolt can not call this function!");
        }
        public override void Ack(StormTuple tuple)
        {
            throw new Exception("[SpoutContext] Bolt can not call this function!");
        }
        public override void Fail(StormTuple tuple)
        {
            throw new Exception("[SpoutContext] Bolt can not call this function!");
        }
        internal SpoutContext(bool enableAck = true)
        {
            this._enableAck = enableAck;
        }
    }
}