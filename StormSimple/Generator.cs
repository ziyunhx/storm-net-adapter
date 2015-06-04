using Storm;
using System;
using System.Collections.Generic;
using System.Threading;

namespace StormSample
{
    /// <summary>
    /// The spout "generator" will randomly generate some sentences, and emit these sentences to "splitter". 
    /// </summary>
    public class Generator : ISpout
    {
        private const int MAX_PENDING_TUPLE_NUM = 20;

        private Context ctx;

        private long lastSeqId = 0;
        private Dictionary<long, string> cachedTuples = new Dictionary<long, string>();

        private Random rand = new Random();
        string[] sentences = new string[] {
                                          "the cow jumped over the moon",
                                          "an apple a day keeps the doctor away",
                                          "four score and seven years ago",
                                          "snow white and the seven dwarfs",
                                          "i am at two with nature"};

        public Generator(Context ctx)
        {
            Context.Logger.Info("Generator constructor called");
            this.ctx = ctx;

            // Declare Output schema
            Dictionary<string, List<Type>> outputSchema = new Dictionary<string, List<Type>>();
            outputSchema.Add("default", new List<Type>() { typeof(string) });
            this.ctx.DeclareComponentSchema(new ComponentStreamSchema(null, outputSchema));
        }

        /// <summary>
        /// This method is used to emit one or more tuples. If there is nothing to emit, this method should return without emitting anything. 
        /// It should be noted that NextTuple(), Ack(), and Fail() are all called in a tight loop in a single thread in C# process. 
        /// When there are no tuples to emit, it is courteous to have NextTuple sleep for a short amount of time (such as 10 milliseconds), so as not to waste too much CPU.
        /// </summary>
        public void NextTuple()
        {
            Context.Logger.Info("NextTuple enter");
            string sentence;

            if (cachedTuples.Count <= MAX_PENDING_TUPLE_NUM)
            {
                lastSeqId++;
                sentence = sentences[rand.Next(0, sentences.Length - 1)];
                Context.Logger.Info("Generator Emit: {0}, seqId: {1}", sentence, lastSeqId);
                this.ctx.Emit("default", new List<object>() { sentence }, lastSeqId);
                cachedTuples[lastSeqId] = sentence;
            }
            else
            {
                // if have nothing to emit, then sleep for a little while to release CPU
                Thread.Sleep(50);
            }
            Context.Logger.Info("cached tuple num: {0}", cachedTuples.Count);

            Context.Logger.Info("Generator NextTx exit");
        }

        /// <summary>
        /// Ack() will be called only when ack mechanism is enabled in spec file.
        /// If ack is not supported in non-transactional topology, the Ack() can be left as empty function. 
        /// </summary>
        /// <param name="seqId">Sequence Id of the tuple which is acked.</param>
        public void Ack(long seqId)
        {
            Context.Logger.Info("Ack, seqId: {0}", seqId);
            bool result = cachedTuples.Remove(seqId);
            if (!result)
            {
                Context.Logger.Warn("Ack(), remove cached tuple for seqId {0} fail!", seqId);
            }
        }

        /// <summary>
        /// Fail() will be called only when ack mechanism is enabled in spec file. 
        /// If ack is not supported in non-transactional topology, the Fail() can be left as empty function.
        /// </summary>
        /// <param name="seqId">Sequence Id of the tuple which is failed.</param>
        public void Fail(long seqId)
        {
            Context.Logger.Info("Fail, seqId: {0}", seqId);
            if (cachedTuples.ContainsKey(seqId))
            {
                string sentence = cachedTuples[seqId];
                Context.Logger.Info("Re-Emit: {0}, seqId: {1}", sentence, seqId);
                this.ctx.Emit("default", new List<object>() { sentence }, seqId);
            }
            else
            {
                Context.Logger.Warn("Fail(), can't find cached tuple for seqId {0}!", seqId);
            }
        }

        /// <summary>
        ///  Implements of delegate "newPlugin", which is used to create a instance of this spout/bolt
        /// </summary>
        /// <param name="ctx">Context instance</param>
        /// <returns></returns>
        public static Generator Get(Context ctx)
        {
            return new Generator(ctx);
        }

        /// <summary>
        /// Called when a task for this component is initialized within a worker on the cluster.
        /// It provides the spout with the environment in which the spout executes.
        /// </summary>
        /// <param name="config">The Storm configuration for this spout. This is the configuration provided to the topology merged in with cluster configuration on this machine.</param>
        /// <param name="topologyContext">This object can be used to get information about this task's place within the topology, including the task id and component id of this task, input and output information, etc.</param>
        public void Open(Config config, TopologyContext topologyContext)
        {
            return;
        }
    }
}