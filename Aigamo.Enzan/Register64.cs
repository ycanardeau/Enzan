using Aigamo.Extensions.Primitives;

namespace Aigamo.Enzan;

public readonly record struct Register64(ulong Value) : IFormattable
{
	public static readonly Register64 Empty = default;

	public static Register64 FromDouble(double value) => new((ulong)BitConverter.DoubleToInt64Bits(value));

	public bool IsEmpty => this == Empty;

	public Register32 Low => new(Value.LowUInt32());

	public Register32 High => new(Value.HighUInt32());

	public override string ToString() => Value.ToString();

	public string ToString(string? format, IFormatProvider? provider) => Value.ToString(format, provider);

	public double ToDouble() => BitConverter.Int64BitsToDouble((long)Value);
}
