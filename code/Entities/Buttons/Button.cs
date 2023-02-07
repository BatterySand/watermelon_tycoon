using System;

namespace MelTycoon;

/// <summary>
/// Generic button.
/// </summary>
[Prefab]
[ClassName( "mel_button" )]
public partial class Button : AnimatedEntity, IUse
{
	[Net]
	[Prefab]
	public int Price { get; set; }

	[Net]
	[Prefab]
	public int NumStages { get; set; } = 0;

	[Net]
	[Prefab]
	public string EventToRun { get; set; }

	[Net]
	public bool Used { get; set; }

	[Net]
	public int NumPresses { get; set; }
	public TimeSince LastPressed { get; private set; } = 0;

	public Action<Button, Player> OnPressed;

	public override void Spawn()
	{
		SetupPhysicsFromAABB( PhysicsMotionType.Static, Vector3.One * -8, Vector3.One * 8 );
		base.Spawn();
	}

	public override void ClientSpawn()
	{
	}

	public virtual bool Press( Player ply )
	{
		if ( ply.Currency < Price )
			return false;

		ply.Currency -= Price;

		Log.Info( NumPresses );
		OnPressed?.Invoke( this, ply );
		Event.Run( EventToRun, ply );

		if ( Used )
		{
			Delete();
			foreach ( var c in Children )
				c.Delete();
		}

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

		LastPressed = 0;
		NumPresses++;
		if ( NumPresses >= NumStages )
			Used = true;

		return Press( ply );
	}

	public bool IsUsable( Entity user )
	{
		return !Used && user is Player && LastPressed > 0.5f;
	}
}
