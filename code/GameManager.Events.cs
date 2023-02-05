
namespace MelTycoon;

public partial class MelGameManager
{
	[BuyEvents.BuyPackagerEvent]
	private void OnBuyPackager( Player ply )
	{
		if ( Game.IsClient )
			return;

		Log.Info( $"{ply} bought a packager" );
		ply.Plate.SpawnMachine<MelonPackager>( "prefabs/machines/melonpackager/melon_packager.prefab" );
	}
}
