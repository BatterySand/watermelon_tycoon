using System;
using System.Linq;

namespace MelTycoon;

public partial class MelGameManager : Sandbox.GameManager
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

		// Get all of the spawnpoints
		var spawnpoints = Entity.All.OfType<SpawnPoint>();

		// chose a random one
		var randomSpawnPoint = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

		// if it exists, place the pawn there
		if ( randomSpawnPoint != null )
		{
			var tx = randomSpawnPoint.Transform;
			tx.Position = tx.Position + Vector3.Up * 50.0f; // raise it up
			pawn.Transform = tx;
		}
	}

	public override void PostLevelLoaded()
	{
		base.PostLevelLoaded();

		// Right now we have it hardcoded to a "facepunch.flatgrass" configuration.
		Log.Info( $"Loading map config: {Game.Server.MapIdent}" );
		if ( Game.Server.MapIdent != "facepunch.flatgrass" )
			return;

		var plate = new Plate
		{
			Position = new Vector3( 1055f, -154f, 0f ),
		};

		plate.CreateClaimButton();
	}
}
