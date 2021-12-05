namespace TyranIds
{
	public interface IRule
	{
		bool Match(string message);
	}
}