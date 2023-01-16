namespace MelTycoon;

public partial class MelonSpawner : AnimatedEntity
{
	[Net]
	public MelonTier TierMelonToSpawn { get; set; }
	TimeSince SinceSpawnedMelon { get; set; }
	TimeSince SinceSpawnedMelonRed { get; set; }
	TimeSince SinceSpawnedMelonGold { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/props/ceiling_fixtures/ceiling_aircon_a.vmdl_c" );
		SetupPhysicsFromModel( PhysicsMotionType.Static );
	}

	

	[Event.Tick.Server]
	private void OnTickServer()
	{
		if(SinceSpawnedMelon > 5)
		{
			var melon = new Melon();
			melon.Tier = MelonTier.Green;
			melon.Spawn();
			melon.Position = Position + Vector3.Down * 24f;
			SinceSpawnedMelon = 0;
		}
		if( SinceSpawnedMelonRed  > 10)
		{
			var melon = new Melon();
			melon.Tier = MelonTier.Red;
			melon.Spawn();
			melon.RenderColor = Color.Red;
			melon.Position = Position + Vector3.Down * 24f;
			SinceSpawnedMelonRed = 0;
		}

		if(SinceSpawnedMelonGold > 20 )
		{
			var melon = new Melon();
			melon.Tier = MelonTier.Gold;
			melon.Spawn();
			melon.RenderColor = Color.Blue;
			melon.Position = Position + Vector3.Down * 24f;
			SinceSpawnedMelonGold = 0;
		}
	}

	[ConCmd.Server("spawn_melon_spawner")]
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
