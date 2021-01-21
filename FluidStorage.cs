using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader.IO;

namespace FluidLibrary
{
	public class FluidStorage : IReadOnlyList<FluidStack>
	{
		[Flags]
		public enum Operation
		{
			Insert = 1,
			Remove = 2,
			Both = 3
		}

		internal FluidStack[] Fluids;

		public int Count => Fluids.Length;

		public FluidStack this[int index]
		{
			get => Fluids[index];
			internal set => Fluids[index] = value;
		}

		public FluidStorage(int size)
		{
			Fluids = new FluidStack[size];
			for (int i = 0; i < size; i++) Fluids[i] = new FluidStack();
		}

		public FluidStorage(IEnumerable<FluidStack> fluids)
		{
			Fluids = fluids.ToArray();
		}

		public FluidStorage Clone()
		{
			FluidStorage storage = (FluidStorage)MemberwiseClone();
			storage.Fluids = Fluids.Select(fluid => fluid?.Clone()).ToArray();
			return storage;
		}

		internal void ValidateSlotIndex(int slot)
		{
			if (slot < 0 || slot >= Count) throw new Exception($"Slot {slot} not in valid range - [0, {Count - 1}]");
		}

		/// <summary>
		/// Puts an fluid into the storage.
		/// </summary>
		/// <param name="user">The object doing this.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="fluid">The fluid.</param>
		/// <returns>
		/// True if the fluid was successfully inserted, even partially. False if the fluid is air, if the slot is already
		/// fully occupied, if the slot rejects the fluid, or if the slot rejects the user.
		/// </returns>
		public bool InsertFluid(object user, int slot, ref FluidStack fluid)
		{
			if (fluid?.Fluid == null) return false;

			ValidateSlotIndex(slot);

			if (!CanInteract(slot, Operation.Insert, user) || !IsFluidValid(slot, fluid)) return false;

			FluidStack existing = Fluids[slot];
			if (existing.Fluid != null && !Equals(existing.Fluid, fluid.Fluid)) return false;

			int slotSize = existing.MaxVolume;
			if (slotSize < 0) slotSize = int.MaxValue;
			int toInsert = Utils.Min(slotSize, slotSize - existing.Volume, fluid.Volume);
			if (toInsert <= 0) return false;

			bool reachedLimit = fluid.Volume > toInsert;

			// OnFluidInsert?.Invoke(user, slot, fluid);

			if (existing.Fluid == null) Fluids[slot] = reachedLimit ? CloneFluidWithSize(fluid, toInsert) : new FluidStack(fluid.Fluid, fluid.Volume, slotSize);
			else existing.Volume += toInsert;

			fluid = reachedLimit ? CloneFluidWithSize(fluid, fluid.Volume - toInsert) : new FluidStack(null, 0, fluid.MaxVolume);
			return true;
		}

		/// <summary>
		/// Puts an fluid into storage, disregarding what slots to put it in.
		/// </summary>
		/// <param name="user">The object doing this.</param>
		/// <param name="fluid">The fluid to insert.</param>
		/// <returns>
		/// True if the fluid was successfully inserted, even partially. False if the fluid is air, if the slot is already
		/// fully occupied, if the slot rejects the fluid, or if the slot rejects the user.
		/// </returns>
		public bool InsertFluid(object user, ref FluidStack fluid)
		{
			if (fluid?.Fluid == null) return false;

			bool ret = false;
			for (int i = 0; i < Count; i++)
			{
				FluidStack other = Fluids[i];
				if (Equals(fluid.Fluid, other.Fluid) && other.Volume < other.MaxVolume) ret |= InsertFluid(user, i, ref fluid);
			}

			for (int i = 0; i < Count; i++)
			{
				FluidStack other = Fluids[i];
				if (other.Fluid == null) ret |= InsertFluid(user, i, ref fluid);
			}

			return ret;
		}

