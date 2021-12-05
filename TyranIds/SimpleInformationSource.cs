using System.Collections.Generic;
using System.Linq;
using TyranIds.Common;

namespace TyranIds
{
	public class SimpleInformationSource : IInformationSource
	{
		private readonly List<NetworkEventArgs> messageBuffer;
		public void AddNetworkMessage(NetworkEventArgs args)
		{
			messageBuffer.Add(args);
		}

		public int BufferCount => messageBuffer.Count;
		public NetworkEventArgs GetNextMessage()
		{
			if (messageBuffer.Any())
			{
				NetworkEventArgs nextMesage = messageBuffer.First();
				messageBuffer.RemoveAt(0);
				return nextMesage;
			} else return null;
		}

		public SimpleInformationSource()
		{
			messageBuffer = new List<NetworkEventArgs>();
		}
		
	}
}