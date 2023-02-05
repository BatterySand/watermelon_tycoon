
namespace MelTycoon;

[Prefab]
public partial class CostComponent : EntityComponent
{
	[Net]
	[Prefab]
	public int Price { get; set; }
}