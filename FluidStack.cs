namespace FluidLibrary;

public class FluidStack
{
	public BaseFluid Fluid;
	public int Volume;

	public FluidStack()
	{
			
	}
		
	public FluidStack(BaseFluid fluid, int volume)
	{
		Fluid = fluid;
		Volume = volume;
	}

	public FluidStack Clone()
	{
		FluidStack stack = (FluidStack)MemberwiseClone();
		stack.Fluid = Fluid?.Clone();
		stack.Volume = Volume;
		return stack;
	}
}