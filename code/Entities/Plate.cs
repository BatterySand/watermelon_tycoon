using System.Collections.Generic;

namespace MelTycoon;

/// <summary>
/// The thing you build all your tycoon stuff on.
/// </summary>
[ClassName( "plate" )]
[Prefab]
public partial class Plate : ModelEntity, IPostSpawn
{
	[Net]
	[Prefab]
	public IList<Prefab> Prefabs { get; set; }

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
		foreach ( var p in Prefabs )
		{
			SpawnMachine<Entity>( p.ResourcePath );
		}
	}

	public void SpawnMachine<T>( string prefabPath ) where T : Entity
	{
		if ( !Game.IsServer )
			return;

		if ( !PrefabLibrary.TrySpawn<T>( prefabPath, out var spawned ) )
			return;

		var owner = spawned.Components.GetOrCreate<PlayerOwnerComponent>();
		owner.Client = PlateOwner;
		spawned.Position = Position;
	}

	[Event.Tick.Server]
	private void TickServer()
	{
		DebugOverlay.Axis( Position, Rotation, depthTest: false );
	}
}
