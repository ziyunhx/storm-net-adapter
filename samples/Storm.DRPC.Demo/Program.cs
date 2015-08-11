using Storm.DRPC;

namespace Storm.DRPC.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            DRPCClient client = new DRPCClient("drpc-host", 3772);
            string result = client.execute("exclamation", "hello word");
        }
    }
}
