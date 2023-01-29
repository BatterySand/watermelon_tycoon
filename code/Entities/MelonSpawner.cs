using Editor;
using System.Runtime.InteropServices;

namespace MelTycoon;

[Library( "melon_spawner" ), HammerEntity]
[ClassName( "melon_spawner" )]
[Title( "Melon Spawner" ), Category( "Melon Tycoon" ), Icon( "cloud_circle" )]
public partial class MelonSpawner : AnimatedEntity, ISetupFromResource
{
	[Net]
	public MelonTier TierMelonToSpawn { get; set; } = MelonTier.Green;
	public float SpawnRate { get; set; } = 5;
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
			melon.Position = Position + Vector3.Down * 24f;
			SinceSpawnedMelon = 0;
		}
	}

	public void Setup( TycoonMachine def )
	{
		if ( def is not MelonSpawnerDef info )
			return;

		Position = Position + info.SpawnPosition;
		TierMelonToSpawn = info.TierMelonToSpawn;
		SpawnRate = info.SpawnRate;
	}
}
