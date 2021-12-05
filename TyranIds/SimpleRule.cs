using TyranIds.Common;

namespace TyranIds
{
	public class SimpleRule : IRule
	{
		private readonly string matchPattern;
		public SimpleRule(string pattern)
		{
			matchPattern = pattern;
		}
		public bool Match(string message)
		{
			return (message.Contains(matchPattern));
		}
	}
}