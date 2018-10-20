namespace FluidLibrary.Content
{
	public class Honey : ModFluid
	{
		public override string Texture => "FluidLibrary/Textures/Honey";

		public override void Initialize()
		{
			DisplayName.SetDefault("Honey");
		}
	}
}