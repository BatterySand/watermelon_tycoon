using Editor;
using Sandbox.UI;

namespace MelTycoon;

[Library( "melon_spawner" ), HammerEntity]
[ClassName( "melon_spawner" )]
[Title( "Melon Spawner" ), Category( "Melon Tycoon" ), Icon( "cloud_circle" )]
public partial class MelonSpawner : AnimatedEntity
{
	[Net]
	[Property]
	public MelonTier TierMelonToSpawn { get; set; } = MelonTier.Green;

	[Net]
	[Property]
	public float SpawnRate { get; set; } = 5;

	[Net]
	TimeSince SinceSpawnedMelon { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/props/ceiling_fixtures/ceiling_aircon_a.vmdl_c" );
		SetupPhysicsFromModel( PhysicsMotionType.Static );
	}

	[Event.Tick.Server]
	private void OnTickServer()
	{
		if ( SinceSpawnedMelon > SpawnRate )
		{
			var melon = new Melon();
			melon.Tier = TierMelonToSpawn;
			melon.Spawn();
			melon.Position = Position + Vector3.Down * 24f;
			SinceSpawnedMelon = 0;
		}
	}

	[ConCmd.Server( "spawn_melon_spawner" )]
	public static void SpawnMelonSpawner()
	{
		var caller = ConsoleSystem.Caller;
		if ( caller.Pawn is not Player ply )
			return;

		var spawner = new MelonSpawner();
		spawner.Spawn();
		spawner.Position = ply.EyePosition + ply.EyeRotation.Forward * 100f;
	}
}
