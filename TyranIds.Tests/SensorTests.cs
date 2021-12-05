using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Core;
using TyranIds.Common;

namespace TyranIds.Tests
{
	[TestClass]
	public class SensorTests
	{
		//Sensors
		//Configure the sensor
		//A sensor receives network information
		//It checks the information against a rule set
		//If there is a match it generates an alert.

		//
		[TestMethod]
		public void UseSubSensorWithInformationSource_NormalData_CountsInformationItemsCorrectly()
		{
			//arrange
			var sensor = Substitute.For<ISensor>();
			var informationSource = Substitute.For<IInformationSource>();
			var netEvenTArgs = new NetworkEventArgs(string.Empty, 0, string.Empty, 0, string.Empty, DateTime.Now, Guid.NewGuid());
			informationSource.AddNetworkMessage(netEvenTArgs);
			informationSource.AddNetworkMessage(netEvenTArgs);
			informationSource.AddNetworkMessage(netEvenTArgs);
			informationSource.BufferCount.Returns(3);

			//act
			sensor.InformationSource = informationSource;

			//assert
			Assert.AreEqual(3,sensor.InformationSource.BufferCount);
		}

		[TestMethod]
		public void CreateSimpleSensorWithInformationSource_NormalData_CountsInformationItemsCorrectly()
		{
			//arrange
			var informationSource = new SimpleInformationSource();
			var netEvenTArgs = new NetworkEventArgs(string.Empty, 0, string.Empty, 0, string.Empty, DateTime.Now, Guid.NewGuid());
			informationSource.AddNetworkMessage(netEvenTArgs);
			informationSource.AddNetworkMessage(netEvenTArgs);
			informationSource.AddNetworkMessage(netEvenTArgs);
			var sensor = new Sensor(informationSource, null);

			//act
			
			//assert
			Assert.AreEqual(3,sensor.UnreadBufferCount);
		}

		[TestMethod]
		public void CreateSimpleSensorWithInformationSource_NormalData_ProcessMessageCorrectly()
		{
			//arrange
			var informationSource = new SimpleInformationSource();
			var netEvenTArgs = new NetworkEventArgs(string.Empty, 0, string.Empty, 0, "hacking attempt", DateTime.Now, Guid.NewGuid());
			var netEvenTArgs2 = new NetworkEventArgs(string.Empty, 0, string.Empty, 0, "hello", DateTime.Now, Guid.NewGuid());
			var netEvenTArgs3 = new NetworkEventArgs(string.Empty, 0, string.Empty, 0, "hello", DateTime.Now, Guid.NewGuid());
			informationSource.AddNetworkMessage(netEvenTArgs);
			informationSource.AddNetworkMessage(netEvenTArgs2);
			informationSource.AddNetworkMessage(netEvenTArgs3);
			SimpleRule rule = new SimpleRule("hacking attempt");
			var sensor = new Sensor(informationSource, rule);

			//act
			bool result = sensor.ProcessNextMessage();
			
			//assert
			Assert.AreEqual(true,result);
			Assert.AreEqual(2, sensor.UnreadBufferCount);
		}

				[TestMethod]
		public void CreateSimpleSensorWithInformationSource_FtpAdminLoginData_ProcessMessageCorrectly()
		{
			//arrange
			var informationSource = new SimpleInformationSource();
			var netEvenTArgs = new NetworkEventArgs(string.Empty, 0, string.Empty, 0, "USER admin", DateTime.Now, Guid.NewGuid());
			var netEvenTArgs2 = new NetworkEventArgs(string.Empty, 0, string.Empty, 0, "hello", DateTime.Now, Guid.NewGuid());
			var netEvenTArgs3 = new NetworkEventArgs(string.Empty, 0, string.Empty, 0, "hello", DateTime.Now, Guid.NewGuid());
			informationSource.AddNetworkMessage(netEvenTArgs);
			informationSource.AddNetworkMessage(netEvenTArgs2);
			informationSource.AddNetworkMessage(netEvenTArgs3);
			SimpleRule ftpRule = new SimpleRule("USER admin");
			var sensor = new Sensor(informationSource, ftpRule);

			//act
			bool result = sensor.ProcessNextMessage();
			
			//assert
			Assert.AreEqual(true,result);
			Assert.AreEqual(2, sensor.UnreadBufferCount);
		}
	}
}
