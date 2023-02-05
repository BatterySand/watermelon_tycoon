
using System.Linq;

namespace MelTycoon;

[Prefab]
public partial class Market : Machine
{
	[Net, Change( nameof( OnHasPalletChanged ) )]
	public bool HasPallet { get; set; }

	private ModelEntity _pallet { get; set; }

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
		_pallet.EnableAllCollisions = newValue;
	}


	[Event.Tick.Server]
	private void Tick()
	{
		_pallet.EnableDrawing = HasPallet;
	}

	public override void StartTouch( Entity other )
	{
		base.StartTouch( other );

		if ( other is not Melon melon )
			return;

		if ( melon.Components.Get<PlayerOwnerComponent>().Client.Pawn is not Player ply )
			return;

		ply.Currency += 1;
		melon.Delete();
	}
}
