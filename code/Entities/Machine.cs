using Editor;

namespace MelTycoon;

public abstract partial class Machine : AnimatedEntity, IPostSpawn
{
	public override void Spawn()
	{
		base.Spawn();
		PostSpawn();
	}
	private async void PostSpawn()
	{
		await GameTask.Delay( 1 );
		Setup();
	}

	protected virtual void Setup()
	{
		if ( !Components.TryGet<SpawnOffsetComponent>( out var so ) )
			return;

		Position += so.Position;
		Rotation = so.Rotation;
	}
}
