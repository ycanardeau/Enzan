using System.Runtime.CompilerServices;

namespace Aigamo.Enzan
{
	public sealed class FpuStatus
	{
		public FpuFlags Flags { get; set; }

		public bool C0
		{
			get => Flags.HasFlag(FpuFlags.C0);
			set
			{
				if (value)
					Flags |= FpuFlags.C0;
				else
					Flags &= ~FpuFlags.C0;
			}
		}

		public bool C1
		{
			get => Flags.HasFlag(FpuFlags.C1);
			set
			{
				if (value)
					Flags |= FpuFlags.C1;
				else
					Flags &= ~FpuFlags.C1;
			}
		}

		public bool C2
		{
			get => Flags.HasFlag(FpuFlags.C2);
			set
			{
				if (value)
					Flags |= FpuFlags.C2;
				else
					Flags &= ~FpuFlags.C2;
			}
		}

		public bool C3
		{
			get => Flags.HasFlag(FpuFlags.C3);
			set
			{
				if (value)
					Flags |= FpuFlags.C3;
				else
					Flags &= ~FpuFlags.C3;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Update(bool c3, bool c2, bool c0)
		{
			C3 = c3;
			C2 = c2;
			C0 = c0;
		}
	}
}
