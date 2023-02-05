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

		var tr = Trace.Ray( Vector3.Up * 128f, Vector3.Down ).WorldOnly().Run();

		if ( !PrefabLibrary.TrySpawn<Entity>( "prefabs/buttons/spawn_plate_button.prefab", out var claimButton ) )
			return;

		claimButton.Position = tr.Hit ? tr.HitPosition : Vector3.Up * 16f;
	}


	[ConCmd.Client( "get_offset" )]
	public static void GetOffset()
	{
		if ( ConsoleSystem.Caller.Pawn is not Player ply )
			return;

		var tr = Trace.Ray( ply.EyePosition, ply.EyePosition + ply.EyeRotation.Forward * 1000 )
			.EntitiesOnly()
			.WithTag( "solid" )
			.Run();

		if ( tr.Entity is not Plate plate )
			return;

		var offset = tr.HitPosition - plate.Position;
		var offsetRounded = new Vector3( offset.x.Floor(), offset.y.Floor(), offset.z.Floor() );

		Sandbox.UI.Clipboard.SetText( offsetRounded.ToString() );
		Log.Info( offsetRounded.ToString() );
		DebugOverlay.Sphere( tr.HitPosition, 2f, Color.Blue, 10 );
		DebugOverlay.Text( offsetRounded.ToString(), tr.HitPosition, 10 );
	}
}
