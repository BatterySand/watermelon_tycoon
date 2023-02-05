using Editor;

namespace MelTycoon;

public abstract partial class Machine : AnimatedEntity, IPostSpawn
{
	[Net]
	public IClient PlayerOwner { get; set; }

	[Prefab]
	[Net]
	public Rotation OverrideRotation { get; set; }

	[Prefab]
	public MachineInfo MachineInfo { get; set; }


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
		Log.Info( PlayerOwner );
		Log.Info( MachineInfo.SpawnPosition );
		Position += MachineInfo.SpawnPosition;
	}
}
