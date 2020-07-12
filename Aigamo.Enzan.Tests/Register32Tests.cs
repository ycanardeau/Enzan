using Xunit;

namespace Aigamo.Enzan.Tests
{
	public class Register32Tests
	{
		[Fact]
		public void DefaultConstructorTest()
		{
			Assert.Equal(Register32.Empty, new Register32());
		}

		[Theory]
		[InlineData(uint.MaxValue)]
		[InlineData(uint.MinValue)]
		[InlineData(0)]
		public void NonDefaultConstructorTest(uint value)
		{
			var r1 = new Register32(value);
			var r2 = new Register32(value);

			Assert.Equal(r1, r2);
		}

		[Theory]
		[InlineData(uint.MaxValue)]
		[InlineData(uint.MinValue)]
		[InlineData(0)]
		public void EqualityTest(uint value)
		{
			var r1 = new Register32(value);
			var r2 = new Register32(value / 2 - 1);
			var r3 = new Register32(value);

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
		public void EqualityTest_NotRegister32()
		{
			var r = new Register32(0);
			Assert.False(r.Equals(null));
			Assert.False(r.Equals(0));
		}

		[Fact]
		public void GetHashCodeTest()
		{
			var r = new Register32(10);
			Assert.Equal(r.GetHashCode(), new Register32(10).GetHashCode());
			Assert.NotEqual(r.GetHashCode(), new Register32(20).GetHashCode());
		}

		[Theory]
		[InlineData(0)]
		[InlineData(5)]
		public void ToStringTest(uint value)
		{
			var r = new Register32(value);
			Assert.Equal($"{r.Value}", r.ToString());
		}

		[Fact]
		public void LowTest()
		{
			Assert.Equal(new Register16(0x0000), new Register32(0x00000000).Low);
			Assert.Equal(new Register16(0x5678), new Register32(0x12345678).Low);
			Assert.Equal(new Register16(0x4321), new Register32(0x87654321).Low);
			Assert.Equal(new Register16(0xFFFF), new Register32(0xFFFFFFFF).Low);
		}

		[Fact]
		public void HighTest()
		{
			Assert.Equal(new Register16(0x0000), new Register32(0x00000000).High);
			Assert.Equal(new Register16(0x1234), new Register32(0x12345678).High);
			Assert.Equal(new Register16(0x8765), new Register32(0x87654321).High);
			Assert.Equal(new Register16(0xFFFF), new Register32(0xFFFFFFFF).High);
		}

		[Fact]
		public void WithLowTest()
		{
			Assert.Equal(new Register32(0x00001234), Register32.Empty.WithLow(new Register16(0x1234)));
			Assert.Equal(new Register32(0x00008765), Register32.Empty.WithLow(new Register16(0x8765)));
			Assert.Equal(new Register32(0x12345678), new Register32(0x12340000).WithLow(new Register16(0x5678)));
			Assert.Equal(new Register32(0x87654321), new Register32(0x87650000).WithLow(new Register16(0x4321)));
		}

		[Fact]
		public void WithHighTest()
		{
			Assert.Equal(new Register32(0x12340000), Register32.Empty.WithHigh(new Register16(0x1234)));
			Assert.Equal(new Register32(0x87650000), Register32.Empty.WithHigh(new Register16(0x8765)));
			Assert.Equal(new Register32(0x12345678), new Register32(0x00005678).WithHigh(new Register16(0x1234)));
			Assert.Equal(new Register32(0x87654321), new Register32(0x00004321).WithHigh(new Register16(0x8765)));
		}
	}
}
