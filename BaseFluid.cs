using System.Text.RegularExpressions;
using Terraria.ModLoader;

namespace FluidLibrary
{
	public abstract class BaseFluid : ModTexturedType
	{
		public override string Texture => "BaseLibrary/Assets/Textures/PlaceholderTexture";

		public ModTranslation DisplayName { get; internal set; }
		public int Type { get; internal set; }

		#region Loading
		protected override void Register()
		{
			FluidLoader.RegisterFluid(this);

			ModTypeLookup<BaseFluid>.Register(this);

			DisplayName = Mod.CreateTranslation("FluidName." + Name);
		}

		public override void SetupContent()
		{
			SetStaticDefaults();

			if (DisplayName.IsDefault())
				DisplayName.SetDefault(Regex.Replace(Name, "([A-Z])", " $1").Trim());

			// SetDefaults();
		}

		public sealed override void Load()
		{
			base.Load();
		}

		public sealed override void Unload()
		{
			base.Unload();
		}
		#endregion

		public virtual BaseFluid Clone() => (BaseFluid)MemberwiseClone();

		public virtual void SetStaticDefaults()
		{
		}

		public override bool Equals(object obj)
		{
			if (obj is BaseFluid fluid)
			{
				return fluid.Type == Type;
			}

			return false;
		}
	}
}