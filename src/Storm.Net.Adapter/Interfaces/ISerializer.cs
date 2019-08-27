using System;
using System.Collections.Generic;

namespace Storm
{
    public interface ISerializer
    {
        List<byte[]> Serialize(List<object> dataList);

        List<object> Deserialize(List<byte[]> dataList, List<Type> targetTypes);
    }
}
