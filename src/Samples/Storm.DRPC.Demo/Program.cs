using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Storm.DRPC;

namespace Storm.DRPC.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DRPCClient client = new DRPCClient("host", 3772);
            string result = client.execute("simpledrpc", "hello word");
            Console.WriteLine(result);
            Console.WriteLine("Please input a word and press enter, if you want quit it, press enter only!");
            do
            {
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    break;

                Console.WriteLine(client.execute("simpledrpc", input));
            }
            while (true);
        }
    }
}
