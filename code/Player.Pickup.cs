
namespace MelTycoon;

partial class Player
{
	[Net] public TimeSince TimeSinceDroppedEntity { get; set; }
	[Net] public ModelEntity PickupEntity { get; set; }

	private PhysicsBody PickupEntityBody;
	private float holdDistance = 45;

	private void TickPickupRagdollOrProp()
	{
		if ( PickupEntity.IsValid() && PickupEntity.Position.Distance( Position ) > 300f )
		{
			PickupEntityBody = null;
			PickupEntity = null;
		}

		var trace = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * (holdDistance * 2) )
			.EntitiesOnly()
			.WithoutTags( "stuck" )
			.Ignore( ActiveChild )
			.Ignore( this )
			.Radius( 2f )
			.Run();

		if ( PickupEntityBody.IsValid() )
		{
			var velocity = PickupEntityBody.Velocity;
			Vector3.SmoothDamp( PickupEntityBody.Position, EyePosition + EyeRotation.Forward * holdDistance, ref velocity, 0.1f, Time.Delta * 0.9f );
			PickupEntityBody.AngularVelocity = Vector3.Zero;
			PickupEntityBody.Velocity = velocity.ClampLength( 400f );
		}

		if ( !Input.Pressed( InputButton.Use ) )
			return;

		var entity = trace.Entity;

		if ( trace.Hit && entity is ModelEntity model && model.PhysicsEnabled )
		{
			if ( !PickupEntityBody.IsValid() && model.CollisionBounds.Size.Length < 128f )
			{
				if ( trace.Body.Mass < 100f )
				{
					PickupEntityBody = trace.Body;
					PickupEntity = model;
					PickupEntity.Tags.Add( "held" );
					return;
				}
			}
		}

		if ( PickupEntityBody.IsValid() )
		{
			trace = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 80f )
				.WorldOnly()
				.Ignore( ActiveChild )
				.Ignore( this )
				.Radius( 2f )
				.Run();

			if ( PickupEntityBody.IsValid() )
			{
				if ( PickupEntity.IsValid() )
				{
					PickupEntityBody.ApplyImpulse( EyeRotation.Forward * PickupEntityBody.Mass );
					PickupEntity.Tags.Remove( "held" );
				}
			}

			TimeSinceDroppedEntity = 0f;
			PickupEntityBody = null;
			PickupEntity = null;
		}
	}
}
