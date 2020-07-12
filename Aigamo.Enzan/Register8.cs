using System;

namespace Aigamo.Enzan
{
	public readonly struct Register8 : IEquatable<Register8>, IFormattable
	{
		public static readonly Register8 Empty = new();
		public static readonly Register8 One = new Register8(1);

		public byte Value { get; }

		public Register8(byte value) => Value = value;

		public bool IsEmpty => this == Empty;

		public bool Sign => ((Value >> 7) & 1) != 0;

		public static bool operator ==(Register8 left, Register8 right) => left.Equals(right);

		public static bool operator !=(Register8 left, Register8 right) => !left.Equals(right);

		public bool Equals(Register8 other) => Value == other.Value;

		public override bool Equals(object? obj) => obj is Register8 other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(Value);

		public override string ToString() => Value.ToString();

		public string ToString(string? format, IFormatProvider? provider) => Value.ToString(format, provider);

		public Register16 SignExtend() => new Register16((ushort)(sbyte)Value);
	}
}
