using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

using EventAnnotationUtil.Elasticsearch;
using Microsoft.Extensions.Configuration;
using NLog;

namespace EventAnnotationUtil
{
	class Program
	{
		static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false)
				.Build();

			var parser = new Parser(cfg => { cfg.CaseInsensitiveEnumValues = true; cfg.HelpWriter = Console.Out; });
			parser.ParseArguments<AddOptions, RemoveOptions>(args)
				.MapResult(
					(AddOptions opts) => AddEvents(opts, config["ElasticsearchUri"]),
					(RemoveOptions opts) => RemoveEvents(opts, config["ElasticsearchUri"]),
					(IEnumerable<Error> errs) => {
						Console.WriteLine("Failed to parse commandline options");
						Console.ReadKey();
						return 0;
					});

			LogManager.Shutdown();

			Console.WriteLine("Press any key...");
			Console.ReadKey();
		}

		static int AddEvents(AddOptions options, string uri)
		{
			var es = new ElasticsearchWriter(uri);

			if (!options.Timestamp.HasValue) es.AddTestLimits(options.RunId);
			else es.Write(options.RunId, options.Timestamp.Value, options.Comment);


			return 0;
		}

		static int RemoveEvents(RemoveOptions options, string uri)
		{
			var es = new ElasticsearchWriter(uri);

			if (!options.StartTime.HasValue) es.Delete(options.RunId);
			else es.Delete(options.RunId, options.StartTime.Value, options.EndTime.Value);

			return 0;
		}
	}
}
