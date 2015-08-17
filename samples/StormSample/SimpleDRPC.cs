using Storm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StormSample
{
    class SimpleDRPC : IBasicBolt
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
            Context.Logger.Info("Splitter constructor called");
            this.ctx = ctx;

            // Declare Input and Output schemas
            Dictionary<string, List<Type>> inputSchema = new Dictionary<string, List<Type>>();
            inputSchema.Add("default", new List<Type>() { typeof(string), typeof(string) });
            Dictionary<string, List<Type>> outputSchema = new Dictionary<string, List<Type>>();
            outputSchema.Add("default", new List<Type>() { typeof(string), typeof(string) });
            this.ctx.DeclareComponentSchema(new ComponentStreamSchema(inputSchema, outputSchema));
        }

        public void Execute(StormTuple tuple)
        {
            Context.Logger.Info("SimpleDRPC Execute enter");
            string sentence = tuple.GetString(1) + "!";
            Context.Logger.Warn(tuple.GetString(0) + ";" + tuple.GetString(1));
            this.ctx.Emit("default", new List<StormTuple> { tuple }, new List<object> { tuple.GetString(0), sentence });
            Context.Logger.Info("SimpleDRPC Execute exit");
        }

        public void Prepare(Config stormConf, TopologyContext context)
        {
            return;
        }
    }
}
