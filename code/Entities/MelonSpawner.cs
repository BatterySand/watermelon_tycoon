namespace MelTycoon;

public partial class MelonSpawner : AnimatedEntity
{
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
		if(SinceSpawnedMelon > 5)
		{
			var melon = new Melon();
			melon.Spawn();
			melon.Position = Position + Vector3.Up * 24f;
			SinceSpawnedMelon = 0;
		}
	}

	[ConCmd.Server("spawn_melon spawner")]
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
