using System.Collections.Generic;
using System.Linq;
using TyranIds.Interfaces;

namespace TyranIds.Tests
{
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
}