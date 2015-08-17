using Storm.DRPC;
using System;

namespace Storm.DRPC.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            DRPCClient client = new DRPCClient("localhost", 3772);
            string result = client.execute("exclamation", "hello word");
            Console.WriteLine(result);
            Console.WriteLine("Please input a word and press enter, if you want quit it, press enter only!");
            do {
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    break;

                Console.WriteLine(client.execute("exclamation", input));
            }
            while (true);
        }
    }
}
