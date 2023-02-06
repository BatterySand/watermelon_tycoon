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

	public const string BuyPallet = "buy.pallet";
	public partial class BuyPalletEventAttribute : Sandbox.EventAttribute
	{
		public BuyPalletEventAttribute() : base( BuyPallet )
		{
		}
	}
}
