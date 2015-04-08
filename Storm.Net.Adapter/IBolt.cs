namespace Storm
{
    public interface IBolt : IPlugin
    {
        void Execute(StormTuple tuple);
    }
}