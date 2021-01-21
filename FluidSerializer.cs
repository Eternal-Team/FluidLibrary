using System;
using System.Reflection;
using MonoMod.RuntimeDetour.HookGen;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FluidLibrary
{
	[Autoload(false)]
	public class FluidSerializer : TagSerializer<BaseFluid, TagCompound>
	{
		private static FluidSerializer Instance = new FluidSerializer();

		private delegate bool orig_TryGetSerializer(Type type, out TagSerializer serializer);

		private delegate bool hook_TryGetSerializer(orig_TryGetSerializer orig, Type type, out TagSerializer serializer);

		internal static void Load()
		{
			Instance = new FluidSerializer();

			HookEndpointManager.Add<hook_TryGetSerializer>(typeof(TagSerializer).GetMethod("TryGetSerializer", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic), (hook_TryGetSerializer)((orig_TryGetSerializer orig, Type type, out TagSerializer serializer) =>
			{
				if (type == typeof(BaseFluid) || type.IsSubclassOf(typeof(BaseFluid)))
				{
					serializer = Instance;
					return true;
				}

				return orig(type, out serializer);
			}));
		}

		public override TagCompound Serialize(BaseFluid value) => new TagCompound
		{
			["Mod"] = value.Mod.Name,
			["Name"] = value.Name
		};

		public override BaseFluid Deserialize(TagCompound tag)
		{
			if (ModContent.TryFind(tag.GetString("Mod"), tag.GetString("Name"), out BaseFluid baseFluid))
			{
				BaseFluid fluid = baseFluid.Clone();
				return fluid;
			}

			// todo: UnloadedFluid
			throw new Exception();
		}
	}
}