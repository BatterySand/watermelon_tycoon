namespace MelTycoon;

public static class BuyEvents
{
	public const string BuyPackager = "buy.packager";
	public partial class BuyPackagerEventAttribute : Sandbox.EventAttribute
	{
		public BuyPackagerEventAttribute() : base( BuyPackager )
		{
		}
	}
}