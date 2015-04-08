using System;
using System.Linq;
using Storm;

namespace StormSample1
{
    class HelloWorld
    {
        static void Main(string[] args)
        {
            if (args.Count() > 0)
            {
                string compName = args[0];

                if ("generator".Equals(compName))
                {

                }
                else if ("splitter".Equals(compName))
                {

                }
                else if ("counter".Equals(compName))
                {

                }
                else
                {
                    throw new Exception(string.Format("unexpected compName: {0}", compName));
                }
            }
            else
            {
                throw new Exception("Not support local model.");
            }
        }
    }
}
