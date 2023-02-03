using System;
using System.Linq;

namespace MelTycoon;

public partial class MelGameManager : Sandbox.GameManager
{
	public static Player LocalPlayer => Game.LocalPawn as Player;

	private static Vector3 DebugSpawnPlayerPosition {get; set;}

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

		pawn.Position = DebugSpawnPlayerPosition;

	}

	public override void PostLevelLoaded()
	{
		base.PostLevelLoaded();

		// Right now we have it hardcoded to a "facepunch.flatgrass" configuration.
		Log.Info( $"Loading map config: {Game.Server.MapIdent}" );
		if ( Game.Server.MapIdent != "facepunch.flatgrass" )
			return;

		if ( PrefabLibrary.TrySpawn<Plate>( "prefabs/plate.prefab", out var plate ) )
		{
			plate.Position = new Vector3( 1055f, -154f, 0f );
			DebugSpawnPlayerPosition = plate.Position + Vector3.Up * 25f;
		}

	}
}
