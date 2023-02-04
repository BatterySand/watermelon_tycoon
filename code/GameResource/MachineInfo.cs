
namespace MelTycoon;

[GameResource( "Machine Info", "tym", "Defines information about a machine." )]
public class MachineInfo : GameResource
{
	public int SpawnCost { get; set; }
	public Vector3 SpawnPosition { get; set; }
}
