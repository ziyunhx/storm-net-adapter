Storm.Net.Adapter
======

![Build Status](https://travis-ci.org/ziyunhx/storm-net-adapter.svg?branch=master)

Overview
========

Storm.Net.Adapter is a csharp adapter for apache storm. Storm is a distributed realtime computation system.
For more information, please check the storm homepage and my personal bolg at: [storm](http://storm.apache.org/ "storm"), [The NewIdea](http://bolg.tnidea.com "The NewIdea").

Requirements
============

* .Net Framework / Mono
* Newtonsoft.Json 6.x
* Python 2.x, JDK 1.6+
* Storm 0.7.1+, Zookeeper
* Works on Windows, Linux, Mac OSX

Releases
========

You can download the latest stable and development releases from: [releases](https://github.com/ziyunhx/storm-net-adapter/releasesiyunhx/storm-net-adapter/releases "releases").


Getting started
=======

- Download the latest stable and add reference to your project.

- Add spout and bolt classes and using Storm. For more information check the StormSimple1 project.

- Build your project and copy the resources to storm topology project. Create a java topology class.

- Under Windows(.Net Framework) call your spout or bolt as follows:

		super("cmd", "/k", "CALL", "StormSimple.exe", "generator");

- Under Linux, Mac OSX, Windows(mono) call your spout or bolt as follows:
		
		super("mono", "StormSimple.exe", "generator");

- Package your topology project using Maven:

    	$ mvn package

- You can submit (run) a topology contained in this uberjar to Storm via the `storm` CLI tool:

		$ storm jar storm-starter-*-jar-with-dependencies.jar storm.starter.WordCountTopologyCsharp wordcount


Licensing
=============

Storm.Net.Adapter is licensed under the Apache License, Version 2.0. See [LICENSE](https://github.com/ziyunhx/storm-net-adapter/blob/master/LICENSE "LICENSE") for the full license text.
