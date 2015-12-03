using System.Collections.Generic;

namespace Storm
{
    /// <summary>
    /// The storm configure json object
    /// </summary>
    public class StormConfigure
    {
        public string pidDir { set; get; }
        public Dictionary<string, object> conf { set; get; }
        public Dictionary<string, object> context { set; get; }
    }
}
