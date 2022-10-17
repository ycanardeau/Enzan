using System.Runtime.CompilerServices;
using Aigamo.Extensions.Primitives;

namespace Aigamo.Enzan;

public readonly record struct Register32(uint Value) : IFormattable
{
	public static readonly Register32 Empty = default;

	public static Register32 FromSingle(float value) => new((uint)BitConverter.SingleToInt32Bits(value));

	public bool IsEmpty => this == Empty;

	public Register16 Low => new(Value.LowUInt16());

	public Register16 High => new(Value.HighUInt16());

	public bool Sign => ((Value >> 31) & 1) != 0;

	public static Register32 operator +(Register32 left, Register32 right) => new(left.Value + right.Value);

	public static Register32 operator -(Register32 left, Register32 right) => new(left.Value - right.Value);

	public static Register32 operator *(Register32 left, Register32 right) => new(left.Value * right.Value);

	public Register32 WithLow(Register16 value) => new(Value.WithLowUInt16(value.Value));

	public Register32 WithHigh(Register16 value) => new(Value.WithHighUInt16(value.Value));

	public override string ToString() => Value.ToString();

	public string ToString(string? format, IFormatProvider? provider) => Value.ToString(format, provider);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public Register64 SignExtend() => new((ulong)(int)Value);

	public float ToSingle() => BitConverter.Int32BitsToSingle((int)Value);
}
