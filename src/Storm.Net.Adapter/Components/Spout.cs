using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm
{
    public class Spout
    {
        private newPlugin _createDelegate;
        private ISpout _spout;
        public Spout(newPlugin createDelegate)
        {
            this._createDelegate = createDelegate;
        }
        public void Launch()
        {
            Context.Logger.Info("[Spout] Launch ...");
            ApacheStorm.ctx = new SpoutContext();
            IPlugin iPlugin = this._createDelegate(ApacheStorm.ctx);
            if (!(iPlugin is ISpout))
            {
                Context.Logger.Error("[Spout] newPlugin must return ISpout!");
            }
            this._spout = (ISpout)iPlugin;
            //call Open method.
            this._spout.Open(Context.Config, Context.TopologyContext);

            while (true)
            {
                try
                {
                    Command command = ApacheStorm.ReadCommand();
                    if (command.command == "next")
                    {
                        this._spout.NextTuple();
                    }
                    else if (command.command == "ack")
                    {
                        long seqId = long.Parse(command.id);
                        this._spout.Ack(seqId);
                    }
                    else if (command.command == "fail")
                    {
                        long seqId = long.Parse(command.id);
                        this._spout.Fail(seqId);
                    }
                    else
                    {
                        Context.Logger.Error("[Spout] unexpected message.");
                    }
                    ApacheStorm.Sync();
                }
                catch (Exception ex)
                {
                    Context.Logger.Error(ex.ToString());
                }
            }
        }
    }
}
