using Terraria.ModLoader;

namespace FluidLibrary;

public class FluidLibrary : Mod
{
	public override void Load()
	{
		FluidSerializer.Load();
	}
}