using System;

namespace Aigamo.Enzan
{
	public readonly struct Sib : IEquatable<Sib>
	{
		public static readonly Sib Empty = new();

		public byte Value { get; }

		public Sib(byte value) => Value = value;

		public byte Base => (byte)(Value & 7);

		public byte Index => (byte)((Value >> 3) & 7);

		public byte Scale => (byte)((Value >> 6) & 3);

		public static bool operator ==(Sib left, Sib right) => left.Equals(right);

		public static bool operator !=(Sib left, Sib right) => !left.Equals(right);

		public bool Equals(Sib other) => Value == other.Value;

		public override bool Equals(object? obj) => obj is Sib other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(Value);

		public override string ToString() => $"[Sib: Value={Value:X2}, Base={Base}, Index={Index}, Scale={Scale}]";
	}
}
