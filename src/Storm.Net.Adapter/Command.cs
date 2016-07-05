using System.Collections.Generic;

namespace Storm
{
    public class Command
    {
        /// <summary>
        /// the command type.
        /// </summary>
        public string command { set; get; }
        /// <summary>
        /// The id for the tuple. Leave this out for an unreliable emit. The id can be a string or a number.
        /// </summary>
        public string id { set; get; }
        /// <summary>
        /// The id of the stream this tuple was emitted to. Leave this empty to emit to default stream.
        /// </summary>
        public string stream { set; get; }
        /// <summary>
        /// If doing an emit direct, indicate the task to send the tuple to
        /// </summary>
        public int task { set; get; }
        /// <summary>
        /// All the values in this tuple
        /// </summary>
        public object[] tuple { set; get; }
        /// <summary>
        /// the message to log
        /// </summary>
        public string msg { set; get; }
        /// <summary>
        /// The id of the component that created this tuple
        /// </summary>
        public string comp { set; get; }
    }
}