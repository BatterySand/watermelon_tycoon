
namespace MelTycoon;

[Prefab]
public partial class SpawnOffsetComponent : EntityComponent
{
	[Net]
	[Prefab]
	public Vector3 OffsetPosition { get; set; }

	[Net]
	[Prefab]
	public Rotation OffsetRotation { get; set; }

}
