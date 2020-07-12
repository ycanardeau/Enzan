using System.Runtime.CompilerServices;

namespace Aigamo.Enzan
{
	public sealed class FpuStack
	{
		private readonly Register64[] _registers = new Register64[8];
		private int _top;

		public Register64 this[int index]
		{
			get => _registers[(_top + index) & 0b111];
			set => _registers[(_top + index) & 0b111] = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Push(Register64 value)
		{
			_top = (_top - 1) & 0b111;
			this[0] = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Pop()
		{
			var ret = this[0];
			_top = (_top + 1) & 0b111;
			return ret;
		}
	}
}
