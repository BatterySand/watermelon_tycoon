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
	public Prefab MelonPrefab { get; set; }

	[Prefab]
	public float SpawnRate { get; set; } = 5;

	[Prefab]
	public Vector3 MelonDropOffset { get; set; }

	TimeSince SinceLastMelonSpawned { get; set; }

	private PlayerOwnerComponent _ownerComp;

	public bool Upgraded { get; private set; }

	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Static );
		SinceLastMelonSpawned = 0;
	}

	[Event.Tick.Server]
	private void OnTickServer()
	{
		_ownerComp ??= Components.Get<PlayerOwnerComponent>();

		if ( SinceLastMelonSpawned > SpawnRate && PrefabLibrary.TrySpawn<Melon>( MelonPrefab.ResourcePath, out var melon ) )
		{
			melon.Position = Position + MelonDropOffset;

			var comp = melon.Components.GetOrCreate<PlayerOwnerComponent>();
			comp.Client = _ownerComp.Client;
			comp.Player = _ownerComp.Client.Pawn as Player;
			SinceLastMelonSpawned = 0;
		}
	}
}
