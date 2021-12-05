using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PacketDotNet;
using TyranIds.Common;
using TyranIds.Data;

namespace TyranIds.Tests
{
	[TestClass]
	public class ReportTests
	{
		[TestMethod]
		public async Task TestTestReporter_ReportsSucessfully()
		{		
			//arrange
			IReportAgent reportAgent = NSubstitute.Substitute.For<IReportAgent>();
			var captureDevice = new TestCaptureDevice();
			SharpPcapInformationSource networkInformationSource = new SharpPcapInformationSource(captureDevice, 2000);
			SimpleRule simpleRule = new SimpleRule("FTP");
			Sensor sensor = new Sensor(networkInformationSource,simpleRule, reportAgent);

			//act
			sensor.Start();
			await TestPause();

			//assert
			reportAgent.ReceivedWithAnyArgs().ReportPacketCaptured(null);
		}



		[TestMethod]
		public async Task TestSimpleReporter_ReportsSucessfully()
		{		
			//arrange
			IDataAgent dataAgent = new EfDataAgent(); 
			DatabaseReporter reportAgent = new DatabaseReporter(dataAgent);
			var captureDevice = new TestCaptureDevice();
			SharpPcapInformationSource networkInformationSource = new SharpPcapInformationSource(captureDevice, 2000);
			SimpleRule simpleRule = new SimpleRule("FTP");
			Sensor sensor = new Sensor(networkInformationSource,simpleRule, reportAgent);

			using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				//act
				sensor.Start();
				await TestPause();	
				int numberOfAlerts = dataAgent.CountAlerts();

				//assert
				Assert.IsTrue(numberOfAlerts > 0);
			}
		}

		private async Task TestPause()
		{
			await Task.Delay(20000);
		}
	}

	public class DatabaseReporter : IReportAgent
	{
		private IDataAgent agent;
		public DatabaseReporter(IDataAgent dataAgent)
		{
			agent = dataAgent;
		}

		public void ReportPacketCaptured(NetworkEventArgs args)
		{
			agent.SaveAlert(args);
		}
	}
}
