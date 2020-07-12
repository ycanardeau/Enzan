namespace Aigamo.Enzan
{
	public sealed class Fpu
	{
		public FpuStack Stack { get; } = new();
		public FpuStatus Status { get; } = new();
	}
}
