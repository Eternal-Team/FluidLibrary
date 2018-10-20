namespace FluidLibrary.Content
{
	public class Lava : ModFluid
	{
		public override string Texture => "FluidLibrary/Textures/Lava";

		public override void Initialize()
		{
			DisplayName.SetDefault("Lava");
		}
	}
}