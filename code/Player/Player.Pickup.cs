
namespace MelTycoon;

partial class Player
{
	[Net] public TimeSince TimeSinceDroppedEntity { get; set; }
	[Net] public ModelEntity PickupEntity { get; set; }

	private PhysicsBody PickupEntityBody;
	private float holdDistance = 45;

	private void TickPickupRagdollOrProp()
	{
		// Currently holding something, move it where our eyes go.
		if ( PickupEntity.IsValid() && PickupEntityBody.IsValid() )
		{
			var velocity = PickupEntityBody.Velocity;
			Vector3.SmoothDamp( PickupEntityBody.Position, EyePosition + EyeRotation.Forward * holdDistance, ref velocity, 0.1f, Time.Delta * 0.9f );
			PickupEntityBody.AngularVelocity = Vector3.Zero;
			PickupEntityBody.Velocity = velocity.ClampLength( 400f );

			if ( PickupEntity.Position.Distance( Position ) > 300f )
			{
				PickupEntityBody = null;
				PickupEntity = null;
			}
		}

		if ( !Input.Pressed( InputButton.Use ) )
			return;

		var trace = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * (holdDistance * 2) )
			.EntitiesOnly()
			.WithoutTags( "stuck" )
			.Ignore( ActiveChild )
			.Ignore( this )
			.Radius( 2f )
			.Run();

		// Pick up the item.
		var entity = trace.Entity;
		if ( trace.Hit && entity is ModelEntity model && model.PhysicsEnabled )
		{
			if ( !PickupEntityBody.IsValid() && model.CollisionBounds.Size.Length < 128f && trace.Body.Mass < 100f )
			{
				PickupEntityBody = trace.Body;
				PickupEntity = model;
				PickupEntity.Tags.Add( "held" );
				return;
			}
		}

		// We're currently holding something and we just pressed the Use key
		// so lets drop the item.
		if ( PickupEntity.IsValid() && PickupEntityBody.IsValid() )
		{
			PickupEntityBody.ApplyImpulse( EyeRotation.Forward * PickupEntityBody.Mass );
			PickupEntity.Tags.Remove( "held" );
		}

		TimeSinceDroppedEntity = 0f;
		PickupEntityBody = null;
		PickupEntity = null;
	}
}
