using Storm;
using System;
using System.Collections.Generic;

namespace StormSample1
{
    /// <summary>
    /// The bolt "splitter" will split the sentences to words and emit these words to "counter" bolt. 
    /// </summary>
    public class Splitter : IBasicBolt
    {
        private Context ctx;
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

            // Demo how to get stormConf info
            if (Context.Config.StormConf.ContainsKey("topology.message.timeout.secs"))
            {
                msgTimeoutSecs = Convert.ToInt32(Context.Config.StormConf["topology.message.timeout.secs"]);
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
                Context.Logger.Info("Splitter Emit: {0}", word);
                this.ctx.Emit("default", new List<StormTuple> { tuple }, new List<object> { word, word[0] });
            }

            Context.Logger.Info("Splitter Execute exit");
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
    }
}