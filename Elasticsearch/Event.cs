using System;

namespace EventAnnotationUtil.Elasticsearch
{
	class Event
	{
		public string RunId { get; set; }
		public string Cluster { get; set; }
		public EventType Type { get; set; }
		public string Comment { get; set; }
		public DateTime Timestamp { get; set; }

		public override string ToString()
		{
			return $"event (Timestamp:{Timestamp} RunId:{RunId} Cluster: {Cluster} Type: {Type} Comment:{Comment}";
		}
	}
}
