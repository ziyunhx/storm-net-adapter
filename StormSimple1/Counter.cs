using Storm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace StormSample1
{
    /// <summary>
    /// The bolt "counter" uses a dictionary to record the occurrence number of each word.
    /// </summary>
    public class Counter : IBolt
    {
        private Context ctx;
        private bool enableAck = true;
        private int taskIndex = -1;

        private Dictionary<string, int> counts = new Dictionary<string, int>();

        public Counter(Context ctx)
        {
            Context.Logger.Info("Counter constructor called");

            this.ctx = ctx;

            // Declare Input and Output schemas
            Dictionary<string, List<Type>> inputSchema = new Dictionary<string, List<Type>>();
            inputSchema.Add("default", new List<Type>() { typeof(string), typeof(char) });

            Dictionary<string, List<Type>> outputSchema = new Dictionary<string, List<Type>>();
            outputSchema.Add("default", new List<Type>() { typeof(string), typeof(int) });
            this.ctx.DeclareComponentSchema(new ComponentStreamSchema(inputSchema, outputSchema));

            Context.Logger.Info("enableAck: {0}", enableAck);
        }

        /// <summary>
        /// The Execute() function will be called, when a new tuple is available.
        /// </summary>
        /// <param name="tuple"></param>
        public void Execute(StormTuple tuple)
        {
            Context.Logger.Info("Execute enter");

            string word = tuple.GetString(0);
            int count = counts.ContainsKey(word) ? counts[word] : 0;
            count++;
            counts[word] = count;

            Context.Logger.Info("Emit: {0}, count: {1}", word, count);
            this.ctx.Emit("default", new List<StormTuple> { tuple }, new List<object> { word, count });

            if (enableAck)
            {
                Context.Logger.Info("Ack tuple: tupleId: {0}", tuple.GetTupleId());
                this.ctx.Ack(tuple);
            }

            Context.Logger.Info("Execute exit");

        }

        /// <summary>
        ///  Implements of delegate "newPlugin", which is used to create a instance of this spout/bolt
        /// </summary>
        /// <param name="ctx">Context instance</param>
        /// <param name="parms">Parameters to initialize this spout/bolt</param>
        /// <returns></returns>
        public static Counter Get(Context ctx)
        {
            return new Counter(ctx);
        }
    }
}