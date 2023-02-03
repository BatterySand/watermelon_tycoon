using System;

namespace MelTycoon;

/// <summary>
/// The thing you build all your tycoon stuff on.
/// </summary>
[ClassName( "plate" )]
[Prefab]
public partial class Plate : ModelEntity
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

	private void OnClaimButtonPressed( Button btn, Player ply )
	{
		if ( !Game.IsServer )
			return;

		if ( Claimed )
			return;

		Log.Info( $"Player : {ply} claimed {this} plate." );
		PlateOwner = ply.Client;
		Claimed = true;

		if ( PrefabLibrary.TrySpawn<MelonSpawner>( "prefabs/melonspawners/green_melon_spawner.prefab", out var melSpawner ) )
		{
			melSpawner.Position = Position + melSpawner.SpawnOffsetPosition;
		}

		btn.Delete();
	}

	[Event.Tick.Server]
	private void TickServer()
	{
		DebugOverlay.Axis( Position, Rotation, depthTest: false );
	}
}
