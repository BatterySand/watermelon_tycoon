namespace MelTycoon;

public partial class MelonSpawner : AnimatedEntity
{
	TimeSince SinceSpawnedMelon { get; set; }

	[Event.Tick.Server]
	private void OnTickServer()
	{
		if(SinceSpawnedMelon < 0)
		{
			
		}
	}
}
