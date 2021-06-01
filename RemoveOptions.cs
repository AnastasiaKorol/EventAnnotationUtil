using System;

using CommandLine;

namespace EventAnnotationUtil
{
	[Verb("remove", HelpText = "Remove event annotations")]
	class RemoveOptions
	{
		[Option("environment", Required = true, HelpText = "Test environment name")]
		public string Cluster { get; set; }

		[Option("start", Required = true, HelpText = "Start time")]
		public DateTime StartTime { get; set; }

		[Option("end", Required = true, HelpText = "End time")]
		public DateTime EndTime { get; set; }
	}
}
