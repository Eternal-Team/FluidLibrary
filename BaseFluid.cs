using Terraria.ModLoader;

namespace FluidLibrary;

public abstract class BaseFluid : ModTexturedType
{
	public int Type { get; internal set; }

	public ModTranslation DisplayName { get; internal set; }

	protected override void Register()
	{
		ModTypeLookup<BaseFluid>.Register(this);

		DisplayName = LocalizationLoader.GetOrCreateTranslation(Mod, $"FluidName.{Name}");

		FluidLoader.RegisterFluid(this);
	}

	public override void SetupContent()
	{
		SetStaticDefaults();
	}

	public sealed override void Load()
	{
		base.Load();
	}

	public sealed override void Unload()
	{
		base.Unload();
	}

	public virtual BaseFluid Clone() => (BaseFluid)MemberwiseClone();

	public virtual bool IsTheSameAs(BaseFluid fluid)
	{
		return fluid.Type == Type;
	}
}