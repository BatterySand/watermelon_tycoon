
using Sandbox;

namespace MelTycoon;

[ClassName( "melon_depot" )]
[Prefab]
public partial class MelonDepot : AnimatedEntity
{
	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Static );
		EnableTouch = true;
	}

	public override void StartTouch( Entity other )
	{
		if ( !Game.IsServer )
			return;

		if ( other is not Melon mel )
			return;

		if ( mel.MelonOwner.Pawn is not Player ply )
			return;

		switch ( mel.Tier )
		{
			case MelonTier.Green:
				ply.Currency += 10;
				break;
			case MelonTier.Red:
				ply.Currency += 20;
				break;
			case MelonTier.Gold:
				ply.Currency += 100;
				break;
			default:
				break;
		}
		mel.Delete();

	}
}
