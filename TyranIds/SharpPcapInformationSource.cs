using System;
using System.CodeDom;
using System.Text.RegularExpressions;
using PacketDotNet;
using SharpPcap;
using TyranIds.Common;

namespace TyranIds
{
	public class SharpPcapInformationSource : SimpleInformationSource, IActiveInformationSource
	{
		private ICaptureDevice captureDevice;

		public event EventHandler NetworkAlert;

		private readonly Guid id;

		public SharpPcapInformationSource(TestCaptureDevice device, int i)
		{
			captureDevice = device;
			captureDevice.OnPacketArrival += CaptureDevice_OnPacketArrival;
			id = Guid.NewGuid();
		}

		private void CaptureDevice_OnPacketArrival(object sender, CaptureEventArgs e)
		{
			NetworkEventArgs netArgs = ExtractNetworkInformation(e.Packet);

			AddNetworkMessage(netArgs);

			if (NetworkAlert != null)
				NetworkAlert(this, netArgs);
		}

		private NetworkEventArgs ExtractNetworkInformation(RawCapture packet)
		{
			EthernetPacket ethernetPacket = PacketDotNet.Packet.ParsePacket(LinkLayers.Ethernet, packet.Data) as EthernetPacket;
			IpPacket ipPacket = ethernetPacket.Extract(typeof(IpPacket)) as IpPacket;
			TcpPacket tcpPacket = ethernetPacket.Extract(typeof(TcpPacket)) as TcpPacket;

			string rawMessageText = System.Text.Encoding.UTF8.GetString(ethernetPacket.Bytes);
			
			string cleanMessageText = Regex.Replace(rawMessageText,@"[^a-zA-Z0-9`!@#$%^&*()_+|\-=\\{}\[\]:"";'<>?,./]", "");

			return new NetworkEventArgs(ipPacket.DestinationAddress.ToString(), tcpPacket.DestinationPort, ipPacket.SourceAddress.ToString(), tcpPacket.SourcePort, cleanMessageText, DateTime.Now, id);
		}

		public void StartListening()
		{
			captureDevice.StartCapture();
		}

		public void StopListening()
		{
			captureDevice.StopCapture();
		}
	}
}