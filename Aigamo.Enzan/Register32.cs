using System;
using System.Runtime.CompilerServices;
using Aigamo.Extensions.Primitives;

namespace Aigamo.Enzan
{
	public readonly struct Register32 : IEquatable<Register32>, IFormattable
	{
		public static readonly Register32 Empty = new();

		public uint Value { get; }

		public Register32(uint value) => Value = value;

		public static Register32 FromSingle(float value) => new Register32((uint)BitConverter.SingleToInt32Bits(value));

		public bool IsEmpty => this == Empty;

		public Register16 Low => new Register16(Value.LowUInt16());

		public Register16 High => new Register16(Value.HighUInt16());

		public bool Sign => ((Value >> 31) & 1) != 0;

		public static bool operator ==(Register32 left, Register32 right) => left.Equals(right);

		public static bool operator !=(Register32 left, Register32 right) => !left.Equals(right);

		public static Register32 operator +(Register32 left, Register32 right) => new Register32(left.Value + right.Value);

		public static Register32 operator -(Register32 left, Register32 right) => new Register32(left.Value - right.Value);

		public static Register32 operator *(Register32 left, Register32 right) => new Register32(left.Value * right.Value);

		public Register32 WithLow(Register16 value) => new Register32(Value.WithLowUInt16(value.Value));

		public Register32 WithHigh(Register16 value) => new Register32(Value.WithHighUInt16(value.Value));

		public bool Equals(Register32 other) => Value == other.Value;

		public override bool Equals(object? obj) => obj is Register32 other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(Value);

		public override string ToString() => Value.ToString();

		public string ToString(string? format, IFormatProvider? provider) => Value.ToString(format, provider);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 SignExtend() => new Register64((ulong)(int)Value);

		public float ToSingle() => BitConverter.Int32BitsToSingle((int)Value);
	}
}
