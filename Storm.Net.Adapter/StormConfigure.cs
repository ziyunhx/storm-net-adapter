using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
