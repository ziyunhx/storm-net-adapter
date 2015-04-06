using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Storm
{
    public class Storm
    {
        public static Queue<Command> pendingTasks = new Queue<Command>();

        /// <summary>
        /// write lines to default stream.
        /// </summary>
        /// <param name="message">stdout</param>
        public static void SendMsgToParent(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            Console.WriteLine(message);
            Console.WriteLine("end");
        }

        /// <summary>
        /// Init the Component
        /// </summary>
        public static void InitComponent(ref Config config, ref TopologyContext context)
        {
            string message = ReadMsg();

            JContainer container = JsonConvert.DeserializeObject(message) as JContainer;

            var _pidDir = container["pidDir"];
            if (_pidDir != null && _pidDir.GetType() == typeof(JValue))
            {
                string pidDir = (_pidDir as JValue).Value.ToString();
                SendPid(pidDir);
            }

            var _conf = container["conf"];
            if (_conf != null && _conf.GetType() == typeof(JContainer))
            {
                config = GetConfig(_conf as JContainer);
            }

            var _context = container["context"];
            if (_context != null && _context.GetType() == typeof(JContainer))
            {
                context = GetContext(_context as JContainer);
            }
        }

        public static Command ReadCommand()
        {
            if (pendingTasks.Count > 0)
                return pendingTasks.Dequeue();
            else
            {
                do
                {
                    string msg = ReadMsg();
                    JContainer container = JsonConvert.DeserializeObject(msg) as JContainer;

                    var _command = container["command"];
                    if (_command != null && _command.GetType() == typeof(JValue))
                    {
                        string command = (_command as JValue).Value.ToString();
                        string id = "";

                        var _id = container["id"];
                        if (_id != null && _id.GetType() == typeof(JValue))
                        {
                            id = (_id as JValue).Value.ToString();
                        }
                        return new Command(command, id);
                    }
                }
                while (true);
            }
        }

        public static StormTuple ReadTuple()
        {
            do
            {
                string msg = ReadMsg();
                JContainer container = JsonConvert.DeserializeObject(msg) as JContainer;

                string taskId, streamId, tupleId, component;
                List<object> value = new List<object>();

                try
                {
                    var _tupleId = container["id"];
                    if (_tupleId != null && _tupleId.GetType() == typeof(JValue))
                    {
                        tupleId = (_tupleId as JValue).Value.ToString();
                    }
                }
                catch { }

                try
                {
                    var _component = container["comp"];
                    if (_component != null && _component.GetType() == typeof(JValue))
                    {
                        component = (_component as JValue).Value.ToString();
                    }
                }
                catch { }

                try
                {
                    var _streamId = container["stream"];
                    if (_streamId != null && _streamId.GetType() == typeof(JValue))
                    {
                        streamId = (_streamId as JValue).Value.ToString();
                    }
                }
                catch { }

                try
                {
                    var _taskId = container["task"];
                    if (_taskId != null && _taskId.GetType() == typeof(JValue))
                    {
                        taskId = (_taskId as JValue).Value.ToString();
                    }
                }
                catch { }

                try
                {
                    var _values = container["tuple"];
                    if (_values != null && _values.GetType() == typeof(JValue))
                    {
                        tupleId = (_values as JValue).Value.ToString();
                    }
                }
                catch { }
            }
            while (true);
        }

        /// <summary>
        /// reads lines and reconstructs newlines appropriately
        /// </summary>
        /// <returns>the stdin message string</returns>
        public static string ReadMsg()
        {
            StringBuilder message = new StringBuilder();

            do
            {
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    throw new Exception("Read EOF from stdin");

                if (line == "end")
                    break;

                message.AppendLine(line);
            }
            while (true);

            return message.ToString();
        }

        public static void ReportError(string message)
        {
            SendMsgToParent("{\"command\": \"error\", \"msg\": " + message + "}");
        }

        public static void Ack(StormTuple tuple)
        {
            SendMsgToParent("{\"command\": \"ack\", \"id\": " + tuple.GetTupleId() + "}");
        }

        public static void Fail(StormTuple tuple)
        {
            SendMsgToParent("{\"command\": \"fail\", \"id\": " + tuple.GetTupleId() + "}");
        }

        public static void RpcMetrics(string name, string parms)
        {
            SendMsgToParent("{\"command\": \"metrics\", \"name\": " + name + ", \"params\": " + parms + "}");
        }

        public static void Sync()
        {
            SendMsgToParent("{\"command\": \"sync\"}");
        }

        /// <summary>
        /// sent pid to storm and create a pid file
        /// </summary>
        /// <param name="heartBeatDir">the heart beat dir</param>
        private static void SendPid(string heartBeatDir)
        {
            Process currentProcess = Process.GetCurrentProcess();
            int pid = currentProcess.Id;
            SendMsgToParent("{\"pid\": " + pid.ToString() + "}");
            File.WriteAllText(heartBeatDir + "/" + pid.ToString(), "");
        }

        private static Config GetConfig(JContainer configContainer)
        {
            Config config = new Config();

            foreach (var item in configContainer)
            {
                if (item.GetType().BaseType == typeof(JProperty))
                {
                    JProperty temp = item as JProperty;

                    if (temp.Value.GetType() == typeof(JValue))
                        config.StormConf.Add(temp.Name, (temp.Value as JValue).Value);
                }
            }
            return config;
        }

        private static TopologyContext GetContext(JContainer contextContainer)
        {
            try
            {
                int taskId = -1;
                Dictionary<int, string> component = new Dictionary<int, string>();

                var _taskId = contextContainer["taskid"];
                if (_taskId.GetType().BaseType == typeof(JValue))
                    Int32.TryParse((_taskId as JValue).Value.ToString(), out taskId);

                var _component = contextContainer["task->component"];
                if (_component != null && _component.GetType() == typeof(JContainer))
                {
                    foreach (var item in _component)
                    {
                        if (item.GetType().BaseType == typeof(JProperty))
                        {
                            JProperty temp = item as JProperty;

                            if (temp.Value.GetType() == typeof(JValue))
                                component.Add(Convert.ToInt32(temp.Name), (temp.Value as JValue).Value.ToString());
                        }
                    }
                }

                return new TopologyContext(taskId, "", component);

            }
            catch
            {
                return null;
            }
        }
    }
}