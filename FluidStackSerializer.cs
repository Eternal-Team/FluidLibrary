using Terraria.ModLoader.IO;

namespace FluidLibrary
{
	public class FluidStackSerializer : TagSerializer<FluidStack, TagCompound>
	{
		public override TagCompound Serialize(FluidStack value) => new TagCompound
		{
			["Fluid"] = value.Fluid,
			["Volume"] = value.Volume,
			["MaxVolume"] = value.MaxVolume
		};

		public override FluidStack Deserialize(TagCompound tag)
		{
			BaseFluid fluid = tag.Get<BaseFluid>("Fluid");
			int volume = tag.GetInt("Volume");
			int maxVolume = tag.GetInt("MaxVolume");
			return new FluidStack(fluid, volume, maxVolume);
		}
	}
}