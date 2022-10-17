namespace Aigamo.Enzan;

public readonly record struct Register8(byte Value) : IFormattable
{
	public static readonly Register8 Empty = default;
	public static readonly Register8 One = new(1);

	public bool IsEmpty => this == Empty;

	public bool Sign => ((Value >> 7) & 1) != 0;

	public override string ToString() => Value.ToString();

	public string ToString(string? format, IFormatProvider? provider) => Value.ToString(format, provider);

	public Register16 SignExtend() => new((ushort)(sbyte)Value);
}
