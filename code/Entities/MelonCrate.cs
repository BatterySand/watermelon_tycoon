
namespace MelTycoon;

[Prefab]
public partial class MelonCrate : ModelEntity
{

	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
	}
}
