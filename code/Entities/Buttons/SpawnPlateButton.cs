
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

		var tr = Trace.Ray( Position, Vector3.Down )
		.WorldOnly()
		.Run();

		if ( !tr.Hit )
			return false;

		if ( !PrefabLibrary.TrySpawn<Plate>( "prefabs/plate.prefab", out var plate ) )
			return false;

		ply.Plate = plate;
		plate.PlateOwner = client;
		plate.Position = tr.HitPosition + Rotation.Backward * 228f;

		plate.Setup();
		Parent?.Delete();
		return base.Press( ply );
	}
}

