namespace TyranIds.Common
{
	public interface IInformationSource
	{
		void AddNetworkMessage(NetworkEventArgs args);
		int BufferCount { get;  }
		NetworkEventArgs GetNextMessage();
	}

	public interface IActiveInformationSource : IInformationSource
	{
		void StartListening();
		void StopListening();
	}

}