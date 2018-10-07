using FluidLibrary.Content;
using Terraria.ModLoader;

namespace FluidLibrary
{
	public class FluidLibrary : Mod
	{
		public override void Load()
		{
			FluidLoader.Load();
		}

		public override void Unload()
		{
			FluidLoader.Unload();
		}
	}
}