namespace Aigamo.Enzan
{
	public readonly record struct ModRM(byte Value)
	{
		public static readonly ModRM Empty = default;

		public byte RM => (byte)(Value & 7);

		public byte Reg => (byte)((Value >> 3) & 7);

		public byte Opcode => Reg;

		public byte Mod => (byte)((Value >> 6) & 3);

		public override string ToString() => $"[ModRM: Value={Value:X2}, Reg={Reg}, Mod={Mod}]";
	}
}
