
using Sandbox.UI;

namespace MelTycoon;

[Prefab]
[ClassName( "melon_packager" )]
public partial class MelonPackager : Machine
{
	[Net]
	[Prefab]
	public int ThresholdToPackage { get; set; }

	[Net]
	public int CurrentMelonCount { get; set; }

	[Net]
	[Prefab]
	public Vector3 TextPanelPosition { get; set; }

	public TextWorldPanel TextPanel { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Static );
		EnableTouch = true;
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		TextPanel = new TextWorldPanel();
		TextPanel.Position = Position;
	}

	[Event.Client.Frame]
	private void Frame()
	{
		// Billboard effect for the text, always face player's view.
		var ply = MelGameManager.LocalPlayer;

		TextPanel.Label = CurrentMelonCount.ToString();
		TextPanel.Style.FontSize = Length.Pixels( 512 );
		TextPanel.Position = Position + Rotation.Forward + TextPanelPosition;
		TextPanel.Rotation = Rotation;
	}

	[Event.Tick.Server]
	private void Tick()
	{
	}

	public override void StartTouch( Entity other )
	{
		if ( other is not Melon mel )
			return;

		if ( mel.Components.Get<PlayerOwnerComponent>().Client.Pawn is not Player ply )
			return;

		CurrentMelonCount++;

		if ( CurrentMelonCount >= ThresholdToPackage
			&& PrefabLibrary.TrySpawn<MelonCrate>( "prefabs/melons/melon_crate.prefab", out var crate ) )
		{
			crate.Position = Position + Vector3.Up * 50f;
			crate.ApplyAbsoluteImpulse( Vector3.Up * 200f );
			CurrentMelonCount = 0;
		}

		if ( !Game.IsServer )
			return;

		mel.Delete();
	}
}
