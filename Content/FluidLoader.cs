using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FluidLibrary.Content
{
	public static class FluidLoader
	{
		internal static Dictionary<string, ModFluid> fluids = new Dictionary<string, ModFluid>();
		internal static Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();

		public static ModFluid GetFluid(string name) => fluids.TryGetValue(name, out ModFluid item) ? item : null;

		public static T GetFluid<T>() where T : ModFluid => (T)GetFluid(typeof(T).Name);

		internal static void Load()
		{
			foreach (Mod mod in ModLoader.Mods.Where(mod => mod != null && mod.Code != null))
			{
				foreach (Type type in mod.Code.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ModFluid)))) AutoloadFluid(type, mod);
			}
		}

		internal static void Unload()
		{
			fluids.Clear();
			textureCache.Clear();
		}

		private static void AutoloadFluid(Type type, Mod mod)
		{
			ModFluid modFluid = (ModFluid)Activator.CreateInstance(type);
			string name = type.Name;

			modFluid.Mod = mod;
			modFluid.Name = name;
			modFluid.DisplayName = mod.CreateTranslation(mod.Name + "." + name);
			fluids[name] = modFluid;

			modFluid.Initialize();

			if (!textureCache.ContainsKey(modFluid.Texture)) textureCache.Add(modFluid.Texture, ModContent.GetTexture(modFluid.Texture));
			TagSerializer.AddSerializer((TagSerializer)Activator.CreateInstance(typeof(FluidSerializer<>).MakeGenericType(type)));
		}
	}
}