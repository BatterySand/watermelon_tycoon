
namespace MelTycoon;

public partial class SpawnMachineButton : ModelEntity
{
	public string MachinePath { get; set; }

	public void OnPress()
	{
		var info = ResourceLibrary.Get<TycoonMachine>( MachinePath );
		var spawner = CreateByName( info.ClassName );

		if ( spawner is ISetupFromResource machine )
		{
			machine.Setup( info );
		}
	}
}
