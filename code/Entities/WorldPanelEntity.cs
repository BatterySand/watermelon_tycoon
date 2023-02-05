
namespace MelTycoon;

[Prefab]
public partial class WorldPanelEntity : Entity
{
	[Net]
	[Prefab]
	public string Text { get; set; }

	public TextWorldPanel Panel { get; set; }

	public override void ClientSpawn()
	{
		Panel ??= new TextWorldPanel();
		Panel.Position = Position;
	}

	protected override void OnDestroy()
	{
		Panel?.Delete();
	}

	[Event.Client.Frame]
	private void Frame()
	{
		// Billboard effect for the text, always face player's view.
		if ( !MelGameManager.LocalPlayer.IsValid() )
			return;

		Panel.Label = Text;
		var ply = MelGameManager.LocalPlayer;
		Panel.Rotation = Rotation.LookAt( ply.EyeRotation.Backward, Vector3.Up );
	}

}