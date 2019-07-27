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

		public static int GetFluidIDByName(string name)
		{
			switch (name)
			{
				case "Water":
					return 0;
				case "Lava":
					return 1;
				case "Honey":
					return 2;
			}

			return -1;
		}

		public static string GetFluidNameByID(int type)
		{
			switch (type)
			{
				case 0:
					return "Water";
				case 1:
					return "Lava";
				case 2:
					return "Honey";
			}

			return null;
		}
	}
}