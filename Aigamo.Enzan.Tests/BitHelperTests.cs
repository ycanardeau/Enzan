using Xunit;

namespace Aigamo.Enzan.Tests
{
	public class BitHelperTests
	{
		[Theory]
		[InlineData(byte.MinValue, byte.MinValue, 0x0000)]
		[InlineData(byte.MaxValue, byte.MinValue, 0x00FF)]
		[InlineData(byte.MinValue, byte.MaxValue, 0xFF00)]
		[InlineData(byte.MaxValue, byte.MaxValue, 0xFFFF)]
		[InlineData(0x34, 0x12, 0x1234)]
		public void MakeShortTest(byte low, byte high, ushort expected)
		{
			Assert.Equal((short)expected, BitHelper.MakeShort(low, high));
		}

		[Theory]
		[InlineData(ushort.MinValue, ushort.MinValue, 0x00000000)]
		[InlineData(ushort.MaxValue, ushort.MinValue, 0x0000FFFF)]
		[InlineData(ushort.MinValue, ushort.MaxValue, 0xFFFF0000)]
		[InlineData(ushort.MaxValue, ushort.MaxValue, 0xFFFFFFFF)]
		[InlineData(0x5678, 0x1234, 0x12345678)]
		public void MakeIntTest(ushort low, ushort high, uint expected)
		{
			Assert.Equal((int)expected, BitHelper.MakeInt((short)low, (short)high));
		}

		[Theory]
		[InlineData(uint.MinValue, uint.MinValue, 0x_00000000_00000000)]
		[InlineData(uint.MaxValue, uint.MinValue, 0x_00000000_FFFFFFFF)]
		[InlineData(uint.MinValue, uint.MaxValue, 0x_FFFFFFFF_00000000)]
		[InlineData(uint.MaxValue, uint.MaxValue, 0x_FFFFFFFF_FFFFFFFF)]
		[InlineData(0x9ABCDEF0, 0x12345678, 0x123456789ABCDEF0)]
		public void MakeLongTest(uint low, uint high, ulong expected)
		{
			Assert.Equal((long)expected, BitHelper.MakeLong((int)low, (int)high));
		}
	}
}
