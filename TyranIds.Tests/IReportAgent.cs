using System;
using PacketDotNet;

namespace TyranIds.Tests
{
	public interface IReportAgent
	{
		void ReportPacketCaptured(Packet packet, DateTime captureTime, Guid sensorId);
	}
}