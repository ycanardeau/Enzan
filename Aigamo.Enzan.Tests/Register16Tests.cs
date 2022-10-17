namespace Aigamo.Enzan.Tests;

public class Register16Tests
{
	[Fact]
	public void DefaultConstructorTest()
	{
		new Register16().Should().Be(Register16.Empty);
	}

	[Theory]
	[InlineData(ushort.MaxValue)]
	[InlineData(ushort.MinValue)]
	[InlineData(0)]
	public void NonDefaultConstructorTest(ushort value)
	{
		var r1 = new Register16(value);
		var r2 = new Register16(value);

		r2.Should().Be(r1);
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
	public void EqualityTest_NotRegister16()
	{
		var r = new Register16(0);
		r.Equals(null).Should().BeFalse();
		r.Equals(0).Should().BeFalse();
	}

	[Fact]
	public void GetHashCodeTest()
	{
		var r = new Register16(10);
		new Register16(10).GetHashCode().Should().Be(r.GetHashCode());
		new Register16(20).GetHashCode().Should().NotBe(r.GetHashCode());
	}

	[Theory]
	[InlineData(0)]
	[InlineData(5)]
	public void ToStringTest(ushort value)
	{
		var r = new Register16(value);
		r.ToString().Should().Be($"{r.Value}");
	}

	[Fact]
	public void LowTest()
	{
		new Register16(0x0000).Low.Should().Be(new Register8(0x00));
		new Register16(0x1234).Low.Should().Be(new Register8(0x34));
		new Register16(0x8765).Low.Should().Be(new Register8(0x65));
		new Register16(0xFFFF).Low.Should().Be(new Register8(0xFF));
	}

	[Fact]
	public void HighTest()
	{
		new Register16(0x0000).High.Should().Be(new Register8(0x00));
		new Register16(0x1234).High.Should().Be(new Register8(0x12));
		new Register16(0x8765).High.Should().Be(new Register8(0x87));
		new Register16(0xFFFF).High.Should().Be(new Register8(0xFF));
	}

	[Fact]
	public void WithLowTest()
	{
		Register16.Empty.WithLow(new Register8(0x12)).Should().Be(new Register16(0x0012));
		Register16.Empty.WithLow(new Register8(0x87)).Should().Be(new Register16(0x0087));
		new Register16(0x1200).WithLow(new Register8(0x34)).Should().Be(new Register16(0x1234));
		new Register16(0x8700).WithLow(new Register8(0x65)).Should().Be(new Register16(0x8765));
	}

	[Fact]
	public void WithHighTest()
	{
		Register16.Empty.WithHigh(new Register8(0x12)).Should().Be(new Register16(0x1200));
		Register16.Empty.WithHigh(new Register8(0x87)).Should().Be(new Register16(0x8700));
		new Register16(0x0034).WithHigh(new Register8(0x12)).Should().Be(new Register16(0x1234));
		new Register16(0x0065).WithHigh(new Register8(0x87)).Should().Be(new Register16(0x8765));
	}
}
