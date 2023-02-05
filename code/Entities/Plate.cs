using System;
using static Sandbox.Event;

namespace MelTycoon;

/// <summary>
/// The thing you build all your tycoon stuff on.
/// </summary>
[ClassName( "plate" )]
[Prefab]
public partial class Plate : ModelEntity, IPostSpawn
{
	[Net]
	public IClient PlateOwner { get; set; }

	// May not be needed at a later date.
	[Net]
	public bool Claimed { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Static );
	
	}

	public void Setup()
	{
		SpawnMachine<MelonSpawner>( "prefabs/melonspawners/green_melon_spawner.prefab" );
		SpawnMachine<MelonPackager>( "prefabs/machines/melonpackager/melon_packager.prefab" );
	}

	public void SpawnMachine<T>( string prefabPath ) where T : Machine
	{
		if ( !PrefabLibrary.TrySpawn<T>( prefabPath, out var spawned ) )
			return;

		spawned.PlayerOwner = PlateOwner;
		spawned.Position = Position;
	}

	[Event.Tick.Server]
	private void TickServer()
	{
		DebugOverlay.Axis( Position, Rotation, depthTest: false );
	}
}
