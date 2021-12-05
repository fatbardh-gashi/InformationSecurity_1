using System.Net.NetworkInformation;

namespace TyranIds.Common
{
	public interface IDataAgent
	{
		void SaveAlert(NetworkEventArgs args);
		//NetworkEventArgs ReadAlert();
		int CountAlerts();
	}
}