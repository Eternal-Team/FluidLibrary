using System.Collections.Generic;
using Terraria.ModLoader;

namespace FluidLibrary
{
	public static class FluidLoader
	{
		internal static List<BaseFluid> fluids = new List<BaseFluid>();

		internal static int NextTypeID = 3;

		public static int Count => NextTypeID;

		internal static void RegisterFluid(BaseFluid fluid)
		{
			switch (fluid)
			{
				case Water _:
					fluid.Type = 0;
					break;
				case Lava _:
					fluid.Type = 1;
					break;
				case Honey _:
					fluid.Type = 2;
					break;
				default:
					fluid.Type = NextTypeID++;
					break;
			}

			fluids.Add(fluid);
		}

		public static int FluidType<T>() where T : BaseFluid => ModContent.GetInstance<T>()?.Type ?? -1;

		public static BaseFluid GetFluid(int type) => fluids[type];
		
		public static T CreateInstance<T>() where T : BaseFluid
		{
			T instance = (T)ModContent.GetInstance<T>().Clone();
			// instance.SetDefaults();
			return instance;
		}
	}
}