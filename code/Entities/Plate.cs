using System.Collections.Generic;

namespace MelTycoon;

/// <summary>
/// The thing you build all your tycoon stuff on.
/// </summary>
[ClassName( "plate" )]
[Prefab]
public partial class Plate : ModelEntity
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
			SpawnMachine<Entity>( p.ResourcePath );
	}

	public void SpawnMachine<T>( string prefabPath ) where T : Entity
	{
		if ( !Game.IsServer )
			return;

		if ( !PrefabLibrary.TrySpawn<T>( prefabPath, out var machine ) )
			return;

		var owner = machine.Components.GetOrCreate<PlayerOwnerComponent>();
		owner.Client = PlateOwner;
		owner.Player = PlateOwner.Pawn as Player;

		machine.Position = Position;

		if ( !machine.Components.TryGet<SpawnOffsetComponent>( out var spawnOffset ) )
			return;

		var tx = new Transform( spawnOffset.OffsetPosition, spawnOffset.OffsetRotation );
		machine.SetParent( this, null, tx );
	}

	public Button AddButton( string path )
	{
		if ( !Game.IsServer )
			return null;

		if ( !PrefabLibrary.TrySpawn<Button>( path, out var btn ) )
			return null;

		var owner = btn.Components.GetOrCreate<PlayerOwnerComponent>();
		owner.Client = PlateOwner;
		owner.Player = PlateOwner.Pawn as Player;

		if ( !btn.Components.TryGet<SpawnOffsetComponent>( out var spawnOffset ) )
			return null;

		var tx = new Transform( spawnOffset.OffsetPosition, spawnOffset.OffsetRotation );

		btn.SetParent( this, null, tx );

		return btn;
	}

	[Event.Tick.Server]
	private void TickServer()
	{
		DebugOverlay.Axis( Position, Rotation, depthTest: false );
	}

	[ConCmd.Admin( "mel_plate_spawn_prefab" )]
	public static void SpawnPrefabCommand( string path )
	{
		Log.Info( path );
		if ( ConsoleSystem.Caller.Pawn is not Player ply )
			return;

		if ( !ply.Plate.IsValid() )
			return;

		var tr = Trace.Ray( ply.EyePosition, ply.EyePosition + ply.EyeRotation.Forward * 1000f )
			.WithoutTags( "player" )
			.Run();

		DebugOverlay.TraceResult( tr, 10 );
		if ( !tr.Hit || tr.Entity != ply.Plate )
			return;

		if ( !PrefabLibrary.TrySpawn<Entity>( path, out var created ) )
			return;

		if ( created.Components.TryGet<SpawnOffsetComponent>( out var spawnOffset ) )
		{
			created.SetParent( ply.Plate, null, new Transform( spawnOffset.OffsetPosition, spawnOffset.OffsetRotation ) );
		}
		else
		{
			created.SetParent( ply.Plate, null, new Transform( tr.HitPosition, Rotation.Identity ) );
		}
	}
}
