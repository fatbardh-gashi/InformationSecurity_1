namespace TyranIds.Common
{
	public interface IRule
	{
		bool Match(string message);
	}
}