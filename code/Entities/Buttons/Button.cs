using System;
using Sandbox.UI;

namespace MelTycoon;

/// <summary>
/// Generic button.
/// </summary>
[Prefab]
[ClassName( "mel_button" )]
public partial class Button : AnimatedEntity, IUse, IPostSpawn
{
	[Net]
	[Prefab]
	public int Price { get; set; }

	[Net]
	[Prefab]
	public string EventToRun { get; set; }

	[Net]
	public bool Used { get; set; }

	public Action<Button, Player> OnPressed;

	public async void PostSpawn()
	{
		await GameTask.Delay( 1 );

		if ( !Components.TryGet<SpawnOffsetComponent>( out var so, true ) )
			return;

		Position += so.Position;
	}

	public override void Spawn()
	{
		SetupPhysicsFromAABB( PhysicsMotionType.Static, Vector3.One * -8, Vector3.One * 8 );
		PostSpawn();
		base.Spawn();
	}

	public override void ClientSpawn()
	{
	}

	public virtual bool Press( Player ply )
	{
		OnPressed?.Invoke( this, ply );
		return true;
	}

	[Event.Tick.Server]
	private void OnTickServer()
	{
		DebugOverlay.Box( this, Color.Yellow );
	}

	[Event.Client.Frame]
	private void OnFrame()
	{

	}

	public bool OnUse( Entity user )
	{
		if ( user is not Player ply || Used )
			return false;

		// TODO: move this
		if ( ply.Currency < Price )
			return false;
		ply.Currency -= Price;
		Used = true;
		Event.Run( EventToRun, ply );
		return Press( ply );
	}

	public bool IsUsable( Entity user )
	{
		return !Used && user is Player;
	}
}
