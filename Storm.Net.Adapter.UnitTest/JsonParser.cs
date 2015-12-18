using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;

namespace Storm.Net.Adapter.UnitTest
{
    [TestClass]
    public class JsonParser
    {
        [TestMethod]
        public void TestInitConfig()
        {
            string conf = 
@"{
    ""conf"": {
        ""topology.message.timeout.secs"": 3,
    },
    ""pidDir"": ""..."",
    ""context"": {
        ""task->component"": {
            ""1"": ""example-spout"",
            ""2"": ""__acker"",
            ""3"": ""example-bolt1"",
            ""4"": ""example-bolt2""
        },
        ""taskid"": 3,
        ""componentid"": ""example-bolt""
        ""stream->target->grouping"": {
            ""default"": {
                ""example-bolt2"": {
                    ""type"": ""SHUFFLE""}}},
        ""streams"": [""default""],
        ""stream->outputfields"": {""default"": [""word""]},
        ""source->stream->grouping"": {
            ""example-spout"": {
                ""default"": {
                    ""type"": ""FIELDS"",
                    ""fields"": [""word""]
                }
            }
        }
        ""source->stream->fields"": {
            ""example-spout"": {
                ""default"": [""word""]
            }
        }
    }
}";

            StormConfigure result = JsonConvert.DeserializeObject<StormConfigure>(conf);
            Assert.IsTrue(result.pidDir.Equals("..."));
            Assert.IsTrue(Convert.ToInt32(result.context["taskid"]) == 3);
        }

        [TestMethod]
        public void TestCommand()
        {
            string command1 = "{\"command\": \"next\"}";
            string command2 = "{\"command\": \"ack\", \"id\": \"1231231\"}";
            string command3 = "{\"command\": \"fail\", \"id\": \"1231231\"}";
            string command4 = 
@"{
    ""command"": ""emit"",
    ""id"": ""1231231"",
    ""stream"": ""1"",
    ""task"": 9,
    ""tuple"": [""field1"", 2, 3]
}";
            string command5 = 
@"{
    ""command"": ""log"",
    ""msg"": ""hello world!""
}";
            string command6 = @"{
    ""id"": ""-6955786537413359385"",
    ""comp"": ""1"",
    ""stream"": ""1"",
    ""task"": 9,
    ""tuple"": [""snow white and the seven dwarfs"", ""field2"", 3]
}";
            string command7 = @"{
    ""id"": ""-6955786537413359385"",
    ""comp"": ""1"",
    ""stream"": ""__heartbeat"",
    ""task"": -1,
    ""tuple"": []
}";

            Command result1 = JsonConvert.DeserializeObject<Command>(command1);
            Command result2 = JsonConvert.DeserializeObject<Command>(command2);
            Command result3 = JsonConvert.DeserializeObject<Command>(command3);            
            Command result4 = JsonConvert.DeserializeObject<Command>(command4);
            Command result5 = JsonConvert.DeserializeObject<Command>(command5);
            Command result6 = JsonConvert.DeserializeObject<Command>(command6);
            Command result7 = JsonConvert.DeserializeObject<Command>(command7);

            Assert.IsTrue(result1.command.Equals("next"));
            Assert.IsTrue(result2.command.Equals("ack"));
            Assert.IsTrue(result2.id.Equals("1231231"));
            Assert.IsTrue(result3.command.Equals("fail"));
            Assert.IsTrue(result4.command.Equals("emit"));
            Assert.IsTrue(result4.stream.Equals("1"));
            Assert.IsTrue(result4.task == 9);
            Assert.IsTrue(result4.tuple.Length == 3);
            Assert.IsTrue(result5.command.Equals("log"));
            Assert.IsTrue(result5.msg.Equals("hello world!"));

            Assert.IsTrue(result6.id.Equals("-6955786537413359385"));
            Assert.IsTrue(result6.comp.Equals("1"));
            Assert.IsTrue(result7.tuple.Length == 0);
        }
    }
}
