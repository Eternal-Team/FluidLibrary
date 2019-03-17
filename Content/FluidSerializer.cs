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
			string type = tag.GetString("Type");
			if (string.IsNullOrWhiteSpace(type)) return null;

			ModFluid fluid = FluidLoader.GetFluid(type).Clone();
			fluid.volume = tag.GetInt("Volume");

			return (T)fluid;
		}
	}
}