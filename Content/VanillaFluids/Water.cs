namespace FluidLibrary.Content
{
	public class Water : ModFluid
	{
		public override string Texture => "FluidLibrary/Textures/Water";

		public override void Initialize()
		{
			DisplayName.SetDefault("Water");
		}
	}
}