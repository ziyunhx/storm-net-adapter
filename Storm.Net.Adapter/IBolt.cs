namespace Storm
{
    public interface IBolt
    {
        void Execute(StormTuple tuple);
    }
}