using System;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TyranIds.Data.Tests
{
	[TestClass]
	public class SqlDataAgentTests
	{
		[TestMethod]
		public void TestSaveAndReadAnAlert_SuccessfullyPersistedAndRead()
		{							
			//arrange
			Data.Alerts anAlert = new Alerts() { 
				Captured = null,
				Created = DateTime.Now,
				DatasourceId = Guid.Parse("E05EF9A1-1268-47AE-BC07-3ADD45B6B975"),
				DestinationIp = "127.0.0.1",
				SourceIp = "192.168.1.13",
				SourcePort = 128,
				DestinationPort = 134,
				Payload = "hello world"
			};

			//act

			using (TransactionScope transaction = new TransactionScope())
			{
				using (Data.AlertsEntities entities = new AlertsEntities()) {
				entities.Alerts.Add(anAlert);
				entities.SaveChanges();

					Guid newAlertGuid = Guid.Parse("E05EF9A1-1268-47AE-BC07-3ADD45B6B975");
				Data.Alerts readAlert =
					entities.Alerts.Single(a => a.DatasourceId == newAlertGuid);

				//assert
				Assert.AreEqual(anAlert.DestinationPort, readAlert.DestinationPort);
				Assert.AreEqual(anAlert.Payload, readAlert.Payload);
			}
			}
		}
	}
}


