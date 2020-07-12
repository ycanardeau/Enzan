﻿using System;
using System.Runtime.CompilerServices;

namespace Aigamo.Enzan
{
	public readonly struct Register16 : IEquatable<Register16>, IFormattable
	{
		public static readonly Register16 Empty = new();

		public ushort Value { get; }

		public Register16(ushort value) => Value = value;

		public bool IsEmpty => this == Empty;

		public Register8 Low => new Register8(BitHelper.Low((short)Value));

		public Register8 High => new Register8(BitHelper.High((short)Value));

		public bool Sign => ((Value >> 15) & 1) != 0;

		public static bool operator ==(Register16 left, Register16 right) => left.Equals(right);

		public static bool operator !=(Register16 left, Register16 right) => !left.Equals(right);

		public Register16 WithLow(Register8 value) => new Register16((ushort)BitHelper.MakeShort(value.Value, High.Value));

		public Register16 WithHigh(Register8 value) => new Register16((ushort)BitHelper.MakeShort(Low.Value, value.Value));

		public bool Equals(Register16 other) => Value == other.Value;

		public override bool Equals(object? obj) => obj is Register16 other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(Value);

		public override string ToString() => Value.ToString();

		public string ToString(string? format, IFormatProvider? provider) => ToString(format, provider);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 SignExtend() => new Register32((uint)(short)Value);
	}
}