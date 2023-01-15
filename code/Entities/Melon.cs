using Sandbox;

namespace MelTycoon;

public partial class Melon : ModelEntity
{
	public override void Spawn()
	{
		SetModel( "models/sbox_props/watermelon/watermelon.vmdl_c" );
		SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		Tags.Add( "interact" );
		EnableTouch = true;
	}

	public override void Touch( Entity other )
	{
		base.Touch( other );

		if ( !Game.IsServer )
			return;


	}
}

