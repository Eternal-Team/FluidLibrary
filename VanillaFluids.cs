namespace FluidLibrary
{
	public class Water : BaseFluid
	{
		public override string Texture => "FluidLibrary/Textures/Water";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water");
		}
	}

	public class Lava : BaseFluid
	{
		public override string Texture => "FluidLibrary/Textures/Lava";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava");
		}
	}

	public class Honey : BaseFluid
	{
		public override string Texture => "FluidLibrary/Textures/Honey";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Honey");
		}
	}
}