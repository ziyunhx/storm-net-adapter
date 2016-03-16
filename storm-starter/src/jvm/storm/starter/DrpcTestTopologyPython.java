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
import backtype.storm.StormSubmitter;
import backtype.storm.drpc.DRPCSpout;
import backtype.storm.drpc.ReturnResults;
import backtype.storm.task.ShellBolt;
import backtype.storm.topology.IRichBolt;
import backtype.storm.topology.OutputFieldsDeclarer;
import backtype.storm.topology.TopologyBuilder;
import backtype.storm.tuple.Fields;

import java.util.Map;

/**
 * This topology demonstrates Storm's stream groupings and multilang capabilities.
 */
public class DrpcTestTopologyPython {
	public static class SimpleDRPC extends ShellBolt implements IRichBolt {

		public SimpleDRPC() {
			super("python", "sampledrpc.py");
		}

		@Override
		public void declareOutputFields(OutputFieldsDeclarer declarer) {
			declarer.declare(new Fields("id", "result"));
		}

		@Override
		public Map<String, Object> getComponentConfiguration() {
			return null;
		}
	}	
	
	public static void main(String[] args) throws Exception {
	  	TopologyBuilder builder = new TopologyBuilder();
		  
	  	DRPCSpout drpcSpout = new DRPCSpout("simplepydrpc");
	    builder.setSpout("drpc-input", drpcSpout,1);

	    builder.setBolt("simple", new SimpleDRPC(), 2)
	    		.noneGrouping("drpc-input");
	    
	    builder.setBolt("return", new ReturnResults(),1)
		.noneGrouping("simple");

	    Config conf = new Config();
	    conf.setDebug(true);
	    conf.setMaxTaskParallelism(1);
	    
	    try
	    {
	    	StormSubmitter.submitTopology("drpc-py", conf,builder.createTopology());
	    }
	    catch (Exception e)
	    {
	    	e.printStackTrace();
	    }
	}
}