
using System.Linq;

namespace MelTycoon;

[Prefab]
public partial class Market : Machine
{
	[Net, Change( nameof( OnHasPalletChanged ) )]
	public bool HasPallet { get; set; }

	private ModelEntity _pallet { get; set; }

	RealTimeSince LastPalletScanTime { get; set; } = 0;

	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Static );
		EnableTouch = true;
		_pallet = (ModelEntity)Children.First(); // TODO: maybe fix this?
		_pallet.SetupPhysicsFromModel( PhysicsMotionType.Static );
		_pallet.EnableAllCollisions = false;
		_pallet.EnableDrawing = false;
	}

	public override void ClientSpawn()
	{
		_pallet = (ModelEntity)Children.First(); // TODO: maybe fix this?
	}

	public void OnHasPalletChanged( bool oldValue, bool newValue )
	{

		_pallet.EnableDrawing = newValue;
	}

	[Event.Tick.Server]
	private void Tick()
	{
		_pallet.EnableDrawing = HasPallet;

		if ( !HasPallet )
			return;

		if ( LastPalletScanTime > 10 )
		{
			var box = new BBox( Position + Rotation.Left * 100, 125 );
			DebugOverlay.Box( box, Color.Random, 2 );
			var tr = Entity.FindInBox( box );
			foreach ( var e in tr )
			{
				if ( e is not MelonCrate crate )
					continue;

				if ( Components.TryGet<PlayerOwnerComponent>( out var owner ) )
					owner.Player.Currency += 10;

				crate.Delete();
			}

			LastPalletScanTime = 0;
		}
	}

	public override void StartTouch( Entity other )
	{
		base.StartTouch( other );

		if ( other is not Melon melon )
			return;

		if ( !melon.Components.TryGet<PlayerOwnerComponent>( out var owner ) )
			return;

		owner.Player.Currency += 1;
		melon.Delete();
	}
}
