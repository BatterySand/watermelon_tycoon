
namespace MelTycoon;

public partial class MelonDepot : AnimatedEntity
{
	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/sbox_props/aircon_unit_large/aircon_unit_large_64x64_a.vmdl_c" );
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

	[ConCmd.Server("spawn_depot")]
	public static void SpawnDepot()
	{
		var caller = ConsoleSystem.Caller;
		if ( caller.Pawn is not Player ply )
			return;

		var spawner = new MelonDepot();
		spawner.Position = ply.EyePosition + ply.EyeRotation.Forward * 100f;
	}
}
