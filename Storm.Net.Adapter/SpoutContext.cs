using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Storm
{
    internal class SpoutContext : Context
    {
        private bool _enableAck;
        public override void Emit(List<object> values)
        {
            if (this._enableAck)
            {
                Context.Logger.Error("[SpoutContext] Ack enabled, should call Emit() with seqId!");
            }
            this.Emit("default", values);
        }

        public override void Emit(string streamId, List<object> values, long? seqId = null)
        {
            base.CheckOutputSchema(streamId, values == null ? 0 : values.Count);
            string msg = @"""command"": ""emit"", ""id"": ""{0}"", ""stream"": ""{1}"", ""tuple"": {2}";
            ApacheStorm.SendMsgToParent("{" + string.Format(msg, seqId == null ? "" : seqId.ToString(), streamId, JsonConvert.SerializeObject(values)) + "}");
            //ApacheStorm.Sync();
            //ApacheStorm.ReadTaskId();
        }
        public override void Emit(string streamId, IEnumerable<StormTuple> anchors, List<object> tuple)
        {
            Context.Logger.Error("[SpoutContext] Bolt can not call this function!");
        }
        public override void Ack(StormTuple tuple)
        {
            Context.Logger.Error("[SpoutContext] Bolt can not call this function!");
        }
        public override void Fail(StormTuple tuple)
        {
            Context.Logger.Error("[SpoutContext] Bolt can not call this function!");
        }
        internal SpoutContext(bool enableAck = true)
        {
            this._enableAck = enableAck;
        }
    }
}