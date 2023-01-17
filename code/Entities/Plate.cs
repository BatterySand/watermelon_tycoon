using System;

namespace MelTycoon;

/// <summary>
/// The thing you build all your tycoon stuff on.
/// </summary>
[ClassName( "plate" )]
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
		SetModel( "models/baseplate.vmdl" );
		SetupPhysicsFromModel( PhysicsMotionType.Static );
	}

	public void CreateClaimButton()
	{
		// Do this rough for now.
		// Spawn the button that a player can press to make this Plate theirs.
		var claimButton = new Button();
		claimButton.SetupPhysicsFromAABB( PhysicsMotionType.Static, Vector3.One * -8, Vector3.One * 8 );
		claimButton.TextLabel = "Claim Plate";
		claimButton.Position = Position + Vector3.Up * 25 + Vector3.Backward * 120f;
		claimButton.OnPressed += OnClaimButtonPressed;
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
	}
}
