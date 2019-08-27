using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm
{
    public class BasicBolt
    {
        private newPlugin _createDelegate;
        private IBasicBolt _bolt;

        public BasicBolt(newPlugin createDelegate)
        {
            this._createDelegate = createDelegate;
        }
        public void Launch()
        {
            Context.Logger.Info("[BasicBolt] Launch ...");
            ApacheStorm.ctx = new BoltContext();
            IPlugin iPlugin = this._createDelegate(ApacheStorm.ctx);
            if (!(iPlugin is IBasicBolt))
            {
                Context.Logger.Error("[BasicBolt] newPlugin must return IBasicBolt!");
            }
            this._bolt = (IBasicBolt)iPlugin;

            //call Prepare method.
            this._bolt.Prepare(Context.Config, Context.TopologyContext);

            while (true)
            {
                StormTuple tuple = ApacheStorm.ReadTuple();
                if (tuple.IsHeartBeatTuple())
                    ApacheStorm.Sync();
                else
                {
                    try
                    {
                        this._bolt.Execute(tuple);
                        ApacheStorm.ctx.Ack(tuple);
                    }
                    catch (Exception ex)
                    {
                        Context.Logger.Error(ex.ToString());
                        ApacheStorm.ctx.Fail(tuple);
                    }
                }
            }
        }
    }
}