		/// <summary>
		/// Removes an fluid from storage and returns the fluid that was grabbed.
		/// <para />
		/// Compare the stack of the <paramref name="fluid" /> parameter with the <paramref name="amount" /> parameter to see if
		/// the fluid was completely taken.
		/// </summary>
		/// <param name="slot">The slot.</param>
		/// <param name="fluid">The fluid that is . Returns null if unsuccessful.</param>
		/// <param name="amount">The amount of fluids to take from a stack.</param>
		/// <param name="user">The object doing this.</param>
		/// <returns>Returns true if any fluids were actually removed. False if the slot is air or if the slot rejects the user.</returns>
		public bool RemoveFluid(object user, int slot, out FluidStack fluid, int amount = -1)
		{
			fluid = Fluids[slot];

			if (amount == 0) return false;

			ValidateSlotIndex(slot);

			if (!CanInteract(slot, Operation.Remove, user)) return false;

			if (fluid.Fluid == null) return false;

			// OnFluidRemove?.Invoke(user, slot);

			int toExtract = Utils.Min(amount < 0 ? int.MaxValue : amount, fluid.MaxVolume, fluid.Volume);

			if (fluid.Volume <= toExtract)
			{
				Fluids[slot] = null;

				return true;
			}

			fluid = CloneFluidWithSize(fluid, toExtract);
			Fluids[slot] = CloneFluidWithSize(fluid, fluid.Volume - toExtract);

			return true;
		}

		/// <summary>
		/// Removes an fluid from storage.
		/// </summary>
		/// <param name="user">The object doing this.</param>
		/// <param name="slot">The slot.</param>
		/// <returns>Returns true if any fluids were actually removed.</returns>
		public bool RemoveFluid(object user, int slot) => RemoveFluid(user, slot, out _);

		/// <summary>
		/// Adds or subtracts to the fluid in the slot specified's stack.
		/// </summary>
		/// <param name="quantity">The amount to increase/decrease the fluid's stack.</param>
		/// <param name="user">The object doing this.</param>
		/// <returns>
		/// True if the fluid was successfully affected. False if the slot denies the user, if the fluid is air, or if the
		/// quantity is zero.
		/// </returns>
		public bool ModifyStackSize(object user, int slot, int quantity)
		{
			FluidStack fluid = Fluids[slot];

			if (quantity > 0 && !CanInteract(slot, Operation.Insert, user) || quantity < 0 && !CanInteract(slot, Operation.Remove, user) || quantity == 0 || fluid.Fluid == null) return false;

			// OnStackModify?.Invoke(user, slot, ref quantity);

			if (quantity < 0)
			{
				if (fluid.Volume + quantity < 0) return false;

				fluid.Volume += quantity;
				if (fluid.Volume <= 0) fluid.Fluid = null;
				// OnContentsChanged(slot, user);
			}
			else
			{
				int limit = fluid.MaxVolume;
				if (fluid.Volume + quantity > limit) return false;

				fluid.Volume += quantity;
				// OnContentsChanged(slot, user);
			}

			return true;
		}

		/// <summary>
		/// Gets if a given fluid is valid to be inserted into in a given slot.
		/// </summary>
		/// <param name="fluid">An fluid to be tried against the slot.</param>
		public virtual bool IsFluidValid(int slot, FluidStack fluid) => true;

		/// <summary>
		/// Gets if a given user can interact with a slot in the storage.
		/// </summary>
		/// <param name="operation">Whether the user is putting an fluid in or taking an fluid out.</param>
		public virtual bool CanInteract(int slot, Operation operation, object user) => true;

		#region IO
		public virtual TagCompound Save()
		{
			return new TagCompound { ["Fluids"] = Fluids.ToList() };
		}

		public virtual void Load(TagCompound tag)
		{
			Fluids = tag.GetList<FluidStack>("Fluids").ToArray();
		}

		// public virtual void Write(BinaryWriter writer)
		// {
		// 	writer.Write(Count);
		//
		// 	for (int i = 0; i < Count; i++)
		// 	{
		// 		FluidIO.Send(Fluids[i], writer, true, true);
		// 	}
		// }
		//
		// public virtual void Read(BinaryReader reader)
		// {
		// 	int size = reader.ReadInt32();
		//
		// 	Fluids = new Fluid[size];
		//
		// 	for (int i = 0; i < Count; i++)
		// 	{
		// 		Fluids[i] = FluidIO.Receive(reader, true, true);
		// 	}
		// }
		#endregion

		private static FluidStack CloneFluidWithSize(FluidStack fluidStack, int size)
		{
			if (size == 0) return null;
			FluidStack copy = fluidStack.Clone();
			copy.Volume = size;
			return copy;
		}

		public IEnumerator<FluidStack> GetEnumerator() => Fluids.AsEnumerable().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public override string ToString() => $"{GetType()} with {Count} slots";
	}

	public interface IFluidStorage
	{
		FluidStorage GetFluidStorage();
	}
}