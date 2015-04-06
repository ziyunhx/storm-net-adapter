using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Storm
{
    public abstract class Context
    {
        internal delegate void ProxyMessageSendMethod(List<ProxyMessage> batch);

        public static PluginType pluginType;
        internal Stopwatch batchProcessWatch = new Stopwatch();
        internal static ComponentStreamSchema _schemaFromJava = null;
        internal ComponentStreamSchema _schemaByCSharp;
        internal ConcurrentQueue<ProxyMessage> msgQueue = new ConcurrentQueue<ProxyMessage>();
        public static Logger Logger = new Logger();

        public static Config Config
        {
            get;
            set;
        }

        public abstract void Emit(List<object> values);
        public abstract void Emit(string streamId, List<object> values);
        public abstract void Emit(string streamId, List<object> values, long seqId);
        public abstract void Emit(string streamId, IEnumerable<StormTuple> anchors, List<object> values);
        public abstract void Ack(StormTuple tuple);
        public abstract void Fail(StormTuple tuple);
        public void DeclareComponentSchema(ComponentStreamSchema schema)
        {
            if (Context._schemaFromJava != null && schema.OutputStreamSchema != null)
            {
                foreach (string current in schema.OutputStreamSchema.Keys)
                {
                    if (!Context._schemaFromJava.OutputStreamSchema.ContainsKey(current))
                    {
                        throw new Exception(string.Format("ouputSchema error, stream {0} missing with spec file", current));
                    }
                    if (schema.OutputStreamSchema[current].Count != Context._schemaFromJava.OutputStreamSchema[current].Count)
                    {
                        throw new Exception(string.Format("ouputSchema error, field count mismatch with spec file for streamId: {0}", current));
                    }
                }
            }
            this._schemaByCSharp = schema;
        }

        internal ProxyMessage generateProxyMessage(string streamId, string tupleId, ProxyEvent evt, List<object> data)
        {
            List<byte[]> data2 = null;
            if (evt == ProxyEvent.DEFAULT && data != null)
            {
                if (this._schemaByCSharp == null || this._schemaByCSharp.OutputStreamSchema == null)
                {
                    throw new Exception(string.Format("[Context.generateProxyMessage()] NO output schema declared!", new object[0]));
                }
                if (!this._schemaByCSharp.OutputStreamSchema.ContainsKey(streamId))
                {
                    throw new Exception(string.Format("[Context.generateProxyMessage()] NO output schema specified for {0}!", streamId));
                }
                if (this._schemaByCSharp.OutputStreamSchema[streamId].Count != data.Count)
                {
                    throw new Exception(string.Format("[Context.generateProxyMessage()] out schema ({0} fields) MISMATCH with data ({1} fields) for streamId: {2}!", this._schemaByCSharp.OutputStreamSchema.Count, data.Count, streamId));
                }

                data2 = Serializer.ConvertToByteArray(data, this._schemaByCSharp.OutputStreamSchema[streamId]);
            }
            return new ProxyMessage
            {
                StreamId = streamId,
                TupleId = tupleId,
                Evt = evt,
                Data = data2
            };
        }
        internal void FlushMsgQueue()
        {
            bool flag = false;
            try
            {
                Monitor.Enter(this, ref flag);
                if (this.msgQueue.Any<ProxyMessage>())
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    List<ProxyMessage> list = new List<ProxyMessage>();
                    ProxyMessage item;
                    while (this.msgQueue.TryDequeue(out item))
                    {
                        list.Add(item);
                    }
                    this.proxyMessageSendMethod(list);
                    stopwatch.Stop();
                }
            }
            finally
            {
                if (flag)
                {
                    Monitor.Exit(this);
                }
            }
        }

        private void proxyMessageSendMethod(List<ProxyMessage> list)
        {
            throw new NotImplementedException();
        }
        internal void CheckInputSchema(string streamId, int tupleFieldCount)
        {
            if (this._schemaByCSharp == null)
            {
                throw new Exception("[Context.CheckInputSchema()] null schema");
            }
            if (this._schemaByCSharp.InputStreamSchema == null)
            {
                throw new Exception("[Context.CheckInputSchema()] null schema.InputStreamSchema");
            }
            if (!this._schemaByCSharp.InputStreamSchema.ContainsKey(streamId))
            {
                throw new Exception(string.Format("[Context.CheckInputSchema()] unkown streamId: {0}", streamId));
            }
            if (this._schemaByCSharp.InputStreamSchema[streamId].Count != tupleFieldCount)
            {
                throw new Exception(string.Format("[Context.CheckInputSchema()] count mismatch, streamId: {0}, SchemaFieldCount: {1}, tupleFieldCount: {2}", streamId, this._schemaByCSharp.InputStreamSchema[streamId].Count, tupleFieldCount));
            }
        }
    }
}