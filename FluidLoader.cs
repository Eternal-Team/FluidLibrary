using System.Collections.Generic;
using Terraria.ModLoader;

namespace FluidLibrary;

public static class FluidLoader
{
	internal static Dictionary<int, BaseFluid> fluids = new();

	internal static int NextTypeID = 3;

	public static int Count => NextTypeID;

	internal static void RegisterFluid(BaseFluid fluid)
	{
		fluid.Type = fluid switch
		{
			Water => 0,
			Lava => 1,
			Honey => 2,
			_ => NextTypeID++
		};

		fluids.Add(fluid.Type, fluid);
	}

	public static int FluidType<T>() where T : BaseFluid => ModContent.GetInstance<T>()?.Type ?? -1;

	public static BaseFluid GetFluid(int type) => fluids[type];

	// public static T CreateInstance<T>() where T : BaseFluid
	// {
	// 	T instance = (T)ModContent.GetInstance<T>().Clone();
	// 	// instance.SetDefaults();
	// 	return instance;
	// }
}