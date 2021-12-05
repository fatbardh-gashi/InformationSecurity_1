using System;

namespace TyranIds.Common
{
	public class NetworkEventArgs : EventArgs
	{
		public string  DestinationIpAddress { get; set; }
		public string SourceIpAddress { get; set; }
		public int DestinationPort { get; set; }
		public int SourcePort { get; set; }
		public string PayloadText { get; set; }
		public DateTime Captured { get; set; }
		public Guid DatasourceId { get; set; }

		public NetworkEventArgs(string destIpAddress, int destPort, string srcIpAddress, int srcPort, string payloadText, DateTime captured, Guid datasourceId)
		{
			DestinationIpAddress = destIpAddress;
			SourceIpAddress = srcIpAddress;
			DestinationPort = destPort;
			SourcePort = srcPort;
			PayloadText = payloadText;
			DatasourceId = datasourceId;
			Captured = captured;
		}
	}
}