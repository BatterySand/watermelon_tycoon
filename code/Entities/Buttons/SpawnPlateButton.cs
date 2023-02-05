
using Sandbox;

namespace MelTycoon;

[Prefab]
public partial class SpawnPlateButton : Button
{
	[Prefab]
	public Vector3 PlateSpawnPosition { get; set; }

	public override bool Press( Player ply )
	{
		if ( !Game.IsServer )
			return false;

		var client = ply.Client;

		if ( !PrefabLibrary.TrySpawn<Plate>( "prefabs/plate.prefab", out var plate ) )
			return false;

		plate.PlateOwner = client;
		plate.Position = Position + Rotation.Backward * 228f;

		plate.Setup();
		Delete();
		return base.Press( ply );
	}
}

