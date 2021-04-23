using System;

using CommandLine;

namespace EventAnnotationUtil
{
	[Verb("add", HelpText = "Add event annotation")]
	class AddOptions
	{
		[Option('r', "runid" , Required = true, HelpText = "Test run id")]
		public string RunId { get; set; }

		[Option('t', "timestamp", Required = false, HelpText = "Event timestamp")]
		public DateTime? Timestamp { get; set; }

		[Option('c', "comment", Required = false, HelpText = "Event comment")]
		public string Comment { get; set; }
	}
}
