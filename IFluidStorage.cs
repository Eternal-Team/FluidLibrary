using System.Collections.Generic;

namespace FluidLibrary;

public interface IFluidStorage
{
	FluidStorage GetFluidStorage();
}

public interface ICraftingStorage : IFluidStorage
{
	IEnumerable<int> GetTanksForCrafting();
}