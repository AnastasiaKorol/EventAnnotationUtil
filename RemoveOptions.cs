using System;

using CommandLine;

namespace EventAnnotationUtil
{
	[Verb("remove", HelpText = "Remove event annotations")]
	class RemoveOptions
	{
		[Option('r', "runid", Required = true, HelpText = "Test run id")]
		public string RunId { get; set; }

		[Option('s', "start", Required = false, HelpText = "Start time")]
		public DateTime? StartTime { get; set; }

		[Option('e', "end", Required = false, HelpText = "End time")]
		public DateTime? EndTime { get; set; }
	}
}
