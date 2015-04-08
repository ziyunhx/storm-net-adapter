using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace StormUnitTest
{
    /// <summary>
    /// Storm 的摘要说明
    /// </summary>
    [TestClass]
    public class Storm
    {
        public Storm()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void InitComponent()
        {
            string context = 
            @"{
                    ""conf"": {
                        ""topology.message.timeout.secs"": 3,
                        // etc
                    },
                    ""context"": {
                        ""task->component"": {
                            ""1"": ""example-spout"",
                            ""2"": ""__acker"",
                            ""3"": ""example-bolt""
                        },
                        ""taskid"": 3
                    },
                    ""pidDir"": ""...""
                }";

            JContainer container = JsonConvert.DeserializeObject(context) as JContainer;

            var _pidDir = container["pidDir"];
            if (_pidDir != null && _pidDir.GetType() == typeof(JValue))
            {
                string pidDir = (_pidDir as JValue).Value.ToString();
            }
        }

        [TestMethod]
        public void ReadCommand()
        {
            string msg = @"{""command"": ""ack"", ""id"": ""1231231""}";
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
            }
        }

        [TestMethod]
        public void ReadTuple()
        {
            string msg = 
             @"{
                    // The tuple's id - this is a string to support languages lacking 64-bit precision
                    ""id"": ""-6955786537413359385"",
                    // The id of the component that created this tuple
                    ""comp"": ""1"",
                    // The id of the stream this tuple was emitted to
                    ""stream"": ""1"",
                    // The id of the task that created this tuple
                    ""task"": 9,
                    // All the values in this tuple
                    ""tuple"": [""snow white and the seven dwarfs"", ""field2"", 3]
                }";

            JContainer container = JsonConvert.DeserializeObject(msg) as JContainer;

            string taskId, streamId, tupleId, component;
            List<object> values = new List<object>();

            try
            {
                var _tupleId = container["id"];
                if (_tupleId != null && _tupleId.GetType() == typeof(JValue))
                {
                    tupleId = (_tupleId as JValue).Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                var _component = container["comp"];
                if (_component != null && _component.GetType() == typeof(JValue))
                {
                    component = (_component as JValue).Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                var _streamId = container["stream"];
                if (_streamId != null && _streamId.GetType() == typeof(JValue))
                {
                    streamId = (_streamId as JValue).Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                var _taskId = container["task"];
                if (_taskId != null && _taskId.GetType() == typeof(JValue))
                {
                    taskId = (_taskId as JValue).Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                var _values = container["tuple"];
                if (_values != null && _values.GetType() == typeof(JArray))
                {
                    foreach (var item in _values as JArray)
                    {
                        values.Add((item as JValue).Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [TestMethod]
        public void GetConfig()
        {
            string context =
            @"{
                    ""conf"": {
                        ""topology.message.timeout.secs"": 3,
                        // etc
                    },
                    ""context"": {
                        ""task->component"": {
                            ""1"": ""example-spout"",
                            ""2"": ""__acker"",
                            ""3"": ""example-bolt""
                        },
                        ""taskid"": 3
                    },
                    ""pidDir"": ""...""
                }";

            JContainer container = JsonConvert.DeserializeObject(context) as JContainer;

            var _conf = container["conf"];

            Dictionary<string, object> config = new Dictionary<string, object>();
            if (_conf != null && _conf.GetType() == typeof(JContainer))
            {
                foreach (var item in _conf)
                {
                    if (item.GetType().BaseType == typeof(JProperty))
                    {
                        JProperty temp = item as JProperty;

                        if (temp.Value.GetType() == typeof(JValue))
                            config.Add(temp.Name, (temp.Value as JValue).Value);
                    }
                }
            }
        }

        [TestMethod]
        public void GetContext()
        {
            string context =
            @"{
                    ""conf"": {
                        ""topology.message.timeout.secs"": 3,
                        // etc
                    },
                    ""context"": {
                        ""task->component"": {
                            ""1"": ""example-spout"",
                            ""2"": ""__acker"",
                            ""3"": ""example-bolt""
                        },
                        ""taskid"": 3
                    },
                    ""pidDir"": ""...""
                }";

            JContainer container = JsonConvert.DeserializeObject(context) as JContainer;

            var _context = container["context"];
            if (_context != null && _context.GetType() == typeof(JContainer))
            {
                int taskId = -1;
                Dictionary<int, string> component = new Dictionary<int, string>();

                var _taskId = _context["taskid"];
                if (_taskId.GetType().BaseType == typeof(JValue))
                    Int32.TryParse((_taskId as JValue).Value.ToString(), out taskId);

                var _component = _context["task->component"];
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
            }
        }
    }
}
