using FluidLibrary.Content;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FluidLibrary
{
	public class FluidLibrary : Mod
	{
		public override void Load()
		{
			FluidLoader.Load();
			TagSerializer.AddSerializer(new FluidSerializer<ModFluid>());
		}

		public override void Unload()
		{
			FluidLoader.Unload();
		}
	}
}