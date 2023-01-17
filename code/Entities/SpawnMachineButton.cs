using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelTycoon;

public partial class SpawnMachineButton : ModelEntity
{
	public string MachinePath { get; set; }

	public void OnPress()
	{
		var info = ResourceLibrary.Get<TycoonMachine>( MachinePath );

		var spawner = CreateByName( info.ClassName );
		if ( spawner is ISetupFromAsset machine )
		{
			machine.Setup( info );
		}
	}
}
