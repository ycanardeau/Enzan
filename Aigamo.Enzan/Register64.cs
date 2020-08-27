using System;
using Aigamo.Extensions.Primitives;

namespace Aigamo.Enzan
{
	public readonly struct Register64 : IEquatable<Register64>, IFormattable
	{
		public static readonly Register64 Empty = new();

		public ulong Value { get; }

		public Register64(ulong value) => Value = value;

		public static Register64 FromDouble(double value) => new Register64((ulong)BitConverter.DoubleToInt64Bits(value));

		public bool IsEmpty => this == Empty;

		public Register32 Low => new Register32(Value.LowUInt32());

		public Register32 High => new Register32(Value.HighUInt32());

		public static bool operator ==(Register64 left, Register64 right) => left.Equals(right);

		public static bool operator !=(Register64 left, Register64 right) => !left.Equals(right);

		public bool Equals(Register64 other) => Value == other.Value;

		public override bool Equals(object? obj) => obj is Register64 other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(Value);

		public override string ToString() => Value.ToString();

		public string ToString(string? format, IFormatProvider? provider) => Value.ToString(format, provider);

		public double ToDouble() => BitConverter.Int64BitsToDouble((long)Value);
	}
}
