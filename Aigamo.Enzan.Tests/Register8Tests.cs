using FluentAssertions;
using Xunit;

namespace Aigamo.Enzan.Tests;

public class Register8Tests
{
	[Fact]
	public void DefaultConstructorTest()
	{
		new Register8().Should().Be(Register8.Empty);
	}

	[Theory]
	[InlineData(byte.MaxValue)]
	[InlineData(byte.MinValue)]
	[InlineData(0)]
	public void NonDefaultConstructorTest(byte value)
	{
		var r1 = new Register8(value);
		var r2 = new Register8(value);

		r2.Should().Be(r1);
	}

	[Theory]
	[InlineData(byte.MaxValue)]
	[InlineData(byte.MinValue)]
	[InlineData(0)]
	public void EqualityTest(byte value)
	{
		var r1 = new Register8(value);
		var r2 = new Register8((byte)(value / 2 - 1));
		var r3 = new Register8(value);

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
	public void EqualityTest_NotRegister8()
	{
		var r = new Register8(0);
		r.Equals(null).Should().BeFalse();
		r.Equals(0).Should().BeFalse();
	}

	[Fact]
	public void GetHashCodeTest()
	{
		var r = new Register8(10);
		new Register8(10).GetHashCode().Should().Be(r.GetHashCode());
		new Register8(20).GetHashCode().Should().NotBe(r.GetHashCode());
	}

	[Theory]
	[InlineData(0)]
	[InlineData(5)]
	public void ToStringTest(byte value)
	{
		var r = new Register8(value);
		r.ToString().Should().Be($"{r.Value}");
	}
}
