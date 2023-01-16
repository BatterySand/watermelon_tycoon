using Sandbox;

namespace MelTycoon;


public enum MelonTier
{
	Green,
	Red,
	Gold
}

public partial class Melon : ModelEntity
{
	[Net]
	public IClient MelonOwner { get; set; }

	[Net]
	public MelonTier Tier { get; set; }

	TimeSince SinceSpawned { get; set; }

	public override void Spawn()
	{
		SetModel( "models/sbox_props/watermelon/watermelon.vmdl_c" );
		SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		Tags.Add( "interact" );
		EnableTouch = true;
		SinceSpawned = 0;
	}

	[Event.Tick.Server]
	private void OnTickServer()
	{
		if ( SinceSpawned >= 20 )
			Delete();
	}

	public override void Touch( Entity other )
	{
		base.Touch( other );

		if ( !Game.IsServer )
			return;
	}
}

