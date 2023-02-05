using Editor;

namespace MelTycoon;

[Prefab]
[Library( "melon_spawner" ), HammerEntity]
[ClassName( "melon_spawner" )]
[Title( "Melon Spawner" ), Category( "Melon Tycoon" ), Icon( "cloud_circle" )]
public partial class MelonSpawner : Machine
{
	[Net]
	[Prefab]
	public MelonTier TierMelonToSpawn { get; set; } = MelonTier.Green;

	[Prefab]
	public float SpawnRate { get; set; } = 5;

	[Prefab]
	public Vector3 SpawnOffsetPosition { get; set; }

	[Prefab]
	public Vector3 MelonDropOffset { get; set; }

	TimeSince SinceSpawnedMelon { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Static );
	}

	protected override void Setup()
	{
		base.Setup();
		RenderColor = TierMelonToSpawn switch
		{
			MelonTier.Green => Color.Green,
			MelonTier.Red => Color.Red,
			MelonTier.Gold => Color.Yellow,
			_ => Color.White
		};
	}

	[Event.Tick.Server]
	private void OnTickServer()
	{
		if ( SinceSpawnedMelon > SpawnRate && PrefabLibrary.TrySpawn<Melon>( "prefabs/melons/melon.prefab", out var melon ) )
		{
			melon.Tier = TierMelonToSpawn;
			melon.Position = Position + MelonDropOffset;
			melon.MelonOwner = PlayerOwner;

			SinceSpawnedMelon = 0;
		}

		DebugOverlay.Sphere( Position + MelonDropOffset, 2f, Color.Green, depthTest: false );
	}
}
