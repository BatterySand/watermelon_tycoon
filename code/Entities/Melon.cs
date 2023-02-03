
namespace MelTycoon;

public enum MelonTier
{
	Green,
	Red,
	Gold
}

[Prefab]
public partial class Melon : ModelEntity
{
	[Net]
	public IClient MelonOwner { get; set; }

	[Net]
	[Prefab]
	public MelonTier Tier { get; set; }

	TimeSince SinceSpawned { get; set; }

	public override void Spawn()
	{
		SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		EnableTouch = true;
		SinceSpawned = 0;
	}

	[Event.Tick.Server]
	private void OnTickServer()
	{
		// TODO: Do this only on first tick.
		RenderColor = Tier switch
		{
			MelonTier.Green => Color.Green,
			MelonTier.Red => Color.Red,
			MelonTier.Gold => Color.Yellow,
			_ => Color.White
		};

		if ( SinceSpawned >= 20 )
			Delete();
	}

	public override void Touch( Entity other )
	{
		base.Touch( other );

		if ( !Game.IsServer )
			return;
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

