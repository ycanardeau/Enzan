using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aigamo.Enzan;

internal static class BitConverterExtensions
{
	// netstandard2.0 lacks BitConverter.SingleToInt32Bits, so this mirrors the .NET Core implementation.
	public static int SingleToInt32Bits(float value) => Unsafe.As<float, int>(ref value);

	// netstandard2.0 lacks BitConverter.Int32BitsToSingle, so this mirrors the .NET Core implementation.
	public static float Int32BitsToSingle(int value) => Unsafe.As<int, float>(ref value);

	// netstandard2.0 lacks BitConverter.ToUInt32(ReadOnlySpan<byte>), so this mirrors the .NET Core implementation.
	public static uint ToUInt32(ReadOnlySpan<byte> value)
	{
		if (value.Length < sizeof(uint))
			throw new ArgumentOutOfRangeException(nameof(value));
		return Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(value));
	}
}
