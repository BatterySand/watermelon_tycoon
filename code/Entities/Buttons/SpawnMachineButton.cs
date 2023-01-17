
namespace MelTycoon;

public partial class SpawnMachineButton : Button
{
	public string MachinePath { get; set; }

	public override bool Press( Player ply )
	{
		var info = ResourceLibrary.Get<TycoonMachine>( MachinePath );
		var spawner = CreateByName( info.ClassName );

		if ( spawner is ISetupFromResource machine )
		{
			machine.Setup( info );
			return true;
		}

		return base.Press( ply );
	}
}
