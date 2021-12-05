using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Core;

namespace TyranIds.Tests
{
	[TestClass]
	public class SenorTests
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
			var informationSource = Substitute.For<INetworkTrafficInformationSource>();
			informationSource.AddNetworkMessage("hello");
			informationSource.AddNetworkMessage("hello");
			informationSource.AddNetworkMessage("hello");
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
			informationSource.AddNetworkMessage("hello");
			informationSource.AddNetworkMessage("hello");
			informationSource.AddNetworkMessage("hello");
			var sensor = new Sensor(informationSource);

			//act
			
			//assert
			Assert.AreEqual(3,sensor.UnreadBufferCount);
		}


		[TestMethod]
		public void CreateSimpleSensorWithInformationSource_NormalData_ProcessMessageCorrectly()
		{
			//arrange
			var informationSource = new SimpleInformationSource();
			informationSource.AddNetworkMessage("hacking attempt");
			informationSource.AddNetworkMessage("hello");
			informationSource.AddNetworkMessage("hello");
			var sensor = new Sensor(informationSource);

			//act
			bool result = sensor.ProcessNextMessage();
			
			//assert
			Assert.AreEqual(true,result);
			Assert.AreEqual(2, sensor.UnreadBufferCount);
		}
	}

	public class SimpleInformationSource : INetworkTrafficInformationSource
	{
		private readonly List<string> messageBuffer;
		public void AddNetworkMessage(string networkData)
		{
			messageBuffer.Add(networkData);
		}

		public int BufferCount => messageBuffer.Count;
		public string GetNextMessage()
		{
			if (messageBuffer.Any())
			{
				string nextMesage = messageBuffer.First();
				messageBuffer.RemoveAt(0);
				return nextMesage;
			} else return string.Empty;
		}

		public SimpleInformationSource()
		{
			messageBuffer = new List<string>();
		}
		
	}

	public class Sensor
	{
		private readonly INetworkTrafficInformationSource informationSource;

		public Sensor(INetworkTrafficInformationSource infoSource)
		{
			informationSource = infoSource;
		}

		public int UnreadBufferCount => informationSource.BufferCount;

		public bool ProcessNextMessage()
		{
			string message = informationSource.GetNextMessage();

			if (message.Contains("hacking attempt"))
				return true;
			return false;
		}
	}

	public interface INetworkTrafficInformationSource
	{
		void AddNetworkMessage(string hello);
		int BufferCount { get;  }
		string GetNextMessage();
	}

	public interface ISensor
	{
		INetworkTrafficInformationSource InformationSource { get; set; }
	}
}
