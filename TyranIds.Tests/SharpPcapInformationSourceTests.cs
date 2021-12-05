using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpPcap;
using TyranIds.Common;

namespace TyranIds.Tests
{
	[TestClass]
	public class SharpPcapInformationSourceTests
	{
		[TestMethod]
		public void CheckSharpPcapIsWorking_ReturnAVersionNumber()
		{
			//Arrange
			//Act
			string version = SharpPcap.Version.VersionString;

			//Assert
			Assert.IsTrue(!string.IsNullOrEmpty(version));
		}

		[TestMethod]
		public void CheckDevicesAvailableToCapture_DevicesFound()
		{		
			//Arrange
			//Act
			CaptureDeviceList deviceList = CaptureDeviceList.Instance;

			//Assert
			Assert.IsTrue(deviceList.Count > 0);
		}


		[TestMethod]
		public void CheckDevicesCapture_FtpNetworkTrafficDataFound()
		{
			//Arrange
			var captureDevice = new TestCaptureDevice();
			bool ftpCaptureOccured = false;

			//Act
			SharpPcapInformationSource networkInformationSource = new SharpPcapInformationSource(captureDevice, 2000);
			networkInformationSource.NetworkAlert += delegate {
				ftpCaptureOccured = true;
			};
			networkInformationSource.StartListening();

			//Assert
			Assert.IsTrue(ftpCaptureOccured);

		}


		[TestMethod]
		public void CheckDevicesCapture_TcpAndIpInfoExtracted()
		{

			string destinationIpAddress = string.Empty;
			string sourceIpAddress = string.Empty;

			int destinationPort = 0;
			int sourcePort = 0;
			//Arrange
			var captureDevice = new TestCaptureDevice();

			//Act
			SharpPcapInformationSource networkInformationSource = new SharpPcapInformationSource(captureDevice, 2000);
			networkInformationSource.NetworkAlert += delegate(object sender, EventArgs e) {
				if (e is NetworkEventArgs)
				{
					NetworkEventArgs nArgs = e as NetworkEventArgs;

					destinationIpAddress = nArgs.DestinationIpAddress;
					sourceIpAddress = nArgs.SourceIpAddress;
					destinationPort = nArgs.DestinationPort;
					sourcePort = nArgs.SourcePort;
				}
			};
			networkInformationSource.StartListening();

			//Internet Protocol Version 4, Src: 62.181.194.206, Dst: 192.168.13.85
			//Transmission Control Protocol, Src Port: 21 (21), Dst Port: 64435 (64435), Seq: 1, Ack: 1, Len: 28
			//Assert
			Assert.AreEqual("192.168.13.85",destinationIpAddress);
			Assert.AreEqual("62.181.194.206",sourceIpAddress);
			Assert.AreEqual(64435,destinationPort);
			Assert.AreEqual(21,sourcePort);	
		}
	}
}
