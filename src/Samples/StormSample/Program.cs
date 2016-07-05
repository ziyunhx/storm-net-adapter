using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Storm;

namespace StormSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Count() > 0)
            {
                string compName = args[0];

                try
                {
                    if ("generator".Equals(compName))
                    {
                        ApacheStorm.LaunchPlugin(new newPlugin(Generator.Get));
                    }
                    else if ("splitter".Equals(compName))
                    {
                        ApacheStorm.LaunchPlugin(new newPlugin(Splitter.Get));
                    }
                    else if ("counter".Equals(compName))
                    {
                        ApacheStorm.LaunchPlugin(new newPlugin(Counter.Get));
                    }
                    else if ("SimpleDRPC".Equals(compName))
                    {
                        ApacheStorm.LaunchPlugin(new newPlugin(SimpleDRPC.Get));
                    }
                    else
                    {
                        throw new Exception(string.Format("unexpected compName: {0}", compName));
                    }
                }
                catch (Exception ex)
                {
                    Context.Logger.Error(ex.ToString());
                }
            }
            else
            {
                Context.Logger.Error("Not support local model.");
            }
        }
    }
}
