using Terraria.ModLoader.IO;

namespace FluidLibrary.Content
{
	public class FluidSerializer<T> : TagSerializer<T, TagCompound> where T : ModFluid
	{
		public override TagCompound Serialize(T value) => new TagCompound
		{
			["Type"] = value.Name,
			["Volume"] = value.volume
		};

		public override T Deserialize(TagCompound tag)
		{
			ModFluid fluid = FluidLoader.GetFluid(tag.GetString("Type")).Clone();
			fluid.volume = tag.GetInt("Volume");

			return (T)fluid;
		}
	}
}