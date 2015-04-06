using System.Collections.Generic;

namespace Storm
{
    public interface ISpout
    {
        void NextTuple(Dictionary<string, object> parms);
        void Ack(long seqId, Dictionary<string, object> parms);
        void Fail(long seqId, Dictionary<string, object> parms);
    }
}