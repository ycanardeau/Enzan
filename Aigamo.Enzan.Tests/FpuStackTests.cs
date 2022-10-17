using FluentAssertions;
using Xunit;

namespace Aigamo.Enzan.Tests;

public class FpuStackTests
{
	[Fact]
	public void PopTest()
	{
		var stack = new FpuStack();

		stack.Push(Register64.FromDouble(1));
		stack[0].Should().Be(Register64.FromDouble(1));

		stack.Push(Register64.FromDouble(2));
		stack[0].Should().Be(Register64.FromDouble(2));
		stack[1].Should().Be(Register64.FromDouble(1));

		stack.Push(Register64.FromDouble(3));
		stack[0].Should().Be(Register64.FromDouble(3));
		stack[1].Should().Be(Register64.FromDouble(2));
		stack[2].Should().Be(Register64.FromDouble(1));

		stack.Push(Register64.FromDouble(4));
		stack[0].Should().Be(Register64.FromDouble(4));
		stack[1].Should().Be(Register64.FromDouble(3));
		stack[2].Should().Be(Register64.FromDouble(2));
		stack[3].Should().Be(Register64.FromDouble(1));

		stack.Push(Register64.FromDouble(5));
		stack[0].Should().Be(Register64.FromDouble(5));
		stack[1].Should().Be(Register64.FromDouble(4));
		stack[2].Should().Be(Register64.FromDouble(3));
		stack[3].Should().Be(Register64.FromDouble(2));
		stack[4].Should().Be(Register64.FromDouble(1));

		stack.Push(Register64.FromDouble(6));
		stack[0].Should().Be(Register64.FromDouble(6));
		stack[1].Should().Be(Register64.FromDouble(5));
		stack[2].Should().Be(Register64.FromDouble(4));
		stack[3].Should().Be(Register64.FromDouble(3));
		stack[4].Should().Be(Register64.FromDouble(2));
		stack[5].Should().Be(Register64.FromDouble(1));

		stack.Push(Register64.FromDouble(7));
		stack[0].Should().Be(Register64.FromDouble(7));
		stack[1].Should().Be(Register64.FromDouble(6));
		stack[2].Should().Be(Register64.FromDouble(5));
		stack[3].Should().Be(Register64.FromDouble(4));
		stack[4].Should().Be(Register64.FromDouble(3));
		stack[5].Should().Be(Register64.FromDouble(2));
		stack[6].Should().Be(Register64.FromDouble(1));

		stack.Push(Register64.FromDouble(8));
		stack[0].Should().Be(Register64.FromDouble(8));
		stack[1].Should().Be(Register64.FromDouble(7));
		stack[2].Should().Be(Register64.FromDouble(6));
		stack[3].Should().Be(Register64.FromDouble(5));
		stack[4].Should().Be(Register64.FromDouble(4));
		stack[5].Should().Be(Register64.FromDouble(3));
		stack[6].Should().Be(Register64.FromDouble(2));
		stack[7].Should().Be(Register64.FromDouble(1));

		stack.Pop().Should().Be(Register64.FromDouble(8));
		stack.Pop().Should().Be(Register64.FromDouble(7));
		stack.Pop().Should().Be(Register64.FromDouble(6));
		stack.Pop().Should().Be(Register64.FromDouble(5));
		stack.Pop().Should().Be(Register64.FromDouble(4));
		stack.Pop().Should().Be(Register64.FromDouble(3));
		stack.Pop().Should().Be(Register64.FromDouble(2));
		stack.Pop().Should().Be(Register64.FromDouble(1));
	}
}
