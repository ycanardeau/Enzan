using System;
using System.Runtime.CompilerServices;
using Aigamo.Extensions.Primitives;

namespace Aigamo.Enzan
{
	public readonly record struct Register16(ushort Value) : IFormattable
	{
		public static readonly Register16 Empty = default;

		public bool IsEmpty => this == Empty;

		public Register8 Low => new(Value.LowByte());

		public Register8 High => new(Value.HighByte());

		public bool Sign => ((Value >> 15) & 1) != 0;

		public Register16 WithLow(Register8 value) => new(Value.WithLowByte(value.Value));

		public Register16 WithHigh(Register8 value) => new(Value.WithHighByte(value.Value));

		public override string ToString() => Value.ToString();

		public string ToString(string? format, IFormatProvider? provider) => Value.ToString(format, provider);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 SignExtend() => new((uint)(short)Value);
	}
}
