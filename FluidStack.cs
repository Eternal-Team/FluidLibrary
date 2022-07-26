namespace FluidLibrary;

public class FluidStack
{
	public BaseFluid Fluid;
	public int Volume;
	// public int MaxVolume;

	public FluidStack()
	{
			
	}
		
	public FluidStack(BaseFluid fluid, int volume/*, int maxVolume*/)
	{
		Fluid = fluid;
		Volume = volume;
		// MaxVolume = maxVolume;
	}

	public FluidStack Clone()
	{
		FluidStack stack = (FluidStack)MemberwiseClone();
		stack.Fluid = Fluid?.Clone();
		stack.Volume = Volume;
		// stack.MaxVolume = MaxVolume;
		return stack;
	}
}