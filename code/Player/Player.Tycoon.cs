
namespace MelTycoon;

partial class Player
{
	[Net]
	public float Currency { get; set; }

	[Net]
	public Plate Plate { get; set; }


	[ConCmd.Admin( "mel_give_money" )]
	public static void GiveMoney( int amount )
	{
		if ( ConsoleSystem.Caller.Pawn is not Player ply )
			return;

		ply.Currency += amount;
	}
}

