using System;

using CommandLine;

namespace EventAnnotationUtil
{
	[Verb("add", HelpText = "Add event annotation")]
	class AddOptions
	{
		[Option("timestamp", Required = true, HelpText = "Event timestamp")]
		public DateTime Timestamp { get; set; }

		[Option("runid", Required = false, HelpText = "Test run id")]
		public string RunId { get; set; }

		[Option("environment", Required = true, HelpText = "Test environment name")]
		public string Cluster { get; set; }

		[Option("comment", Required = false, HelpText = "Event comment")]
		public string Comment { get; set; }

		[Option("type", Required = true, HelpText = "Event type")]
		public EventType Type { get; set; }
	}
}
