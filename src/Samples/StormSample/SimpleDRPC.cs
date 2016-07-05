using Storm;
using System;
using System.Collections.Generic;

namespace StormSample
{
    public class SimpleDRPC : IBolt
    {
        private Context ctx;

        /// <summary>
        ///  Implements of delegate "newPlugin", which is used to create a instance of this spout/bolt
        /// </summary>
        /// <param name="ctx">Context instance</param>
        /// <returns></returns>
        public static SimpleDRPC Get(Context ctx)
        {
            return new SimpleDRPC(ctx);
        }

        public SimpleDRPC(Context ctx)
        {
            Context.Logger.Info("SimpleDRPC constructor called");
            this.ctx = ctx;

            // Declare Input and Output schemas
            Dictionary<string, List<Type>> inputSchema = new Dictionary<string, List<Type>>();
            inputSchema.Add("default", new List<Type>() { typeof(string), typeof(object) });
            Dictionary<string, List<Type>> outputSchema = new Dictionary<string, List<Type>>();
            outputSchema.Add("default", new List<Type>() { typeof(string), typeof(object) });
            this.ctx.DeclareComponentSchema(new ComponentStreamSchema(inputSchema, outputSchema));
        }

        public void Execute(StormTuple tuple)
        {
            Context.Logger.Info("SimpleDRPC Execute enter");
            
            string sentence = tuple.GetString(0) + "!";
            this.ctx.Emit("default", new List<StormTuple> { tuple }, new List<object> { sentence, tuple.GetValue(1) });
            Context.Logger.Info("SimpleDRPC Execute exit");
            ApacheStorm.Ack(tuple);
        }

        public void Prepare(Config stormConf, TopologyContext context)
        {
            return;
        }
    }
}
