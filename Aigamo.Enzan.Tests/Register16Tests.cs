using Xunit;

namespace Aigamo.Enzan.Tests
{
	public class Register16Tests
	{
		[Fact]
		public void DefaultConstructorTest()
		{
			Assert.Equal(Register16.Empty, new Register16());
		}

		[Theory]
		[InlineData(ushort.MaxValue)]
		[InlineData(ushort.MinValue)]
		[InlineData(0)]
		public void NonDefaultConstructorTest(ushort value)
		{
			var r1 = new Register16(value);
			var r2 = new Register16(value);

			Assert.Equal(r1, r2);
		}

		[Theory]
		[InlineData(ushort.MaxValue)]
		[InlineData(ushort.MinValue)]
		[InlineData(0)]
		public void EqualityTest(ushort value)
		{
			var r1 = new Register16(value);
			var r2 = new Register16((ushort)(value / 2 - 1));
			var r3 = new Register16(value);

			Assert.True(r1 == r3);
			Assert.True(r1 != r2);
			Assert.True(r2 != r3);

			Assert.True(r1.Equals(r3));
			Assert.False(r1.Equals(r2));
			Assert.False(r2.Equals(r3));

			Assert.True(r1.Equals((object)r3));
			Assert.False(r1.Equals((object)r2));
			Assert.False(r2.Equals((object)r3));

			Assert.Equal(r1.GetHashCode(), r3.GetHashCode());
		}

		[Fact]
		public void EqualityTest_NotRegister16()
		{
			var r = new Register16(0);
			Assert.False(r.Equals(null));
			Assert.False(r.Equals(0));
		}

		[Fact]
		public void GetHashCodeTest()
		{
			var r = new Register16(10);
			Assert.Equal(r.GetHashCode(), new Register16(10).GetHashCode());
			Assert.NotEqual(r.GetHashCode(), new Register16(20).GetHashCode());
		}

		[Theory]
		[InlineData(0)]
		[InlineData(5)]
		public void ToStringTest(ushort value)
		{
			var r = new Register16(value);
			Assert.Equal($"{r.Value}", r.ToString());
		}

		[Fact]
		public void LowTest()
		{
			Assert.Equal(new Register8(0x00), new Register16(0x0000).Low);
			Assert.Equal(new Register8(0x34), new Register16(0x1234).Low);
			Assert.Equal(new Register8(0x65), new Register16(0x8765).Low);
			Assert.Equal(new Register8(0xFF), new Register16(0xFFFF).Low);
		}

		[Fact]
		public void HighTest()
		{
			Assert.Equal(new Register8(0x00), new Register16(0x0000).High);
			Assert.Equal(new Register8(0x12), new Register16(0x1234).High);
			Assert.Equal(new Register8(0x87), new Register16(0x8765).High);
			Assert.Equal(new Register8(0xFF), new Register16(0xFFFF).High);
		}

		[Fact]
		public void WithLowTest()
		{
			Assert.Equal(new Register16(0x0012), Register16.Empty.WithLow(new Register8(0x12)));
			Assert.Equal(new Register16(0x0087), Register16.Empty.WithLow(new Register8(0x87)));
			Assert.Equal(new Register16(0x1234), new Register16(0x1200).WithLow(new Register8(0x34)));
			Assert.Equal(new Register16(0x8765), new Register16(0x8700).WithLow(new Register8(0x65)));
		}

		[Fact]
		public void WithHighTest()
		{
			Assert.Equal(new Register16(0x1200), Register16.Empty.WithHigh(new Register8(0x12)));
			Assert.Equal(new Register16(0x8700), Register16.Empty.WithHigh(new Register8(0x87)));
			Assert.Equal(new Register16(0x1234), new Register16(0x0034).WithHigh(new Register8(0x12)));
			Assert.Equal(new Register16(0x8765), new Register16(0x0065).WithHigh(new Register8(0x87)));
		}
	}
}
