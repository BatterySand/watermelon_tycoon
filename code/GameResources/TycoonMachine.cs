
namespace MelTycoon;

public abstract class TycoonMachine : GameResource
{
	public virtual string ClassName { get; set; }
	public virtual int SpawnCost { get; set; }
	public virtual Vector3 SpawnPosition { get; set; }
}
