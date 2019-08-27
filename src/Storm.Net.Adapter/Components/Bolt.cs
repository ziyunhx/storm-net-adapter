using System;

namespace Storm
{
    public class Bolt
    {
        private newPlugin _createDelegate;
        private IBolt _bolt;

        public Bolt(newPlugin createDelegate)
        {
            this._createDelegate = createDelegate;
        }
        public void Launch()
        {
            Context.Logger.Info("[Bolt] Launch ...");
            ApacheStorm.ctx = new BoltContext();
            IPlugin iPlugin = this._createDelegate(ApacheStorm.ctx);
            if (!(iPlugin is IBolt))
            {
                Context.Logger.Error("[Bolt] newPlugin must return IBolt!");
            }
            this._bolt = (IBolt)iPlugin;

            try
            {
                //call Prepare method.
                this._bolt.Prepare(Context.Config, Context.TopologyContext);

                while (true)
                {
                    StormTuple tuple = ApacheStorm.ReadTuple();
                    if (tuple.IsHeartBeatTuple())
                        ApacheStorm.Sync();
                    else
                    {
                        this._bolt.Execute(tuple);
                    }
                }
            }
            catch (Exception ex)
            {
                Context.Logger.Error(ex.ToString());
            }
        }
    }
}
