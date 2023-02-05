
namespace MelTycoon;

[Prefab]
public partial class PlayerOwnerComponent : EntityComponent
{

	[Net]
	public IClient Client { get; set; }
}