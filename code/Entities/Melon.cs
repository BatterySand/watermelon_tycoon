
namespace MelTycoon;

public enum MelonTier
{
	Green,
	Blue,
	Red
}

[Prefab]
public partial class Melon : ModelEntity
{
	[Net]
	[Prefab]
	public MelonTier Tier { get; set; }

	[Net]
	[Prefab]
	public float AutoDeleteTime { get; set; } = 10;

	TimeSince SinceSpawned { get; set; }

	public override void Spawn()
	{
		SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		EnableTouch = true;
		SinceSpawned = 0;
	}

	public override void Touch( Entity other )
	{
		base.Touch( other );

		if ( !Game.IsServer )
			return;
	}

	[Event.Tick.Server]
	private void OnTickServer()
	{
		// TODO: Do this only on first tick.
		RenderColor = Tier switch
		{
			MelonTier.Green => Color.White,
			MelonTier.Blue => Color.Blue,
			MelonTier.Red => Color.Red,
			_ => Color.White
		};

		if ( SinceSpawned >= AutoDeleteTime )
			Delete();
	}

	[ConCmd.Server( "spawn_melon" )]
	public static void SpawnMelon()
	{
		var caller = ConsoleSystem.Caller;
		if ( caller.Pawn is not Player ply )
			return;

		var melon = new Melon();
		melon.Position = ply.EyePosition + ply.EyeRotation.Forward * 100f;
	}
}

