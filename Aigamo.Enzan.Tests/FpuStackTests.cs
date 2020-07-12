using Xunit;

namespace Aigamo.Enzan.Tests
{
	public class FpuStackTests
	{
		[Fact]
		public void PopTest()
		{
			var stack = new FpuStack();

			stack.Push(Register64.FromDouble(1));
			Assert.Equal(Register64.FromDouble(1), stack[0]);

			stack.Push(Register64.FromDouble(2));
			Assert.Equal(Register64.FromDouble(2), stack[0]);
			Assert.Equal(Register64.FromDouble(1), stack[1]);

			stack.Push(Register64.FromDouble(3));
			Assert.Equal(Register64.FromDouble(3), stack[0]);
			Assert.Equal(Register64.FromDouble(2), stack[1]);
			Assert.Equal(Register64.FromDouble(1), stack[2]);

			stack.Push(Register64.FromDouble(4));
			Assert.Equal(Register64.FromDouble(4), stack[0]);
			Assert.Equal(Register64.FromDouble(3), stack[1]);
			Assert.Equal(Register64.FromDouble(2), stack[2]);
			Assert.Equal(Register64.FromDouble(1), stack[3]);

			stack.Push(Register64.FromDouble(5));
			Assert.Equal(Register64.FromDouble(5), stack[0]);
			Assert.Equal(Register64.FromDouble(4), stack[1]);
			Assert.Equal(Register64.FromDouble(3), stack[2]);
			Assert.Equal(Register64.FromDouble(2), stack[3]);
			Assert.Equal(Register64.FromDouble(1), stack[4]);

			stack.Push(Register64.FromDouble(6));
			Assert.Equal(Register64.FromDouble(6), stack[0]);
			Assert.Equal(Register64.FromDouble(5), stack[1]);
			Assert.Equal(Register64.FromDouble(4), stack[2]);
			Assert.Equal(Register64.FromDouble(3), stack[3]);
			Assert.Equal(Register64.FromDouble(2), stack[4]);
			Assert.Equal(Register64.FromDouble(1), stack[5]);

			stack.Push(Register64.FromDouble(7));
			Assert.Equal(Register64.FromDouble(7), stack[0]);
			Assert.Equal(Register64.FromDouble(6), stack[1]);
			Assert.Equal(Register64.FromDouble(5), stack[2]);
			Assert.Equal(Register64.FromDouble(4), stack[3]);
			Assert.Equal(Register64.FromDouble(3), stack[4]);
			Assert.Equal(Register64.FromDouble(2), stack[5]);
			Assert.Equal(Register64.FromDouble(1), stack[6]);

			stack.Push(Register64.FromDouble(8));
			Assert.Equal(Register64.FromDouble(8), stack[0]);
			Assert.Equal(Register64.FromDouble(7), stack[1]);
			Assert.Equal(Register64.FromDouble(6), stack[2]);
			Assert.Equal(Register64.FromDouble(5), stack[3]);
			Assert.Equal(Register64.FromDouble(4), stack[4]);
			Assert.Equal(Register64.FromDouble(3), stack[5]);
			Assert.Equal(Register64.FromDouble(2), stack[6]);
			Assert.Equal(Register64.FromDouble(1), stack[7]);

			Assert.Equal(Register64.FromDouble(8), stack.Pop());
			Assert.Equal(Register64.FromDouble(7), stack.Pop());
			Assert.Equal(Register64.FromDouble(6), stack.Pop());
			Assert.Equal(Register64.FromDouble(5), stack.Pop());
			Assert.Equal(Register64.FromDouble(4), stack.Pop());
			Assert.Equal(Register64.FromDouble(3), stack.Pop());
			Assert.Equal(Register64.FromDouble(2), stack.Pop());
			Assert.Equal(Register64.FromDouble(1), stack.Pop());
		}
	}
}
