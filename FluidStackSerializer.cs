using Terraria.ModLoader.IO;

namespace FluidLibrary;

public class FluidStackSerializer : TagSerializer<FluidStack, TagCompound>
{
	public override TagCompound Serialize(FluidStack value)
	{
		if (value.Fluid == null)
		{
			return new TagCompound
			{
				// ["MaxVolume"] = value.MaxVolume
			};
		}

		return new TagCompound
		{
			["Fluid"] = value.Fluid,
			["Volume"] = value.Volume,
			// ["MaxVolume"] = value.MaxVolume
		};
	}

	public override FluidStack Deserialize(TagCompound tag)
	{
		if (tag.ContainsKey("Fluid"))
		{
			BaseFluid fluid = tag.Get<BaseFluid>("Fluid");
			int volume = tag.GetInt("Volume");
			// int maxVolume = tag.GetInt("MaxVolume");
			return new FluidStack(fluid, volume/*, maxVolume*/);
		}

		// int maxVolume = tag.GetInt("MaxVolume");
		return new FluidStack(null, 0/*, maxVolume*/);
	}
}