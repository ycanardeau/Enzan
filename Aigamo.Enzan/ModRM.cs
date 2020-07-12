using System;

namespace Aigamo.Enzan
{
	public readonly struct ModRM : IEquatable<ModRM>
	{
		public static readonly ModRM Empty = new();

		public byte Value { get; }

		public ModRM(byte value) => Value = value;

		public byte RM => (byte)(Value & 7);

		public byte Reg => (byte)((Value >> 3) & 7);

		public byte Opcode => Reg;

		public byte Mod => (byte)((Value >> 6) & 3);

		public static bool operator ==(ModRM left, ModRM right) => left.Equals(right);

		public static bool operator !=(ModRM left, ModRM right) => !left.Equals(right);

		public bool Equals(ModRM other) => Value == other.Value;

		public override bool Equals(object? obj) => obj is ModRM other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(Value);

		public override string ToString() => $"[ModRM: Value={Value:X2}, Reg={Reg}, Mod={Mod}]";
	}
}
