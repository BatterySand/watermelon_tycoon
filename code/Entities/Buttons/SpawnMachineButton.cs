
namespace MelTycoon;

public partial class SpawnMachineButton : Button
{
	public string PrefabPath { get; set; }

	public override bool Press( Player ply )
	{
		if ( PrefabLibrary.TrySpawn<Entity>( "green_melon_spawner.prefab", out var spawnEntity ) )
		{
			spawnEntity.Position = Position;
		}

		return base.Press( ply );
	}
}
