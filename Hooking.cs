using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;

namespace FluidLibrary;

internal static class Hooking
{
	internal static void Load()
	{
		IL.Terraria.Recipe.FindRecipes += Recipe_FindRecipes;
		IL.Terraria.Recipe.Create += Recipe_Create;
	}

	private static IEnumerable<ICraftingStorage> GetCraftingStorages(Player player)
	{
		foreach (Item item in player.inventory)
		{
			if (!item.IsAir && item.ModItem is ICraftingStorage storage) yield return storage;
		}
	}

	private static void Recipe_FindRecipes(ILContext il)
	{
		ILCursor cursor = new ILCursor(il);

		// Count fluids in fluid storages as "virtual" buckets 
		if (cursor.TryGotoNext(MoveType.AfterLabel, i => i.MatchLdcI4(0), i => i.MatchStloc(9)))
		{
			cursor.Emit(OpCodes.Ldloc, 6);

			cursor.EmitDelegate<Func<Dictionary<int, int>, Dictionary<int, int>>>(availableItems =>
			{
				foreach (ICraftingStorage craftingStorage in GetCraftingStorages(Main.LocalPlayer))
				{
					FluidStorage storage = craftingStorage.GetFluidStorage();

					foreach (int slot in craftingStorage.GetTanksForCrafting())
					{
						FluidStack fluid = storage[slot];

						if (fluid.Fluid != null)
						{
							int type = fluid.Fluid switch
							{
								Water => ItemID.WaterBucket,
								Lava => ItemID.LavaBucket,
								Honey => ItemID.HoneyBucket,
								_ => 0
							};

							if (type == 0) continue;

							if (availableItems.ContainsKey(type)) availableItems[type] += fluid.Volume / 255;
							else availableItems[type] = fluid.Volume / 255;
						}
					}
				}

				return availableItems;
			});

			cursor.Emit(OpCodes.Stloc, 6);
		}
	}

	private static void Recipe_Create(ILContext il)
	{
		ILCursor cursor = new ILCursor(il);

		// If there are no more buckets, use fluids from storages instead
		if (cursor.TryGotoNext(MoveType.AfterLabel, i => i.MatchLdsfld<Main>("player"), i => i.MatchLdsfld<Main>("myPlayer"), i => i.MatchLdelemRef(), i => i.MatchLdfld<Player>("chest")))
		{
			cursor.Emit(OpCodes.Ldarg, 0);
			cursor.Emit(OpCodes.Ldloc, 3);
			cursor.Emit(OpCodes.Ldloc, 4);

			cursor.EmitDelegate<Func<Recipe, Item, int, int>>((self, ingredient, amount) =>
			{
				if (ingredient.type is not (ItemID.WaterBucket or ItemID.LavaBucket or ItemID.HoneyBucket)) return amount;

				foreach (ICraftingStorage craftingStorage in GetCraftingStorages(Main.LocalPlayer))
				{
					FluidStorage storage = craftingStorage.GetFluidStorage();

					foreach (int slot in craftingStorage.GetTanksForCrafting())
					{
						if (amount <= 0) return amount;

						FluidStack fluid = storage[slot];

						if (fluid.Fluid != null)
						{
							bool validFluid = ingredient.type == ItemID.WaterBucket && fluid.Fluid is Water || ingredient.type == ItemID.LavaBucket && fluid.Fluid is Lava || ingredient.type == ItemID.HoneyBucket && fluid.Fluid is Honey;
							if (validFluid)
							{
								int toRemove = Math.Min(amount, fluid.Volume / 255);
								amount -= toRemove;
								storage.ModifyStackSize(Main.LocalPlayer, slot, -toRemove * 255);
							}
						}
					}
				}

				return amount;
			});

			cursor.Emit(OpCodes.Stloc, 4);
		}
	}
}