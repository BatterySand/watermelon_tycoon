using Sandbox;
using System;
using System.IO;
using System.Linq;

namespace MelTycoon;

public partial class MelGameManager : GameManager
{
	public static Player LocalPlayer => Game.LocalPawn as Player;

	public MelGameManager()
	{
		if ( Game.IsClient )
			_ = new Hud();
	}

	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		// Create a pawn for this client to play with
		var pawn = new Player();
		client.Pawn = pawn;

		pawn.Respawn();
		pawn.Position = Vector3.One * 200;
	}

	public override void PostLevelLoaded()
	{
		base.PostLevelLoaded();

		if ( !PrefabLibrary.TrySpawn<SpawnPlateButton>( "prefabs/buttons/spawn_plate_button.prefab", out var claimButton ) )
			return;

		claimButton.TextLabel = "Claim Plate";
		claimButton.Position = new Vector3( 1055f, -154f, 0f );
	}

}
