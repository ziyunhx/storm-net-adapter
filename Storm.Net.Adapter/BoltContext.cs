using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Storm
{
    internal class BoltContext : Context
    {
        public override void Emit(List<object> values, string taskId = null)
        {
            this.Emit("default", null, values, taskId);
        }

        public override void Emit(string streamId, List<object> values, string taskId = null)
        {
            this.Emit(streamId, null, values, taskId);
        }

        public override void Emit(string streamId, List<object> values, long seqId, string taskId = null)
        {
            Context.Logger.Error("[BoltContext] Only Non-Tx Spout can call this function!");
        }

        public override void Emit(string streamId, IEnumerable<StormTuple> anchors, List<object> values, string taskId = null)
        {
            List<string> tupleIds = new List<string>();

            if (anchors != null && anchors.Count<StormTuple>() > 0)
            {
                tupleIds = (
                    from p in anchors
                    select p.GetTupleId()).ToList<string>();
            }

            base.CheckOutputSchema(streamId, values == null ? 0 : values.Count);

            if (string.IsNullOrWhiteSpace(taskId))
            {
                string msg = @"""command"": ""emit"", ""anchors"": {0}, ""stream"": ""{1}"", ""tuple"": {2}";
                ApacheStorm.SendMsgToParent("{" + string.Format(msg, JsonConvert.SerializeObject(tupleIds), streamId, JsonConvert.SerializeObject(values)) + "}");
                ApacheStorm.ReadTaskId();
            }
            else
            {
                string msg = @"""command"": ""emit"", ""anchors"": {0}, ""stream"": ""{1}"", ""task"": {2}, ""tuple"": {3}";
                ApacheStorm.SendMsgToParent("{" + string.Format(msg, JsonConvert.SerializeObject(tupleIds), streamId, taskId, JsonConvert.SerializeObject(values)) + "}");
            }
        }

        public override void Ack(StormTuple tuple)
        {
            ApacheStorm.Ack(tuple);
        }

        public override void Fail(StormTuple tuple)
        {
            ApacheStorm.Fail(tuple);
        }

        internal BoltContext()
        {
        }
    }
}