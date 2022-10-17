namespace Aigamo.Enzan;

public readonly record struct Sib(byte Value)
{
	public static readonly Sib Empty = default;

	public byte Base => (byte)(Value & 7);

	public byte Index => (byte)((Value >> 3) & 7);

	public byte Scale => (byte)((Value >> 6) & 3);

	public override string ToString() => $"[Sib: Value={Value:X2}, Base={Base}, Index={Index}, Scale={Scale}]";
}
