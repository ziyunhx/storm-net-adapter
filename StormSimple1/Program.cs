using System;
using System.Linq;
using Storm;
using HooLab.Runtime;

namespace StormSample1
{
    class HelloWorld
    {
        static void Main(string[] args)
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
                    else
                    {
                        throw new Exception(string.Format("unexpected compName: {0}", compName));
                    }
                }
                catch (Exception ex)
                {
                    Context.Logger.Info(ex.ToString());
                }
            }
            else
            {
                throw new Exception("Not support local model.");
            }
        }
    }
}
