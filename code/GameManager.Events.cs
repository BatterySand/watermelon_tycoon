
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
		ply.Plate.AddButton( "prefabs/buttons/buy_blue_melon_spawner_button.prefab" );
	}

	[BuyEvents.BuyPalletEvent]
	private void OnBuyPallet( Player ply )
	{
		if ( Game.IsClient )
			return;

		var plate = ply.Plate;
		var market = plate.Children.OfType<Market>().Where( x => x.Components.Get<PlayerOwnerComponent>().Player == ply ).First();
		market.HasPallet = true;

		plate.AddButton( "prefabs/buttons/buy_packager_button.prefab" );
		Log.Info( $"{ply} bought a Pallet Upgrade" );
	}

	[Event( "buy.blue_spawner" )]
	private void OnBuyBlueSpawner(Player ply)
	{
		if ( Game.IsClient )
			return;

		ply.Plate.SpawnMachine<MelonSpawner>( "prefabs/melonspawners/blue_melon_spawner.prefab" );
		Log.Info( $"{ply} bought an SMG Spawner" );
	}
}
