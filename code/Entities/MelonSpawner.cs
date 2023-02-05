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
	public Vector3 MelonDropOffset { get; set; }

	TimeSince SinceLastMelonSpawned { get; set; }
	private PlayerOwnerComponent _ownerComp;


	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Static );
		SinceLastMelonSpawned = 0;
	}

	protected override void Setup()
	{
		base.Setup();
		RenderColor = TierMelonToSpawn switch
		{
			MelonTier.Green => Color.Green,
			MelonTier.Blue => Color.Blue,
			MelonTier.Red => Color.Red,
			_ => Color.White
		};

		_ownerComp = Components.Get<PlayerOwnerComponent>();
	}

	[Event.Tick.Server]
	private void OnTickServer()
	{
		if ( SinceLastMelonSpawned > SpawnRate && PrefabLibrary.TrySpawn<Melon>( "prefabs/melons/melon.prefab", out var melon ) )
		{
			melon.Tier = TierMelonToSpawn;
			melon.Position = Position + MelonDropOffset;

			var comp = melon.Components.GetOrCreate<PlayerOwnerComponent>();
			comp.Client = _ownerComp.Client;
			SinceLastMelonSpawned = 0;
		}
	}
}
