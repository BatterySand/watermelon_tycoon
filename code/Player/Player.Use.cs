
namespace MelTycoon;

partial class Player
{
	/// <summary>
	/// Entity the player is currently using via their interaction key.
	/// </summary>
	public Entity Using { get; protected set; }
	private float _maxUseDistance = 512f;

	private void TickUse()
	{
		// This is serverside only
		if ( !Game.IsServer )
			return;

		if ( Input.Pressed( InputButton.Use ) )
		{
			Using = FindUsable();

			if ( Using == null )
			{
				UseFail();
				return;
			}

		}

		if ( Using is IUse use && use.OnUse( this ) )
			StopUsing();

		return;
	}

	/// <summary>
	/// Player tried to use something but there was nothing there.
	/// Tradition is to give a disappointed boop.
	/// </summary>
	private void UseFail()
	{
		PlaySound( "player_use_fail" );
	}

	/// <summary>
	/// If we're using an entity, stop using it
	/// </summary>
	private void StopUsing()
	{
		Using = null;
	}

	/// <summary>
	/// Returns if the entity is a valid usable entity
	/// </summary>
	protected bool IsValidUseEntity( Entity e )
	{
		return (e != null && e is IUse use && use.IsUsable( this ));
	}

	/// <summary>
	/// Find a usable entity for this player to use
	/// </summary>
	private Entity FindUsable()
	{
		// First try a direct 0 width line
		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * _maxUseDistance )
			.Ignore( this )
			.Run();

		// See if any of the parent entities are usable if we ain't.
		var ent = tr.Entity;
		while ( ent.IsValid() && !IsValidUseEntity( ent ) )
		{
			ent = ent.Parent;
		}

		// Nothing found, try a wider search
		if ( !IsValidUseEntity( ent ) )
		{
			tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * _maxUseDistance )
			.Radius( 2 )
			.Ignore( this )
			.Run();

			// See if any of the parent entities are usable if we ain't.
			ent = tr.Entity;
			while ( ent.IsValid() && !IsValidUseEntity( ent ) )
			{
				ent = ent.Parent;
			}
		}

		// Still no good? Bail.
		if ( !IsValidUseEntity( ent ) ) return null;

		return ent;
	}
}

