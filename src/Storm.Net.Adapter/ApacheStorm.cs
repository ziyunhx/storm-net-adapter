using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Storm
{
    public class ApacheStorm
    {
        public static Context ctx;
        public static Queue<Command> pendingCommands = new Queue<Command>();
        public static Queue<string> pendingTaskIds = new Queue<string>();

        public static void LaunchPlugin(newPlugin createDelegate)
        {
            TopologyContext context = null;
            Config config = new Config();

            Context.pluginType = PluginType.UNKNOW;
#if NET40
            Type classType = createDelegate.Method.ReturnType;
#else
            Type classType = createDelegate.GetMethodInfo().ReturnType;
#endif
            Type[] interfaces = classType.GetInterfaces();

            foreach (Type eachType in interfaces)
            {
                if (eachType == typeof(ISpout))
                {
                    Context.pluginType = PluginType.SPOUT;
                    break;
                }
                else if (eachType == typeof(IBolt))
                {
                    Context.pluginType = PluginType.BOLT;
                    break;
                }
                else if (eachType == typeof(IBasicBolt))
                {
                    Context.pluginType = PluginType.BASICBOLT;
                    break;
                }
            }

            InitComponent(ref config, ref context);
            Context.Config = config;
            Context.TopologyContext = context;

            PluginType pluginType = Context.pluginType;
            Context.Logger.Info("LaunchPlugin, pluginType: {0}", new object[]
            {
                pluginType
            });

            switch (pluginType)
            {
                case PluginType.SPOUT:
                    {
                        Spout spout = new Spout(createDelegate);
                        spout.Launch();
                        return;
                    }
                case PluginType.BOLT:
                    {
                        Bolt bolt = new Bolt(createDelegate);
                        bolt.Launch();
                        return;
                    }
                case PluginType.BASICBOLT:
                    {
                        BasicBolt basicBolt = new BasicBolt(createDelegate);
                        basicBolt.Launch();
                        return;
                    }
                default:
                    {
                        Context.Logger.Error("unexpected pluginType: {0}!", new object[]
                        {
                        pluginType
                        });
                        return;
                    }
            }
        }


        /// <summary>
        /// write lines to default stream.
        /// </summary>
        /// <param name="message">stdout</param>
        public static void SendMsgToParent(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            //fix output bug on mono.
            var encoding = new UTF8Encoding(false);
            Console.OutputEncoding = encoding;
            Console.WriteLine(message);
            Console.WriteLine("end");
        }

        /// <summary>
        /// Init the Component
        /// </summary>
        public static void InitComponent(ref Config config, ref TopologyContext context)
        {
            string message = ReadMsg();

            StormConfigure configure = JsonConvert.DeserializeObject<StormConfigure>(message);

            if (!string.IsNullOrEmpty(configure.pidDir))
                SendPid(configure.pidDir);

            config.StormConf = configure.conf;

            //Todo: ignore the context?
            //string topologyId = "";
            //if (configure.context.ContainsKey("componentid"))
            //    topologyId = configure.context["componentid"].ToString();

            //context = new TopologyContext(Convert.ToInt32(configure.context["taskid"]), topologyId, null);
        }

        public static Command ReadCommand()
        {
            if (pendingCommands.Count > 0)
                return pendingCommands.Dequeue();
            else
            {
                do
                {
                    string msg = ReadMsg();

                    //deserialize and verify the type.
                    if (!msg.StartsWith("["))
                    {
                        Command stormCommand = JsonConvert.DeserializeObject<Command>(msg);

                        //verify the tuple.
                        if (stormCommand.tuple != null && stormCommand.tuple.Length > 0)
                        {
                            ApacheStorm.ctx.CheckInputSchema(stormCommand.stream, stormCommand.tuple.Length);
                            for (int i = 0; i < stormCommand.tuple.Length; i++)
                            {
                                stormCommand.tuple[i] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(stormCommand.tuple[i]), ctx._schemaByCSharp.InputStreamSchema[stormCommand.stream][i]);
                            }
                        }

                        return stormCommand;
                    }
                    else
                    {
                        string[] taskIds = JsonConvert.DeserializeObject<string[]>(msg);
                        if (taskIds == null || taskIds.Length == 0)
                            pendingTaskIds.Enqueue(" ");
                        else
                        {
                            foreach (string taskId in taskIds)
                                pendingTaskIds.Enqueue(taskId);
                        }
                    }
                }
                while (true);
            }
        }

        public static string ReadTaskId()
        {
            if (pendingTaskIds.Count > 0)
                return pendingTaskIds.Dequeue();
            else
            {
                do
                {
                    string msg = ReadMsg();

                    //deserialize and verify the type.
                    if (!msg.StartsWith("["))
                    {
                        Command stormCommand = JsonConvert.DeserializeObject<Command>(msg);

                        //verify the tuple.
                        if (stormCommand.tuple != null && stormCommand.tuple.Length > 0)
                        {
                            ApacheStorm.ctx.CheckInputSchema(stormCommand.stream, stormCommand.tuple.Length);
                            for (int i = 0; i < stormCommand.tuple.Length; i++)
                            {
                                stormCommand.tuple[i] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(stormCommand.tuple[i]), ctx._schemaByCSharp.InputStreamSchema[stormCommand.stream][i]);
                            }
                        }

                        pendingCommands.Enqueue(stormCommand);
                    }
                    else
                    {
                        string[] taskIds = JsonConvert.DeserializeObject<string[]>(msg);
                        if (taskIds == null || taskIds.Length == 0)
                            return "";
                        else
                            return taskIds[0];
                    }
                }
                while (true);
            }
        }

        public static StormTuple ReadTuple()
        {
            Command command = ReadCommand();
            return new StormTuple(command.tuple, command.task, command.stream, command.id, command.comp);
        }

        /// <summary>
        /// reads lines and reconstructs newlines appropriately
        /// </summary>
        /// <returns>the stdin message string</returns>
        public static string ReadMsg()
        {
            StringBuilder message = new StringBuilder();
            Stream inputStream = Console.OpenStandardInput();

            do
            {
                List<byte> bytes = new List<byte>();
                do
                {
                    byte[] _bytes = new byte[1];
                    int outputLength = inputStream.Read(_bytes, 0, 1);
                    if (outputLength < 1 || _bytes[0] == 10)
                        break;

                    bytes.AddRange(_bytes);
                }
                while (true);

                string line = Encoding.UTF8.GetString(bytes.ToArray()).TrimEnd('\r');

                if (string.IsNullOrEmpty(line))
                    Context.Logger.Error("Read EOF from stdin");

                if (line == "end")
                    break;

                message.AppendLine(line);
            }
            while (true);

            return message.ToString();
        }

        public static void ReportError(string message)
        {
            SendMsgToParent("{\"command\": \"error\", \"msg\": \"" + message + "\"}");
        }

        public static void Ack(StormTuple tuple)
        {
            SendMsgToParent("{\"command\": \"ack\", \"id\": \"" + tuple.GetTupleId() + "\"}");
        }

        public static void Fail(StormTuple tuple)
        {
            SendMsgToParent("{\"command\": \"fail\", \"id\": \"" + tuple.GetTupleId() + "\"}");
        }

        public static void RpcMetrics(string name, string parms)
        {
            SendMsgToParent("{\"command\": \"metrics\", \"name\": \"" + name + "\", \"params\": \"" + parms + "\"}");
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
            File.WriteAllText(heartBeatDir + "/" + pid.ToString(), "");
            SendMsgToParent("{\"pid\": " + pid.ToString() + "}");
        }
    }

    public class Spout
    {
        private newPlugin _createDelegate;
        private ISpout _spout;
        public Spout(newPlugin createDelegate)
        {
            this._createDelegate = createDelegate;
        }
        public void Launch()
        {
            Context.Logger.Info("[Spout] Launch ...");
            ApacheStorm.ctx = new SpoutContext();
            IPlugin iPlugin = this._createDelegate(ApacheStorm.ctx);
            if (!(iPlugin is ISpout))
            {
                Context.Logger.Error("[Spout] newPlugin must return ISpout!");
            }
            this._spout = (ISpout)iPlugin;
            //call Open method.
            this._spout.Open(Context.Config, Context.TopologyContext);

            Stopwatch stopwatch = new Stopwatch();
            while (true)
            {
                try
                {
                    stopwatch.Start();
                    Command command = ApacheStorm.ReadCommand();
                    if (command.command == "next")
                    {
                        this._spout.NextTuple();
                    }
                    else if (command.command == "ack")
                    {
                        long seqId = long.Parse(command.id);
                        this._spout.Ack(seqId);
                    }
                    else if (command.command == "fail")
                    {
                        long seqId = long.Parse(command.id);
                        this._spout.Fail(seqId);
                    }
                    else
                    {
                        Context.Logger.Error("[Spout] unexpected message.");
                    }
                    ApacheStorm.Sync();
                    stopwatch.Stop();
                }
                catch (Exception ex)
                {
                    Context.Logger.Error(ex.ToString());
                }
            }
        }
    }

    public class Bolt
    {
        private newPlugin _createDelegate;
        private IBolt _bolt;

        public Bolt(newPlugin createDelegate)
        {
            this._createDelegate = createDelegate;
        }
        public void Launch()
        {
            Context.Logger.Info("[Bolt] Launch ...");
            ApacheStorm.ctx = new BoltContext();
            IPlugin iPlugin = this._createDelegate(ApacheStorm.ctx);
            if (!(iPlugin is IBolt))
            {
                Context.Logger.Error("[Bolt] newPlugin must return IBolt!");
            }
            this._bolt = (IBolt)iPlugin;

            try
            {
                //call Prepare method.
                this._bolt.Prepare(Context.Config, Context.TopologyContext);

                while (true)
                {
                    StormTuple tuple = ApacheStorm.ReadTuple();
                    if (tuple.IsHeartBeatTuple())
                        ApacheStorm.Sync();
                    else
                    {
                        this._bolt.Execute(tuple);
                    }
                }
            }
            catch (Exception ex)
            {
                Context.Logger.Error(ex.ToString());
            }
        }
    }

    public class BasicBolt
    {
        private newPlugin _createDelegate;
        private IBasicBolt _bolt;

        public BasicBolt(newPlugin createDelegate)
        {
            this._createDelegate = createDelegate;
        }
        public void Launch()
        {
            Context.Logger.Info("[BasicBolt] Launch ...");
            ApacheStorm.ctx = new BoltContext();
            IPlugin iPlugin = this._createDelegate(ApacheStorm.ctx);
            if (!(iPlugin is IBasicBolt))
            {
                Context.Logger.Error("[BasicBolt] newPlugin must return IBasicBolt!");
            }
            this._bolt = (IBasicBolt)iPlugin;

            //call Prepare method.
            this._bolt.Prepare(Context.Config, Context.TopologyContext);

            while (true)
            {
                StormTuple tuple = ApacheStorm.ReadTuple();
                if (tuple.IsHeartBeatTuple())
                    ApacheStorm.Sync();
                else
                {
                    try
                    {
                        this._bolt.Execute(tuple);
                        ApacheStorm.ctx.Ack(tuple);
                    }
                    catch (Exception ex)
                    {
                        Context.Logger.Error(ex.ToString());
                        ApacheStorm.ctx.Fail(tuple);
                    }
                }
            }
        }
    }
}