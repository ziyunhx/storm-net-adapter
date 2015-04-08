using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Storm
{
    internal class BoltContext : Context
    {
        private bool _enableAck;
        public override void Emit(List<object> values)
        {
            this.Emit("default", values);
        }

        public override void Emit(string streamId, List<object> values, long? seqId = null)
        {
            if (seqId != null)
                throw new Exception("[BoltContext] Only Non-Tx Spout can call this function!");
            else
                this.Emit(streamId, null, values);
        }
        public override void Emit(string streamId, IEnumerable<StormTuple> anchors, List<object> values)
        {
            List<string> tupleIds = new List<string>();
            if (anchors != null && anchors.Count<StormTuple>() > 0)
            {
                tupleIds = (
                    from p in anchors
                    select p.GetTupleId()).ToList<string>();
            }

            base.CheckOutputSchema(streamId, values == null ? 0 : values.Count);
            string msg = @"""command"": ""emit"", ""anchors"": ""{0}"", ""stream"": ""{1}"", ""tuple"": [{2}]";
            Storm.SendMsgToParent("{" + string.Format(msg, JsonConvert.SerializeObject(tupleIds), streamId, JsonConvert.SerializeObject(values)) + "}");

            //As of version 0.7.1, there is no longer any need for a shell bolt to 'sync'.
            //Storm.Sync();
        }
        public override void Ack(StormTuple tuple)
        {
            if (!this._enableAck)
            {
                throw new Exception("[BoltContext.Ack()] nontransactional.ack.enabled is not enabled!");
            }
            Storm.Ack(tuple);

            //As of version 0.7.1, there is no longer any need for a shell bolt to 'sync'.
            //Storm.Sync();
        }
        public override void Fail(StormTuple tuple)
        {
            if (!this._enableAck)
            {
                throw new Exception("[BoltContext.Fail()] nontransactional.ack.enabled is not enabled!");
            }
            Storm.Fail(tuple);

            //As of version 0.7.1, there is no longer any need for a shell bolt to 'sync'.
            //Storm.Sync();
        }
        internal BoltContext(bool enableAck = true)
        {
            this._enableAck = enableAck;
        }
    }
}