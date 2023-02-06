
using System.Linq;

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

	[BuyEvents.BuyPalletEvent]
	private void OnBuyPallet( Player ply )
	{
		if ( Game.IsClient )
			return;

		Log.Info( $"{ply} bought a Pallet Upgrade" );
		var market = All.OfType<Market>().Where( x => x.Components.Get<PlayerOwnerComponent>().Player == ply ).First();
		market.HasPallet = true;
	}
}
