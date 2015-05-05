namespace Storm
{
    public interface IBasicBolt : IPlugin
    {
        void Execute(StormTuple tuple);
    }
}