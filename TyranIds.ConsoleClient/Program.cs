using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using SharpPcap.LibPcap;

namespace TyranIds.ConsoleClient
{
	class Program
	{
		static void Main(string[] args)
		{
			CaptureDeviceList deviceList = CaptureDeviceList.Instance;
			//captureDevice = deviceList.Cast<PcapDevice>().ToList().FirstOrDefault(x => x.Interface.FriendlyName == "WiFi");
			ICaptureDevice captureDevice = deviceList.Cast<PcapDevice>().ToList().FirstOrDefault(x => x.Interface.FriendlyName == "Ethernet 2");

			SharpPcapInformationSource source = new SharpPcapInformationSource(captureDevice,2000);
			source.StartListening();

			Console.ReadLine();

			source.StopListening();
		}
	}
}
