
namespace MelTycoon;

[Prefab]
public partial class SpawnOffsetComponent : EntityComponent
{
	[Net]
	[Prefab]
	public Vector3 Position { get; set; }

	[Net]
	[Prefab]
	public Rotation Rotation { get; set; }
}


