using System;

namespace Aigamo.Enzan;

[Flags]
public enum FpuFlags
{
	C0 = 1 << 8,
	C1 = 1 << 9,
	C2 = 1 << 10,
	C3 = 1 << 14,
}
