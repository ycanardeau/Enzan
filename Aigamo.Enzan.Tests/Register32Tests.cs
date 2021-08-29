using FluentAssertions;
using Xunit;

namespace Aigamo.Enzan.Tests
{
	public class Register32Tests
	{
		[Fact]
		public void DefaultConstructorTest()
		{
			new Register32().Should().Be(Register32.Empty);
		}

		[Theory]
		[InlineData(uint.MaxValue)]
		[InlineData(uint.MinValue)]
		[InlineData(0)]
		public void NonDefaultConstructorTest(uint value)
		{
			var r1 = new Register32(value);
			var r2 = new Register32(value);

			r2.Should().Be(r1);
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

			(r1 == r3).Should().BeTrue();
			(r1 != r2).Should().BeTrue();
			(r2 != r3).Should().BeTrue();

			r1.Equals(r3).Should().BeTrue();
			r1.Equals(r2).Should().BeFalse();
			r2.Equals(r3).Should().BeFalse();

			r1.Equals((object)r3).Should().BeTrue();
			r1.Equals((object)r2).Should().BeFalse();
			r2.Equals((object)r3).Should().BeFalse();

			r3.GetHashCode().Should().Be(r1.GetHashCode());
		}

		[Fact]
		public void EqualityTest_NotRegister32()
		{
			var r = new Register32(0);
			r.Equals(null).Should().BeFalse();
			r.Equals(0).Should().BeFalse();
		}

		[Fact]
		public void GetHashCodeTest()
		{
			var r = new Register32(10);
			new Register32(10).GetHashCode().Should().Be(r.GetHashCode());
			new Register32(20).GetHashCode().Should().NotBe(r.GetHashCode());
		}

		[Theory]
		[InlineData(0)]
		[InlineData(5)]
		public void ToStringTest(uint value)
		{
			var r = new Register32(value);
			r.ToString().Should().Be($"{r.Value}");
		}

		[Fact]
		public void LowTest()
		{
			new Register32(0x00000000).Low.Should().Be(new Register16(0x0000));
			new Register32(0x12345678).Low.Should().Be(new Register16(0x5678));
			new Register32(0x87654321).Low.Should().Be(new Register16(0x4321));
			new Register32(0xFFFFFFFF).Low.Should().Be(new Register16(0xFFFF));
		}

		[Fact]
		public void HighTest()
		{
			new Register32(0x00000000).High.Should().Be(new Register16(0x0000));
			new Register32(0x12345678).High.Should().Be(new Register16(0x1234));
			new Register32(0x87654321).High.Should().Be(new Register16(0x8765));
			new Register32(0xFFFFFFFF).High.Should().Be(new Register16(0xFFFF));
		}

		[Fact]
		public void WithLowTest()
		{
			Register32.Empty.WithLow(new Register16(0x1234)).Should().Be(new Register32(0x00001234));
			Register32.Empty.WithLow(new Register16(0x8765)).Should().Be(new Register32(0x00008765));
			new Register32(0x12340000).WithLow(new Register16(0x5678)).Should().Be(new Register32(0x12345678));
			new Register32(0x87650000).WithLow(new Register16(0x4321)).Should().Be(new Register32(0x87654321));
		}

		[Fact]
		public void WithHighTest()
		{
			Register32.Empty.WithHigh(new Register16(0x1234)).Should().Be(new Register32(0x12340000));
			Register32.Empty.WithHigh(new Register16(0x8765)).Should().Be(new Register32(0x87650000));
			new Register32(0x00005678).WithHigh(new Register16(0x1234)).Should().Be(new Register32(0x12345678));
			new Register32(0x00004321).WithHigh(new Register16(0x8765)).Should().Be(new Register32(0x87654321));
		}
	}
}
