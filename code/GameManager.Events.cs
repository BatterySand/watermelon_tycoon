
using System.Linq;

namespace MelTycoon;

public partial class MelGameManager
{
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

	[Event( "buy.packager_pistol" )]
	private void BuyGreenPackager( Player ply )
	{
		if ( Game.IsClient )
			return;

		Log.Info( $"{ply} bought a Pistol packager" );
		ply.Plate.SpawnMachine<MelonPackager>( "prefabs/machines/melonpackager/melon_packager.prefab" );
		ply.Plate.AddButton( "prefabs/buttons/buy_blue_melon_spawner_button.prefab" );
	}

	[Event( "buy.packager_smg" )]
	private void BuyBluePackager( Player ply )
	{
		if ( Game.IsClient )
			return;

		Log.Info( $"{ply} bought a SMG packager" );
		ply.Plate.SpawnMachine<MelonPackager>( "prefabs/machines/melonpackager/smg_packager.prefab" );
		ply.Plate.AddButton( "prefabs/buttons/buy_red_melon_spawner_button.prefab" );
	}

	[Event( "buy.packager_shotgun" )]
	private void BuyRedPackager( Player ply )
	{
		if ( Game.IsClient )
			return;

		Log.Info( $"{ply} bought a Shotgun packager" );
		ply.Plate.SpawnMachine<MelonPackager>( "prefabs/machines/melonpackager/melon_packager_shotgun.prefab" );
	}

	[Event( "buy.spawner_smg" )]
	private void OnBuyBlueSpawner( Player ply )
	{
		if ( Game.IsClient )
			return;

		ply.Plate.SpawnMachine<MelonSpawner>( "prefabs/melonspawners/blue_melon_spawner.prefab" );
		ply.Plate.AddButton( "prefabs/buttons/buy_smg_packager_button.prefab" );
		Log.Info( $"{ply} bought an SMG Spawner" );
	}
}
