using System;
using System.Linq;
using TyranIds.Common;

namespace TyranIds.Data
{
	public class EfDataAgent : IDataAgent
	{
		public void SaveAlert(NetworkEventArgs args)
		{
			using (AlertsEntities entities = new AlertsEntities())
			{
				entities.Alerts.Add(new Alerts()
				{
					Created = DateTime.Now,
					DatasourceId = args.DatasourceId,
					DestinationPort = args.DestinationPort,
					SourcePort = args.SourcePort,
					DestinationIp = args.DestinationIpAddress,
					SourceIp = args.SourceIpAddress,
					Payload = args.PayloadText
				});
				entities.SaveChanges();
			}
		}

		public int CountAlerts()
		{
			using (AlertsEntities entities = new AlertsEntities())
			{
				return entities.Alerts.Count();
			}
		}
	}
}