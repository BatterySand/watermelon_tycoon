
namespace MelTycoon;

[Prefab]
public partial class WorldPanelEntity : Entity
{
	[Net]
	[Prefab]
	public string Text { get; set; }

	[Net]
	[Prefab]
	public Vector3 Offset { get; set; }

	[Net]
	[Prefab]
	public Rotation RotOffset { get; set; }

	public TextWorldPanel Panel { get; set; }

	public override void Spawn()
	{
		Transmit = TransmitType.Always;
		base.Spawn();
	}

	public override void ClientSpawn()
	{
		Panel ??= new TextWorldPanel();
	}

	protected override void OnDestroy()
	{
		Panel?.Delete();
	}

	[Event.Client.Frame]
	private void Frame()
	{
		if ( !MelGameManager.LocalPlayer.IsValid() )
			return;

		Panel.Label = Text;

		var ply = MelGameManager.LocalPlayer;
		Panel.Position = Parent.Position + Offset;
		// Billboard effect for the text, always face player's view.
		Panel.Rotation = Rotation.LookAt( ply.EyeRotation.Backward, Vector3.Up );
	}

}
