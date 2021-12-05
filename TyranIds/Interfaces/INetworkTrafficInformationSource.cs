namespace TyranIds.Interfaces
{
	public interface INetworkTrafficInformationSource
	{
		void AddNetworkMessage(string hello);
		int BufferCount { get;  }
		string GetNextMessage();
	}
}