using Storm;
using System;
using System.Collections.Generic;
using System.Threading;

namespace StormSample1
{
    /// <summary>
    /// The bolt "splitter" will split the sentences to words and emit these words to "counter" bolt. 
    /// </summary>
    public class Splitter : IBolt
    {
        private Context ctx;
        private bool enableAck = false;
        private int msgTimeoutSecs;

        private Random rnd = new Random();

        public Splitter(Context ctx)
        {
            Context.Logger.Info("Splitter constructor called");
            this.ctx = ctx;

            // Declare Input and Output schemas
            Dictionary<string, List<Type>> inputSchema = new Dictionary<string, List<Type>>();
            inputSchema.Add("default", new List<Type>() { typeof(string) });
            Dictionary<string, List<Type>> outputSchema = new Dictionary<string, List<Type>>();
            outputSchema.Add("default", new List<Type>() { typeof(string), typeof(char) });
            this.ctx.DeclareComponentSchema(new ComponentStreamSchema(inputSchema, outputSchema));

            Context.Logger.Info("enableAck: {0}", enableAck);

            // Demo how to get stormConf info
            if (Context.Config.StormConf.ContainsKey("topology.message.timeout.secs"))
            {
                msgTimeoutSecs = (int)(Context.Config.StormConf["topology.message.timeout.secs"]);
            }
            Context.Logger.Info("msgTimeoutSecs: {0}", msgTimeoutSecs);
        }

        /// <summary>
        /// The Execute() function will be called, when a new tuple is available.
        /// </summary>
        /// <param name="tuple"></param>
        public void Execute(StormTuple tuple)
        {
            Context.Logger.Info("Execute enter");

            string sentence = tuple.GetString(0);
            foreach (string word in sentence.Split(' '))
            {
                Context.Logger.Info("Emit: {0}", word);
                this.ctx.Emit("default", new List<StormTuple> { tuple }, new List<object> { word, word[0] });
            }

            if (enableAck)
            {
                if (Sample(50)) // this is to demo how to fail tuple. We do it randomly
                {
                    Context.Logger.Info("fail tuple: tupleId: {0}", tuple.GetTupleId());
                    this.ctx.Fail(tuple);
                }
                else
                {
                    if (Sample(50)) // this is to simulate timeout
                    {
                        Context.Logger.Info("sleep {0} seconds", msgTimeoutSecs + 1);
                        Thread.Sleep((msgTimeoutSecs + 1) * 1000);
                    }
                    Context.Logger.Info("Ack tuple: tupleId: {0}", tuple.GetTupleId());
                    this.ctx.Ack(tuple);
                }
            }

            Context.Logger.Info("Execute exit");
        }

        /// <summary>
        ///  Implements of delegate "newPlugin", which is used to create a instance of this spout/bolt
        /// </summary>
        /// <param name="ctx">Context instance</param>
        /// <param name="parms">Parameters to initialize this spout/bolt</param>
        /// <returns></returns>
        public static Splitter Get(Context ctx)
        {
            return new Splitter(ctx);
        }

        private bool Sample(int sampleRate)
        {
            bool result = false;
            int n = rnd.Next(sampleRate);
            if (n == 0)
            {
                result = true;
            }
            return result;
        }
    }
}