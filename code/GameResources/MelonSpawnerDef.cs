
namespace MelTycoon;

[GameResource( "Melon Spawner Data", "tym", "Defines information about a melon spawner." )]
public class MelonSpawnerDef : TycoonMachine
{
	public override string ClassName => "melon_spawner";
	public MelonTier TierMelonToSpawn { get; set; } = MelonTier.Green;
	public float SpawnRate { get; set; } = 5;
}
