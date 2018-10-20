using Terraria.ModLoader;

namespace FluidLibrary.Content
{
	public abstract class ModFluid
	{
		public Mod Mod { get; internal set; }

		public string Name { get; internal set; }
		public ModTranslation DisplayName { get; internal set; }

		public virtual string Texture => (GetType().Namespace + "." + Name).Replace('.', '/');

		public int volume;

		public virtual ModFluid Clone() => (ModFluid)MemberwiseClone();

		public virtual void Initialize()
		{
		}

		public virtual ModFluid NewInstance()
		{
			var copy = (ModFluid)MemberwiseClone();
			return copy;
		}
	}
}