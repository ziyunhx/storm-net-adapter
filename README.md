Storm.Net.Adapter
======

[![Build status](https://travis-ci.org/ziyunhx/storm-net-adapter.svg?branch=master)](https://travis-ci.org/ziyunhx/storm-net-adapter)

Overview
========

Storm.Net.Adapter is a csharp adapter for apache storm. Storm is a distributed realtime computation system.
For more information, please check the storm homepage and my personal bolg at: [storm](http://storm.apache.org/ "storm"), [The NewIdea (zh-CN)](http://blog.tnidea.com/ "The NewIdea").

Requirements
============

* .Net Framework / Mono
* Thrift 0.9.x
* Python 2.x, JDK 1.6+, Maven 3.x
* Storm 0.7.1+, Zookeeper
* Works on Windows, Linux, Mac OSX

Releases
========

- You can download the latest stable and development releases from: [releases](https://github.com/ziyunhx/storm-net-adapter/releases "releases").

- Or using Nuget:

		PM> Install-Package Storm.Net.Adapter

- Or clone and build it by yourself.


Build and install Storm jars locally
========

If you are using the latest development version of Storm, e.g. by having cloned the Storm git repository, then you must first perform a local build of Storm itself. Otherwise you will run into Maven errors such as "Could not resolve dependencies for project `org.apache.storm:storm-starter:<storm-version>-SNAPSHOT"`.

    # Must be run from the top-level directory of the Storm code repository
    $ mvn clean install -DskipTests=true

This command will build Storm locally and install its jar files to your user's `$HOME/.m2/repository/`. When you run the Maven command to build and run storm-starter (see below), Maven will then be able to find the corresponding version of Storm in this local Maven repository at `$HOME/.m2/repository`.


Getting started
=======



- Add spout classes based on ISpout and add bolt classes based on IBolt or IBasicBolt, Both of those need using Storm. For more information check the [StormSimple project](https://github.com/ziyunhx/storm-net-adapter/tree/master/samples/StormSimple "StormSimple Project").

- Build your project and copy the resources to storm topology project. Create a java topology class. For more information check the [storm-starter](https://github.com/ziyunhx/storm-net-adapter/tree/master/storm-starter "storm-starter").

- Under Windows(.Net Framework) call your spout or bolt as follows:

		super("cmd", "/k", "CALL", "StormSimple.exe", "generator");

- Under Linux, Mac OSX, Windows(mono) call your spout or bolt as follows:
		
		super("mono", "StormSimple.exe", "generator");

- Package your topology project using Maven:

    	$ mvn package

- You can submit (run) a topology contained in this uberjar to Storm via the `storm` CLI tool:

		$ storm jar storm-starter-*-jar-with-dependencies.jar storm.starter.WordCountTopologyCsharp wordcount

Remote DRPC
=============

 Storm.Net.Adapter is also support DRPC now. You can call the remote DRPC like this:

	DRPCClient client = new DRPCClient("drpc-host", 3772);
	string result = client.execute("exclamation", "hello word");

Licensing
=============

Storm.Net.Adapter is licensed under the Apache License, Version 2.0. See [LICENSE](http://www.apache.org/licenses/LICENSE-2.0.html "LICENSE") for the full license text.
