using Xunit;

namespace Aigamo.Enzan.Tests
{
	public class Register8Tests
	{
		[Fact]
		public void DefaultConstructorTest()
		{
			Assert.Equal(Register8.Empty, new Register8());
		}

		[Theory]
		[InlineData(byte.MaxValue)]
		[InlineData(byte.MinValue)]
		[InlineData(0)]
		public void NonDefaultConstructorTest(byte value)
		{
			var r1 = new Register8(value);
			var r2 = new Register8(value);

			Assert.Equal(r1, r2);
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
		public void EqualityTest_NotRegister8()
		{
			var r = new Register8(0);
			Assert.False(r.Equals(null));
			Assert.False(r.Equals(0));
		}

		[Fact]
		public void GetHashCodeTest()
		{
			var r = new Register8(10);
			Assert.Equal(r.GetHashCode(), new Register8(10).GetHashCode());
			Assert.NotEqual(r.GetHashCode(), new Register8(20).GetHashCode());
		}

		[Theory]
		[InlineData(0)]
		[InlineData(5)]
		public void ToStringTest(byte value)
		{
			var r = new Register8(value);
			Assert.Equal($"{r.Value}", r.ToString());
		}
	}
}
