using System;

namespace EventAnnotationUtil.Elasticsearch
{
	class Event
	{
		public string RunId { get; set; }
		public string Comment { get; set; }
		public DateTime Timestamp { get; set; }

		public override string ToString()
		{
			return $"event (Timestamp:{Timestamp} RunId:{RunId} Comment:{Comment}";
		}
	}
}
