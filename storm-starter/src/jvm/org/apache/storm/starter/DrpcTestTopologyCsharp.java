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
package org.apache.storm.starter;

import org.apache.storm.Config;
import org.apache.storm.LocalCluster;
import org.apache.storm.LocalDRPC;
import org.apache.storm.StormSubmitter;
import org.apache.storm.drpc.DRPCSpout;
import org.apache.storm.drpc.LinearDRPCTopologyBuilder;
import org.apache.storm.drpc.ReturnResults;
import org.apache.storm.spout.ShellSpout;
import org.apache.storm.task.ShellBolt;
import org.apache.storm.topology.IRichBolt;
import org.apache.storm.topology.IRichSpout;
import org.apache.storm.topology.OutputFieldsDeclarer;
import org.apache.storm.topology.TopologyBuilder;
import org.apache.storm.tuple.Fields;

import java.util.Map;

/**
 * This topology demonstrates Storm's stream groupings and multilang capabilities.
 */
public class DrpcTestTopologyCsharp {
	public static class SimpleDRPC extends ShellBolt implements IRichBolt {

		public SimpleDRPC() {
			super("dotnet", "StormSample.dll", "SimpleDRPC");
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
		  
	  	DRPCSpout drpcSpout = new DRPCSpout("simpledrpc");
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
	    	StormSubmitter.submitTopology("drpc-q", conf,builder.createTopology());
	    }
	    catch (Exception e)
	    {
	    	e.printStackTrace();
	    }
	}
}