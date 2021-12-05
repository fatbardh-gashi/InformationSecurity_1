using System;

namespace TyranIds.Common
{
	public interface IReportAgent
	{
		//void ReportPacketCaptured(Packet packet, DateTime captureTime, Guid sensorId);

		void ReportPacketCaptured(NetworkEventArgs args);
	}
}