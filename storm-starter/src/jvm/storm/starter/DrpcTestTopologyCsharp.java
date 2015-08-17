/**
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package storm.starter;

import backtype.storm.Config;
import backtype.storm.LocalCluster;
import backtype.storm.LocalDRPC;
import backtype.storm.StormSubmitter;
import backtype.storm.drpc.LinearDRPCTopologyBuilder;
import backtype.storm.spout.ShellSpout;
import backtype.storm.task.ShellBolt;
import backtype.storm.topology.IRichBolt;
import backtype.storm.topology.IRichSpout;
import backtype.storm.topology.OutputFieldsDeclarer;
import backtype.storm.topology.TopologyBuilder;
import backtype.storm.tuple.Fields;

import java.util.Map;

/**
 * This topology demonstrates Storm's stream groupings and multilang capabilities.
 */
public class DrpcTestTopologyCsharp {
	public static class SimpleDRPC extends ShellBolt implements IRichBolt {

		public SimpleDRPC() {
			super("cmd", "/k", "CALL", "StormSimple.exe", "SimpleDRPC");
			
		}

		@Override
		public void declareOutputFields(OutputFieldsDeclarer declarer) {
			declarer.declare(new Fields("id", "word"));
		}

		@Override
		public Map<String, Object> getComponentConfiguration() {
			return null;
		}
	}	
	
	public static void main(String[] args) throws Exception {
		LinearDRPCTopologyBuilder builder = new LinearDRPCTopologyBuilder("simpledrpc");
	    builder.addBolt(new SimpleDRPC(), 3);

		Config conf = new Config();
		conf.setDebug(true);

		if (args == null || args.length == 0) {
		      LocalDRPC drpc = new LocalDRPC();
		      LocalCluster cluster = new LocalCluster();

		      cluster.submitTopology("drpc-demo", conf, builder.createLocalTopology(drpc));

		      for (String word : new String[]{ "hello", "goodbye" }) {
		        System.out.println("Result for \"" + word + "\": " + drpc.execute("exclamation", word));
		      }

		      cluster.shutdown();
		      drpc.shutdown();
		    }
		    else {
		      conf.setNumWorkers(1);
		      StormSubmitter.submitTopologyWithProgressBar(args[0], conf, builder.createRemoteTopology());
		    }
	}
}