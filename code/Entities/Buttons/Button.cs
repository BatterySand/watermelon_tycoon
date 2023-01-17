using System;
using Sandbox.UI;

namespace MelTycoon;

/// <summary>
/// Generic button.
/// </summary>
public partial class Button : AnimatedEntity, IUse
{
	[Net]
	public string TextLabel { get; set; }

	public ButtonText Text { get; set; }

	public Action<Button, Player> OnPressed;

	public override void ClientSpawn()
	{
		Text = new ButtonText();
		Text.Position = Position;
		Text.Label = TextLabel;
	}

	public virtual bool Press( Player ply )
	{
		OnPressed?.Invoke( this, ply );
		return false;
	}

	[Event.Tick.Server]
	private void OnTickServer()
	{
		DebugOverlay.Box( this, Color.Yellow );
	}

	[Event.Client.Frame]
	private void OnFrame()
	{
		// Billboard effect for the text, always face player's view.
		var ply = MelGameManager.LocalPlayer;
		Text.Rotation = Rotation.LookAt( ply.EyeRotation.Backward, Vector3.Up );
	}

	public bool OnUse( Entity user )
	{
		if ( user is not Player ply )
			return false;

		return Press( ply );
	}

	public bool IsUsable( Entity user )
	{
		return user is Player;
	}
}
