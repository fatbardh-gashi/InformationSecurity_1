using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using TyranIds.Common;

namespace TyranIds
{
	public class Sensor : ISensor
	{
		private readonly int delay ;
		private readonly IInformationSource informationSource;
		private readonly IRule rule;
		private IReportAgent reportAgent;

		public Sensor(IInformationSource infoSource, IRule idsRule, IReportAgent reportAgent) : this(infoSource, idsRule)
		{
			this.reportAgent = reportAgent;
		}

		public Sensor(IInformationSource infoSource, IRule idsRule)
		{
			informationSource = infoSource;
			rule = idsRule;
			delay = 1000;
		}

		public int UnreadBufferCount => informationSource.BufferCount;

		public bool ProcessNextMessage()
		{
			NetworkEventArgs message = informationSource.GetNextMessage();

			if (rule.Match(message.PayloadText) && reportAgent != null)
				reportAgent.ReportPacketCaptured(message);

			return rule.Match(message.PayloadText);
		}

		public IInformationSource InformationSource { get { return informationSource; } set { throw new NotImplementedException(); } }

		public void Start()
		{
			IntialiseInformationSource(informationSource as dynamic);
				
			//Poll information source periodically to process the next message
			var listener = Task.Factory.StartNew(() =>{
				while (true)
				{
					ProcessNextMessage();
					Thread.Sleep(delay);
				}
			});
		}

		private void IntialiseInformationSource(IActiveInformationSource activeInformationSource)
		{
			 activeInformationSource.StartListening();
		}

		private void IntialiseInformationSource(IInformationSource informationSource)
		{
			
		}
	}
}