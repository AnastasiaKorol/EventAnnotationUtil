using System;

using Elasticsearch.Net;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using NLog.Fluent;

namespace EventAnnotationUtil.Elasticsearch
{
	class ElasticsearchWriter
	{
		public ElasticsearchWriter(string uri)
		{
			var pool = new SingleNodeConnectionPool(new Uri(uri));
			var connectionSettings = new Nest.ConnectionSettings(
				pool,
				sourceSerializer: (builtin, settings) => new JsonNetSerializer(
					builtin,
					settings,
					contractJsonConverters: new JsonConverter[] { new StringEnumConverter() })); // enum'ы как строки
			connectionSettings.DefaultFieldNameInferrer(i => i);
			connectionSettings.DefaultIndex(Index);

			_client = new ElasticClient(connectionSettings);
		}

		public void Write(Event data)
		{
			var response = _client.IndexDocument(data);
			if (!response.IsValid)
			{
				_logger.Error("Failed to write event to elastic index {0}: {1}", Index, response.DebugInformation);

				if (response.ServerError != null)
				{
					_logger.Error("Error: {0}", response.ServerError.Error);
				}
			}
			_logger.Info($"Added {data}");
		}

		public void Write(string runId, DateTime timestamp, string comment)
		{
			Write(new Event
			{
				RunId = runId,
				Comment = comment,
				Timestamp = timestamp
			});
		}

		public void AddTestLimits(string runId)
		{
			throw new NotImplementedException();
		}

		public void Delete(string runId)
		{
			var response = _client.DeleteByQuery<Event>(q => q
				.Query(rq => rq.Match(m => m.Field(f => f.RunId).Query(runId))
				)
			);

			if (!response.IsValid)
			{
				_logger.Error("Failed to delete data from elastic index {0}: {1}", Index, response.DebugInformation);

				if (response.ServerError != null)
				{
					_logger.Error("Error: {0}", response.ServerError.Error);
				}
			}

			_logger.Info($"Deleted all events for RunId:{runId}");
		}
		
		public void Delete(string runId, DateTime start, DateTime end)
		{
			Event t;
			var queryForm = new TermQuery
			{
				Field = nameof(t.RunId),
				Value = runId
			};

			var rangeQuery = new DateRangeQuery
			{
				Field = nameof(t.Timestamp),
				GreaterThanOrEqualTo = start,
				LessThanOrEqualTo = end
			};

			var query = new DeleteByQueryRequest<Event>
			{
				Query = queryForm && rangeQuery
			};

			var response = _client.DeleteByQuery(query);

			if (!response.IsValid)
			{
				_logger.Error("Failed to delete data from elastic index {0}: {1}", Index, response.DebugInformation);

				if (response.ServerError != null)
				{
					_logger.Error("Error: {0}", response.ServerError.Error);
				}
			}

			_logger.Info($"Deleted all events for RunId:{runId} from {start} to {end}");
		}


		private const string Index = "events";

		private readonly ElasticClient _client;
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
	}
}
