using System;

namespace Aigamo.Enzan;

[Flags]
public enum Eflags : uint
{
	Carry = 1 << 0,
	Zero = 1 << 6,
	Sign = 1 << 7,
	Overflow = 1 << 11,
}
