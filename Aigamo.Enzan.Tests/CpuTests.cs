using System;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Aigamo.Enzan.Tests
{
	public class CpuTests
	{
		private readonly Cpu _cpu = new(Array.Empty<byte>(), Register32.Empty);

		[Fact]
		public void Register32Test()
		{
			_cpu.Eax = new Register32(0x31415926);
			_cpu.Ecx = new Register32(0x53589793);
			_cpu.Edx = new Register32(0x23846264);
			_cpu.Ebx = new Register32(0x33832795);
			_cpu.Esp = new Register32(0x02884197);
			_cpu.Ebp = new Register32(0x16939937);
			_cpu.Esi = new Register32(0x51058209);
			_cpu.Edi = new Register32(0x74944592);

			_cpu.Ax.Should().Be(new Register16(0x5926));
			_cpu.Cx.Should().Be(new Register16(0x9793));
			_cpu.Dx.Should().Be(new Register16(0x6264));
			_cpu.Bx.Should().Be(new Register16(0x2795));
			_cpu.Sp.Should().Be(new Register16(0x4197));
			_cpu.Bp.Should().Be(new Register16(0x9937));
			_cpu.Si.Should().Be(new Register16(0x8209));
			_cpu.Di.Should().Be(new Register16(0x4592));

			_cpu.Al.Should().Be(new Register8(0x26));
			_cpu.Cl.Should().Be(new Register8(0x93));
			_cpu.Dl.Should().Be(new Register8(0x64));
			_cpu.Bl.Should().Be(new Register8(0x95));

			_cpu.Ah.Should().Be(new Register8(0x59));
			_cpu.Ch.Should().Be(new Register8(0x97));
			_cpu.Dh.Should().Be(new Register8(0x62));
			_cpu.Bh.Should().Be(new Register8(0x27));
		}

		[Fact]
		public void Register16Test()
		{
			_cpu.Eax = new Register32(0x31410000);
			_cpu.Ecx = new Register32(0x53580000);
			_cpu.Edx = new Register32(0x23840000);
			_cpu.Ebx = new Register32(0x33830000);
			_cpu.Esp = new Register32(0x02880000);
			_cpu.Ebp = new Register32(0x16930000);
			_cpu.Esi = new Register32(0x51050000);
			_cpu.Edi = new Register32(0x74940000);

			_cpu.Ax = new Register16(0x5926);
			_cpu.Cx = new Register16(0x9793);
			_cpu.Dx = new Register16(0x6264);
			_cpu.Bx = new Register16(0x2795);
			_cpu.Sp = new Register16(0x4197);
			_cpu.Bp = new Register16(0x9937);
			_cpu.Si = new Register16(0x8209);
			_cpu.Di = new Register16(0x4592);

			_cpu.Eax.Should().Be(new Register32(0x31415926));
			_cpu.Ecx.Should().Be(new Register32(0x53589793));
			_cpu.Edx.Should().Be(new Register32(0x23846264));
			_cpu.Ebx.Should().Be(new Register32(0x33832795));
			_cpu.Esp.Should().Be(new Register32(0x02884197));
			_cpu.Ebp.Should().Be(new Register32(0x16939937));
			_cpu.Esi.Should().Be(new Register32(0x51058209));
			_cpu.Edi.Should().Be(new Register32(0x74944592));
		}

		[Fact]
		public void Register8Test()
		{
			_cpu.Eax = new Register32(0x31410000);
			_cpu.Ecx = new Register32(0x53580000);
			_cpu.Edx = new Register32(0x23840000);
			_cpu.Ebx = new Register32(0x33830000);

			_cpu.Al = new Register8(0x26);
			_cpu.Cl = new Register8(0x93);
			_cpu.Dl = new Register8(0x64);
			_cpu.Bl = new Register8(0x95);

			_cpu.Ah = new Register8(0x59);
			_cpu.Ch = new Register8(0x97);
			_cpu.Dh = new Register8(0x62);
			_cpu.Bh = new Register8(0x27);

			_cpu.Eax.Should().Be(new Register32(0x31415926));
			_cpu.Ecx.Should().Be(new Register32(0x53589793));
			_cpu.Edx.Should().Be(new Register32(0x23846264));
			_cpu.Ebx.Should().Be(new Register32(0x33832795));
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80000000, 0x00000000, true, true, false, true)]
		[InlineData(0x80000000, 0x00008000, 0x80008000, false, false, true, false)]
		[InlineData(0x80000000, 0x00000080, 0x80000080, false, false, true, false)]
		[InlineData(0x80000000, 0x00000000, 0x80000000, false, false, true, false)]
		[InlineData(0x80000000, 0x0000007F, 0x8000007F, false, false, true, false)]
		[InlineData(0x80000000, 0x000000FF, 0x800000FF, false, false, true, false)]
		[InlineData(0x80000000, 0x00007FFF, 0x80007FFF, false, false, true, false)]
		[InlineData(0x80000000, 0x0000FFFF, 0x8000FFFF, false, false, true, false)]
		[InlineData(0x80000000, 0x7FFFFFFF, 0xFFFFFFFF, false, false, true, false)]
		[InlineData(0x80000000, 0xFFFFFFFF, 0x7FFFFFFF, true, false, false, true)]
		[InlineData(0x00008000, 0x80000000, 0x80008000, false, false, true, false)]
		[InlineData(0x00008000, 0x00008000, 0x00010000, false, false, false, false)]
		[InlineData(0x00008000, 0x00000080, 0x00008080, false, false, false, false)]
		[InlineData(0x00008000, 0x00000000, 0x00008000, false, false, false, false)]
		[InlineData(0x00008000, 0x0000007F, 0x0000807F, false, false, false, false)]
		[InlineData(0x00008000, 0x000000FF, 0x000080FF, false, false, false, false)]
		[InlineData(0x00008000, 0x00007FFF, 0x0000FFFF, false, false, false, false)]
		[InlineData(0x00008000, 0x0000FFFF, 0x00017FFF, false, false, false, false)]
		[InlineData(0x00008000, 0x7FFFFFFF, 0x80007FFF, false, false, true, true)]
		[InlineData(0x00008000, 0xFFFFFFFF, 0x00007FFF, true, false, false, false)]
		[InlineData(0x00000080, 0x80000000, 0x80000080, false, false, true, false)]
		[InlineData(0x00000080, 0x00008000, 0x00008080, false, false, false, false)]
		[InlineData(0x00000080, 0x00000080, 0x00000100, false, false, false, false)]
		[InlineData(0x00000080, 0x00000000, 0x00000080, false, false, false, false)]
		[InlineData(0x00000080, 0x0000007F, 0x000000FF, false, false, false, false)]
		[InlineData(0x00000080, 0x000000FF, 0x0000017F, false, false, false, false)]
		[InlineData(0x00000080, 0x00007FFF, 0x0000807F, false, false, false, false)]
		[InlineData(0x00000080, 0x0000FFFF, 0x0001007F, false, false, false, false)]
		[InlineData(0x00000080, 0x7FFFFFFF, 0x8000007F, false, false, true, true)]
		[InlineData(0x00000080, 0xFFFFFFFF, 0x0000007F, true, false, false, false)]
		[InlineData(0x00000000, 0x80000000, 0x80000000, false, false, true, false)]
		[InlineData(0x00000000, 0x00008000, 0x00008000, false, false, false, false)]
		[InlineData(0x00000000, 0x00000080, 0x00000080, false, false, false, false)]
		[InlineData(0x00000000, 0x00000000, 0x00000000, false, true, false, false)]
		[InlineData(0x00000000, 0x0000007F, 0x0000007F, false, false, false, false)]
		[InlineData(0x00000000, 0x000000FF, 0x000000FF, false, false, false, false)]
		[InlineData(0x00000000, 0x00007FFF, 0x00007FFF, false, false, false, false)]
		[InlineData(0x00000000, 0x0000FFFF, 0x0000FFFF, false, false, false, false)]
		[InlineData(0x00000000, 0x7FFFFFFF, 0x7FFFFFFF, false, false, false, false)]
		[InlineData(0x00000000, 0xFFFFFFFF, 0xFFFFFFFF, false, false, true, false)]
		[InlineData(0x0000007F, 0x80000000, 0x8000007F, false, false, true, false)]
		[InlineData(0x0000007F, 0x00008000, 0x0000807F, false, false, false, false)]
		[InlineData(0x0000007F, 0x00000080, 0x000000FF, false, false, false, false)]
		[InlineData(0x0000007F, 0x00000000, 0x0000007F, false, false, false, false)]
		[InlineData(0x0000007F, 0x0000007F, 0x000000FE, false, false, false, false)]
		[InlineData(0x0000007F, 0x000000FF, 0x0000017E, false, false, false, false)]
		[InlineData(0x0000007F, 0x00007FFF, 0x0000807E, false, false, false, false)]
		[InlineData(0x0000007F, 0x0000FFFF, 0x0001007E, false, false, false, false)]
		[InlineData(0x0000007F, 0x7FFFFFFF, 0x8000007E, false, false, true, true)]
		[InlineData(0x0000007F, 0xFFFFFFFF, 0x0000007E, true, false, false, false)]
		[InlineData(0x000000FF, 0x80000000, 0x800000FF, false, false, true, false)]
		[InlineData(0x000000FF, 0x00008000, 0x000080FF, false, false, false, false)]
		[InlineData(0x000000FF, 0x00000080, 0x0000017F, false, false, false, false)]
		[InlineData(0x000000FF, 0x00000000, 0x000000FF, false, false, false, false)]
		[InlineData(0x000000FF, 0x0000007F, 0x0000017E, false, false, false, false)]
		[InlineData(0x000000FF, 0x000000FF, 0x000001FE, false, false, false, false)]
		[InlineData(0x000000FF, 0x00007FFF, 0x000080FE, false, false, false, false)]
		[InlineData(0x000000FF, 0x0000FFFF, 0x000100FE, false, false, false, false)]
		[InlineData(0x000000FF, 0x7FFFFFFF, 0x800000FE, false, false, true, true)]
		[InlineData(0x000000FF, 0xFFFFFFFF, 0x000000FE, true, false, false, false)]
		[InlineData(0x00007FFF, 0x80000000, 0x80007FFF, false, false, true, false)]
		[InlineData(0x00007FFF, 0x00008000, 0x0000FFFF, false, false, false, false)]
		[InlineData(0x00007FFF, 0x00000080, 0x0000807F, false, false, false, false)]
		[InlineData(0x00007FFF, 0x00000000, 0x00007FFF, false, false, false, false)]
		[InlineData(0x00007FFF, 0x0000007F, 0x0000807E, false, false, false, false)]
		[InlineData(0x00007FFF, 0x000000FF, 0x000080FE, false, false, false, false)]
		[InlineData(0x00007FFF, 0x00007FFF, 0x0000FFFE, false, false, false, false)]
		[InlineData(0x00007FFF, 0x0000FFFF, 0x00017FFE, false, false, false, false)]
		[InlineData(0x00007FFF, 0x7FFFFFFF, 0x80007FFE, false, false, true, true)]
		[InlineData(0x00007FFF, 0xFFFFFFFF, 0x00007FFE, true, false, false, false)]
		[InlineData(0x0000FFFF, 0x80000000, 0x8000FFFF, false, false, true, false)]
		[InlineData(0x0000FFFF, 0x00008000, 0x00017FFF, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x00000080, 0x0001007F, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x00000000, 0x0000FFFF, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x0000007F, 0x0001007E, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x000000FF, 0x000100FE, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x00007FFF, 0x00017FFE, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x0000FFFF, 0x0001FFFE, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x7FFFFFFF, 0x8000FFFE, false, false, true, true)]
		[InlineData(0x0000FFFF, 0xFFFFFFFF, 0x0000FFFE, true, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x80000000, 0xFFFFFFFF, false, false, true, false)]
		[InlineData(0x7FFFFFFF, 0x00008000, 0x80007FFF, false, false, true, true)]
		[InlineData(0x7FFFFFFF, 0x00000080, 0x8000007F, false, false, true, true)]
		[InlineData(0x7FFFFFFF, 0x00000000, 0x7FFFFFFF, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x0000007F, 0x8000007E, false, false, true, true)]
		[InlineData(0x7FFFFFFF, 0x000000FF, 0x800000FE, false, false, true, true)]
		[InlineData(0x7FFFFFFF, 0x00007FFF, 0x80007FFE, false, false, true, true)]
		[InlineData(0x7FFFFFFF, 0x0000FFFF, 0x8000FFFE, false, false, true, true)]
		[InlineData(0x7FFFFFFF, 0x7FFFFFFF, 0xFFFFFFFE, false, false, true, true)]
		[InlineData(0x7FFFFFFF, 0xFFFFFFFF, 0x7FFFFFFE, true, false, false, false)]
		[InlineData(0xFFFFFFFF, 0x80000000, 0x7FFFFFFF, true, false, false, true)]
		[InlineData(0xFFFFFFFF, 0x00008000, 0x00007FFF, true, false, false, false)]
		[InlineData(0xFFFFFFFF, 0x00000080, 0x0000007F, true, false, false, false)]
		[InlineData(0xFFFFFFFF, 0x00000000, 0xFFFFFFFF, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x0000007F, 0x0000007E, true, false, false, false)]
		[InlineData(0xFFFFFFFF, 0x000000FF, 0x000000FE, true, false, false, false)]
		[InlineData(0xFFFFFFFF, 0x00007FFF, 0x00007FFE, true, false, false, false)]
		[InlineData(0xFFFFFFFF, 0x0000FFFF, 0x0000FFFE, true, false, false, false)]
		[InlineData(0xFFFFFFFF, 0x7FFFFFFF, 0x7FFFFFFE, true, false, false, false)]
		[InlineData(0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFE, true, false, true, false)]
		#endregion
		public void Add32Test(uint left, uint right, uint result, bool carry, bool zero, bool sign, bool overflow)
		{
			_cpu.Add(new Register32(left), new Register32(right)).Should().Be(new Register32(result));
			_cpu.Eflags.Carry.Should().Be(carry);
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().Be(overflow);
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80, 0x7FFFFF80)]
		[InlineData(0x80000000, 0x00, 0x80000000)]
		[InlineData(0x80000000, 0x7F, 0x8000007F)]
		[InlineData(0x80000000, 0xFF, 0x7FFFFFFF)]
		[InlineData(0x00008000, 0x80, 0x00007F80)]
		[InlineData(0x00008000, 0x00, 0x00008000)]
		[InlineData(0x00008000, 0x7F, 0x0000807F)]
		[InlineData(0x00008000, 0xFF, 0x00007FFF)]
		[InlineData(0x00000080, 0x80, 0x00000000)]
		[InlineData(0x00000080, 0x00, 0x00000080)]
		[InlineData(0x00000080, 0x7F, 0x000000FF)]
		[InlineData(0x00000080, 0xFF, 0x0000007F)]
		[InlineData(0x00000000, 0x80, 0xFFFFFF80)]
		[InlineData(0x00000000, 0x00, 0x00000000)]
		[InlineData(0x00000000, 0x7F, 0x0000007F)]
		[InlineData(0x00000000, 0xFF, 0xFFFFFFFF)]
		[InlineData(0x0000007F, 0x80, 0xFFFFFFFF)]
		[InlineData(0x0000007F, 0x00, 0x0000007F)]
		[InlineData(0x0000007F, 0x7F, 0x000000FE)]
		[InlineData(0x0000007F, 0xFF, 0x0000007E)]
		[InlineData(0x000000FF, 0x80, 0x0000007F)]
		[InlineData(0x000000FF, 0x00, 0x000000FF)]
		[InlineData(0x000000FF, 0x7F, 0x0000017E)]
		[InlineData(0x000000FF, 0xFF, 0x000000FE)]
		[InlineData(0x00007FFF, 0x80, 0x00007F7F)]
		[InlineData(0x00007FFF, 0x00, 0x00007FFF)]
		[InlineData(0x00007FFF, 0x7F, 0x0000807E)]
		[InlineData(0x00007FFF, 0xFF, 0x00007FFE)]
		[InlineData(0x0000FFFF, 0x80, 0x0000FF7F)]
		[InlineData(0x0000FFFF, 0x00, 0x0000FFFF)]
		[InlineData(0x0000FFFF, 0x7F, 0x0001007E)]
		[InlineData(0x0000FFFF, 0xFF, 0x0000FFFE)]
		[InlineData(0x7FFFFFFF, 0x80, 0x7FFFFF7F)]
		[InlineData(0x7FFFFFFF, 0x00, 0x7FFFFFFF)]
		[InlineData(0x7FFFFFFF, 0x7F, 0x8000007E)]
		[InlineData(0x7FFFFFFF, 0xFF, 0x7FFFFFFE)]
		[InlineData(0xFFFFFFFF, 0x80, 0xFFFFFF7F)]
		[InlineData(0xFFFFFFFF, 0x00, 0xFFFFFFFF)]
		[InlineData(0xFFFFFFFF, 0x7F, 0x0000007E)]
		[InlineData(0xFFFFFFFF, 0xFF, 0xFFFFFFFE)]
		#endregion
		public void Add32_8Test(uint left, byte right, uint result)
		{
			_cpu.Add(new Register32(left), new Register8(right)).Should().Be(new Register32(result));
		}

		[Theory]
		#region
		[InlineData(0x80, 0x80, 0x80, false, true)]
		[InlineData(0x80, 0x00, 0x00, true, false)]
		[InlineData(0x80, 0x7F, 0x00, true, false)]
		[InlineData(0x80, 0xFF, 0x80, false, true)]
		[InlineData(0x00, 0x80, 0x00, true, false)]
		[InlineData(0x00, 0x00, 0x00, true, false)]
		[InlineData(0x00, 0x7F, 0x00, true, false)]
		[InlineData(0x00, 0xFF, 0x00, true, false)]
		[InlineData(0x7F, 0x80, 0x00, true, false)]
		[InlineData(0x7F, 0x00, 0x00, true, false)]
		[InlineData(0x7F, 0x7F, 0x7F, false, false)]
		[InlineData(0x7F, 0xFF, 0x7F, false, false)]
		[InlineData(0xFF, 0x80, 0x80, false, true)]
		[InlineData(0xFF, 0x00, 0x00, true, false)]
		[InlineData(0xFF, 0x7F, 0x7F, false, false)]
		[InlineData(0xFF, 0xFF, 0xFF, false, true)]
		#endregion
		public void And8Test(byte left, byte right, byte result, bool zero, bool sign)
		{
			_cpu.And(new Register8(left), new Register8(right)).Should().Be(new Register8(result));
			_cpu.Eflags.Carry.Should().BeFalse();
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().BeFalse();
		}

		[Theory]
		#region
		[InlineData(0x8000, 0x8000, 0x8000, false, true)]
		[InlineData(0x8000, 0x0080, 0x0000, true, false)]
		[InlineData(0x8000, 0x0000, 0x0000, true, false)]
		[InlineData(0x8000, 0x007F, 0x0000, true, false)]
		[InlineData(0x8000, 0x00FF, 0x0000, true, false)]
		[InlineData(0x8000, 0x7FFF, 0x0000, true, false)]
		[InlineData(0x8000, 0xFFFF, 0x8000, false, true)]
		[InlineData(0x0080, 0x8000, 0x0000, true, false)]
		[InlineData(0x0080, 0x0080, 0x0080, false, false)]
		[InlineData(0x0080, 0x0000, 0x0000, true, false)]
		[InlineData(0x0080, 0x007F, 0x0000, true, false)]
		[InlineData(0x0080, 0x00FF, 0x0080, false, false)]
		[InlineData(0x0080, 0x7FFF, 0x0080, false, false)]
		[InlineData(0x0080, 0xFFFF, 0x0080, false, false)]
		[InlineData(0x0000, 0x8000, 0x0000, true, false)]
		[InlineData(0x0000, 0x0080, 0x0000, true, false)]
		[InlineData(0x0000, 0x0000, 0x0000, true, false)]
		[InlineData(0x0000, 0x007F, 0x0000, true, false)]
		[InlineData(0x0000, 0x00FF, 0x0000, true, false)]
		[InlineData(0x0000, 0x7FFF, 0x0000, true, false)]
		[InlineData(0x0000, 0xFFFF, 0x0000, true, false)]
		[InlineData(0x007F, 0x8000, 0x0000, true, false)]
		[InlineData(0x007F, 0x0080, 0x0000, true, false)]
		[InlineData(0x007F, 0x0000, 0x0000, true, false)]
		[InlineData(0x007F, 0x007F, 0x007F, false, false)]
		[InlineData(0x007F, 0x00FF, 0x007F, false, false)]
		[InlineData(0x007F, 0x7FFF, 0x007F, false, false)]
		[InlineData(0x007F, 0xFFFF, 0x007F, false, false)]
		[InlineData(0x00FF, 0x8000, 0x0000, true, false)]
		[InlineData(0x00FF, 0x0080, 0x0080, false, false)]
		[InlineData(0x00FF, 0x0000, 0x0000, true, false)]
		[InlineData(0x00FF, 0x007F, 0x007F, false, false)]
		[InlineData(0x00FF, 0x00FF, 0x00FF, false, false)]
		[InlineData(0x00FF, 0x7FFF, 0x00FF, false, false)]
		[InlineData(0x00FF, 0xFFFF, 0x00FF, false, false)]
		[InlineData(0x7FFF, 0x8000, 0x0000, true, false)]
		[InlineData(0x7FFF, 0x0080, 0x0080, false, false)]
		[InlineData(0x7FFF, 0x0000, 0x0000, true, false)]
		[InlineData(0x7FFF, 0x007F, 0x007F, false, false)]
		[InlineData(0x7FFF, 0x00FF, 0x00FF, false, false)]
		[InlineData(0x7FFF, 0x7FFF, 0x7FFF, false, false)]
		[InlineData(0x7FFF, 0xFFFF, 0x7FFF, false, false)]
		[InlineData(0xFFFF, 0x8000, 0x8000, false, true)]
		[InlineData(0xFFFF, 0x0080, 0x0080, false, false)]
		[InlineData(0xFFFF, 0x0000, 0x0000, true, false)]
		[InlineData(0xFFFF, 0x007F, 0x007F, false, false)]
		[InlineData(0xFFFF, 0x00FF, 0x00FF, false, false)]
		[InlineData(0xFFFF, 0x7FFF, 0x7FFF, false, false)]
		[InlineData(0xFFFF, 0xFFFF, 0xFFFF, false, true)]
		#endregion
		public void And16Test(ushort left, ushort right, ushort result, bool zero, bool sign)
		{
			_cpu.And(new Register16(left), new Register16(right)).Should().Be(new Register16(result));
			_cpu.Eflags.Carry.Should().BeFalse();
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().BeFalse();
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80000000, 0x80000000, false, true)]
		[InlineData(0x80000000, 0x00008000, 0x00000000, true, false)]
		[InlineData(0x80000000, 0x00000080, 0x00000000, true, false)]
		[InlineData(0x80000000, 0x00000000, 0x00000000, true, false)]
		[InlineData(0x80000000, 0x0000007F, 0x00000000, true, false)]
		[InlineData(0x80000000, 0x000000FF, 0x00000000, true, false)]
		[InlineData(0x80000000, 0x00007FFF, 0x00000000, true, false)]
		[InlineData(0x80000000, 0x0000FFFF, 0x00000000, true, false)]
		[InlineData(0x80000000, 0x7FFFFFFF, 0x00000000, true, false)]
		[InlineData(0x80000000, 0xFFFFFFFF, 0x80000000, false, true)]
		[InlineData(0x00008000, 0x80000000, 0x00000000, true, false)]
		[InlineData(0x00008000, 0x00008000, 0x00008000, false, false)]
		[InlineData(0x00008000, 0x00000080, 0x00000000, true, false)]
		[InlineData(0x00008000, 0x00000000, 0x00000000, true, false)]
		[InlineData(0x00008000, 0x0000007F, 0x00000000, true, false)]
		[InlineData(0x00008000, 0x000000FF, 0x00000000, true, false)]
		[InlineData(0x00008000, 0x00007FFF, 0x00000000, true, false)]
		[InlineData(0x00008000, 0x0000FFFF, 0x00008000, false, false)]
		[InlineData(0x00008000, 0x7FFFFFFF, 0x00008000, false, false)]
		[InlineData(0x00008000, 0xFFFFFFFF, 0x00008000, false, false)]
		[InlineData(0x00000080, 0x80000000, 0x00000000, true, false)]
		[InlineData(0x00000080, 0x00008000, 0x00000000, true, false)]
		[InlineData(0x00000080, 0x00000080, 0x00000080, false, false)]
		[InlineData(0x00000080, 0x00000000, 0x00000000, true, false)]
		[InlineData(0x00000080, 0x0000007F, 0x00000000, true, false)]
		[InlineData(0x00000080, 0x000000FF, 0x00000080, false, false)]
		[InlineData(0x00000080, 0x00007FFF, 0x00000080, false, false)]
		[InlineData(0x00000080, 0x0000FFFF, 0x00000080, false, false)]
		[InlineData(0x00000080, 0x7FFFFFFF, 0x00000080, false, false)]
		[InlineData(0x00000080, 0xFFFFFFFF, 0x00000080, false, false)]
		[InlineData(0x00000000, 0x80000000, 0x00000000, true, false)]
		[InlineData(0x00000000, 0x00008000, 0x00000000, true, false)]
		[InlineData(0x00000000, 0x00000080, 0x00000000, true, false)]
		[InlineData(0x00000000, 0x00000000, 0x00000000, true, false)]
		[InlineData(0x00000000, 0x0000007F, 0x00000000, true, false)]
		[InlineData(0x00000000, 0x000000FF, 0x00000000, true, false)]
		[InlineData(0x00000000, 0x00007FFF, 0x00000000, true, false)]
		[InlineData(0x00000000, 0x0000FFFF, 0x00000000, true, false)]
		[InlineData(0x00000000, 0x7FFFFFFF, 0x00000000, true, false)]
		[InlineData(0x00000000, 0xFFFFFFFF, 0x00000000, true, false)]
		[InlineData(0x0000007F, 0x80000000, 0x00000000, true, false)]
		[InlineData(0x0000007F, 0x00008000, 0x00000000, true, false)]
		[InlineData(0x0000007F, 0x00000080, 0x00000000, true, false)]
		[InlineData(0x0000007F, 0x00000000, 0x00000000, true, false)]
		[InlineData(0x0000007F, 0x0000007F, 0x0000007F, false, false)]
		[InlineData(0x0000007F, 0x000000FF, 0x0000007F, false, false)]
		[InlineData(0x0000007F, 0x00007FFF, 0x0000007F, false, false)]
		[InlineData(0x0000007F, 0x0000FFFF, 0x0000007F, false, false)]
		[InlineData(0x0000007F, 0x7FFFFFFF, 0x0000007F, false, false)]
		[InlineData(0x0000007F, 0xFFFFFFFF, 0x0000007F, false, false)]
		[InlineData(0x000000FF, 0x80000000, 0x00000000, true, false)]
		[InlineData(0x000000FF, 0x00008000, 0x00000000, true, false)]
		[InlineData(0x000000FF, 0x00000080, 0x00000080, false, false)]
		[InlineData(0x000000FF, 0x00000000, 0x00000000, true, false)]
		[InlineData(0x000000FF, 0x0000007F, 0x0000007F, false, false)]
		[InlineData(0x000000FF, 0x000000FF, 0x000000FF, false, false)]
		[InlineData(0x000000FF, 0x00007FFF, 0x000000FF, false, false)]
		[InlineData(0x000000FF, 0x0000FFFF, 0x000000FF, false, false)]
		[InlineData(0x000000FF, 0x7FFFFFFF, 0x000000FF, false, false)]
		[InlineData(0x000000FF, 0xFFFFFFFF, 0x000000FF, false, false)]
		[InlineData(0x00007FFF, 0x80000000, 0x00000000, true, false)]
		[InlineData(0x00007FFF, 0x00008000, 0x00000000, true, false)]
		[InlineData(0x00007FFF, 0x00000080, 0x00000080, false, false)]
		[InlineData(0x00007FFF, 0x00000000, 0x00000000, true, false)]
		[InlineData(0x00007FFF, 0x0000007F, 0x0000007F, false, false)]
		[InlineData(0x00007FFF, 0x000000FF, 0x000000FF, false, false)]
		[InlineData(0x00007FFF, 0x00007FFF, 0x00007FFF, false, false)]
		[InlineData(0x00007FFF, 0x0000FFFF, 0x00007FFF, false, false)]
		[InlineData(0x00007FFF, 0x7FFFFFFF, 0x00007FFF, false, false)]
		[InlineData(0x00007FFF, 0xFFFFFFFF, 0x00007FFF, false, false)]
		[InlineData(0x0000FFFF, 0x80000000, 0x00000000, true, false)]
		[InlineData(0x0000FFFF, 0x00008000, 0x00008000, false, false)]
		[InlineData(0x0000FFFF, 0x00000080, 0x00000080, false, false)]
		[InlineData(0x0000FFFF, 0x00000000, 0x00000000, true, false)]
		[InlineData(0x0000FFFF, 0x0000007F, 0x0000007F, false, false)]
		[InlineData(0x0000FFFF, 0x000000FF, 0x000000FF, false, false)]
		[InlineData(0x0000FFFF, 0x00007FFF, 0x00007FFF, false, false)]
		[InlineData(0x0000FFFF, 0x0000FFFF, 0x0000FFFF, false, false)]
		[InlineData(0x0000FFFF, 0x7FFFFFFF, 0x0000FFFF, false, false)]
		[InlineData(0x0000FFFF, 0xFFFFFFFF, 0x0000FFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x80000000, 0x00000000, true, false)]
		[InlineData(0x7FFFFFFF, 0x00008000, 0x00008000, false, false)]
		[InlineData(0x7FFFFFFF, 0x00000080, 0x00000080, false, false)]
		[InlineData(0x7FFFFFFF, 0x00000000, 0x00000000, true, false)]
		[InlineData(0x7FFFFFFF, 0x0000007F, 0x0000007F, false, false)]
		[InlineData(0x7FFFFFFF, 0x000000FF, 0x000000FF, false, false)]
		[InlineData(0x7FFFFFFF, 0x00007FFF, 0x00007FFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x0000FFFF, 0x0000FFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x7FFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0xFFFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0xFFFFFFFF, 0x80000000, 0x80000000, false, true)]
		[InlineData(0xFFFFFFFF, 0x00008000, 0x00008000, false, false)]
		[InlineData(0xFFFFFFFF, 0x00000080, 0x00000080, false, false)]
		[InlineData(0xFFFFFFFF, 0x00000000, 0x00000000, true, false)]
		[InlineData(0xFFFFFFFF, 0x0000007F, 0x0000007F, false, false)]
		[InlineData(0xFFFFFFFF, 0x000000FF, 0x000000FF, false, false)]
		[InlineData(0xFFFFFFFF, 0x00007FFF, 0x00007FFF, false, false)]
		[InlineData(0xFFFFFFFF, 0x0000FFFF, 0x0000FFFF, false, false)]
		[InlineData(0xFFFFFFFF, 0x7FFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, false, true)]
		#endregion
		public void And32Test(uint left, uint right, uint result, bool zero, bool sign)
		{
			_cpu.And(new Register32(left), new Register32(right)).Should().Be(new Register32(result));
			_cpu.Eflags.Carry.Should().BeFalse();
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().BeFalse();
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80, 0x80000000)]
		[InlineData(0x80000000, 0x00, 0x00000000)]
		[InlineData(0x80000000, 0x7F, 0x00000000)]
		[InlineData(0x80000000, 0xFF, 0x80000000)]
		[InlineData(0x00008000, 0x80, 0x00008000)]
		[InlineData(0x00008000, 0x00, 0x00000000)]
		[InlineData(0x00008000, 0x7F, 0x00000000)]
		[InlineData(0x00008000, 0xFF, 0x00008000)]
		[InlineData(0x00000080, 0x80, 0x00000080)]
		[InlineData(0x00000080, 0x00, 0x00000000)]
		[InlineData(0x00000080, 0x7F, 0x00000000)]
		[InlineData(0x00000080, 0xFF, 0x00000080)]
		[InlineData(0x00000000, 0x80, 0x00000000)]
		[InlineData(0x00000000, 0x00, 0x00000000)]
		[InlineData(0x00000000, 0x7F, 0x00000000)]
		[InlineData(0x00000000, 0xFF, 0x00000000)]
		[InlineData(0x0000007F, 0x80, 0x00000000)]
		[InlineData(0x0000007F, 0x00, 0x00000000)]
		[InlineData(0x0000007F, 0x7F, 0x0000007F)]
		[InlineData(0x0000007F, 0xFF, 0x0000007F)]
		[InlineData(0x000000FF, 0x80, 0x00000080)]
		[InlineData(0x000000FF, 0x00, 0x00000000)]
		[InlineData(0x000000FF, 0x7F, 0x0000007F)]
		[InlineData(0x000000FF, 0xFF, 0x000000FF)]
		[InlineData(0x00007FFF, 0x80, 0x00007F80)]
		[InlineData(0x00007FFF, 0x00, 0x00000000)]
		[InlineData(0x00007FFF, 0x7F, 0x0000007F)]
		[InlineData(0x00007FFF, 0xFF, 0x00007FFF)]
		[InlineData(0x0000FFFF, 0x80, 0x0000FF80)]
		[InlineData(0x0000FFFF, 0x00, 0x00000000)]
		[InlineData(0x0000FFFF, 0x7F, 0x0000007F)]
		[InlineData(0x0000FFFF, 0xFF, 0x0000FFFF)]
		[InlineData(0x7FFFFFFF, 0x80, 0x7FFFFF80)]
		[InlineData(0x7FFFFFFF, 0x00, 0x00000000)]
		[InlineData(0x7FFFFFFF, 0x7F, 0x0000007F)]
		[InlineData(0x7FFFFFFF, 0xFF, 0x7FFFFFFF)]
		[InlineData(0xFFFFFFFF, 0x80, 0xFFFFFF80)]
		[InlineData(0xFFFFFFFF, 0x00, 0x00000000)]
		[InlineData(0xFFFFFFFF, 0x7F, 0x0000007F)]
		[InlineData(0xFFFFFFFF, 0xFF, 0xFFFFFFFF)]
		#endregion
		public void And32_8Test(uint left, byte right, uint result)
		{
			_cpu.And(new Register32(left), new Register8(right)).Should().Be(new Register32(result));
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x_FFFFFFFF_80000000)]
		[InlineData(0x00008000, 0x_00000000_00008000)]
		[InlineData(0x00000080, 0x_00000000_00000080)]
		[InlineData(0x00000000, 0x_00000000_00000000)]
		[InlineData(0x0000007F, 0x_00000000_0000007F)]
		[InlineData(0x000000FF, 0x_00000000_000000FF)]
		[InlineData(0x00007FFF, 0x_00000000_00007FFF)]
		[InlineData(0x0000FFFF, 0x_00000000_0000FFFF)]
		[InlineData(0x7FFFFFFF, 0x_00000000_7FFFFFFF)]
		[InlineData(0xFFFFFFFF, 0x_FFFFFFFF_FFFFFFFF)]
		#endregion
		public void CdqTest(uint value, ulong result)
		{
			_cpu.Eax = new Register32(value);
			_cpu.Cdq();
			_cpu.Edx.Should().Be(new Register64(result).High);
			_cpu.Eax.Should().Be(new Register64(result).Low);
		}

		[Theory]
		#region
		[InlineData(0x8000, 0x8000, false, true, false, false)]
		[InlineData(0x8000, 0x0080, false, false, false, true)]
		[InlineData(0x8000, 0x0000, false, false, true, false)]
		[InlineData(0x8000, 0x007F, false, false, false, true)]
		[InlineData(0x8000, 0x00FF, false, false, false, true)]
		[InlineData(0x8000, 0x7FFF, false, false, false, true)]
		[InlineData(0x8000, 0xFFFF, true, false, true, false)]
		[InlineData(0x0080, 0x8000, true, false, true, true)]
		[InlineData(0x0080, 0x0080, false, true, false, false)]
		[InlineData(0x0080, 0x0000, false, false, false, false)]
		[InlineData(0x0080, 0x007F, false, false, false, false)]
		[InlineData(0x0080, 0x00FF, true, false, true, false)]
		[InlineData(0x0080, 0x7FFF, true, false, true, false)]
		[InlineData(0x0080, 0xFFFF, true, false, false, false)]
		[InlineData(0x0000, 0x8000, true, false, true, true)]
		[InlineData(0x0000, 0x0080, true, false, true, false)]
		[InlineData(0x0000, 0x0000, false, true, false, false)]
		[InlineData(0x0000, 0x007F, true, false, true, false)]
		[InlineData(0x0000, 0x00FF, true, false, true, false)]
		[InlineData(0x0000, 0x7FFF, true, false, true, false)]
		[InlineData(0x0000, 0xFFFF, true, false, false, false)]
		[InlineData(0x007F, 0x8000, true, false, true, true)]
		[InlineData(0x007F, 0x0080, true, false, true, false)]
		[InlineData(0x007F, 0x0000, false, false, false, false)]
		[InlineData(0x007F, 0x007F, false, true, false, false)]
		[InlineData(0x007F, 0x00FF, true, false, true, false)]
		[InlineData(0x007F, 0x7FFF, true, false, true, false)]
		[InlineData(0x007F, 0xFFFF, true, false, false, false)]
		[InlineData(0x00FF, 0x8000, true, false, true, true)]
		[InlineData(0x00FF, 0x0080, false, false, false, false)]
		[InlineData(0x00FF, 0x0000, false, false, false, false)]
		[InlineData(0x00FF, 0x007F, false, false, false, false)]
		[InlineData(0x00FF, 0x00FF, false, true, false, false)]
		[InlineData(0x00FF, 0x7FFF, true, false, true, false)]
		[InlineData(0x00FF, 0xFFFF, true, false, false, false)]
		[InlineData(0x7FFF, 0x8000, true, false, true, true)]
		[InlineData(0x7FFF, 0x0080, false, false, false, false)]
		[InlineData(0x7FFF, 0x0000, false, false, false, false)]
		[InlineData(0x7FFF, 0x007F, false, false, false, false)]
		[InlineData(0x7FFF, 0x00FF, false, false, false, false)]
		[InlineData(0x7FFF, 0x7FFF, false, true, false, false)]
		[InlineData(0x7FFF, 0xFFFF, true, false, true, true)]
		[InlineData(0xFFFF, 0x8000, false, false, false, false)]
		[InlineData(0xFFFF, 0x0080, false, false, true, false)]
		[InlineData(0xFFFF, 0x0000, false, false, true, false)]
		[InlineData(0xFFFF, 0x007F, false, false, true, false)]
		[InlineData(0xFFFF, 0x00FF, false, false, true, false)]
		[InlineData(0xFFFF, 0x7FFF, false, false, true, false)]
		[InlineData(0xFFFF, 0xFFFF, false, true, false, false)]
		#endregion
		public void Cmp16Test(ushort left, ushort right, bool carry, bool zero, bool sign, bool overflow)
		{
			_cpu.Cmp(new Register16(left), new Register16(right));
			_cpu.Eflags.Carry.Should().Be(carry);
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().Be(overflow);
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80000000, false, true, false, false)]
		[InlineData(0x80000000, 0x00008000, false, false, false, true)]
		[InlineData(0x80000000, 0x00000080, false, false, false, true)]
		[InlineData(0x80000000, 0x00000000, false, false, true, false)]
		[InlineData(0x80000000, 0x0000007F, false, false, false, true)]
		[InlineData(0x80000000, 0x000000FF, false, false, false, true)]
		[InlineData(0x80000000, 0x00007FFF, false, false, false, true)]
		[InlineData(0x80000000, 0x0000FFFF, false, false, false, true)]
		[InlineData(0x80000000, 0x7FFFFFFF, false, false, false, true)]
		[InlineData(0x80000000, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(0x00008000, 0x80000000, true, false, true, true)]
		[InlineData(0x00008000, 0x00008000, false, true, false, false)]
		[InlineData(0x00008000, 0x00000080, false, false, false, false)]
		[InlineData(0x00008000, 0x00000000, false, false, false, false)]
		[InlineData(0x00008000, 0x0000007F, false, false, false, false)]
		[InlineData(0x00008000, 0x000000FF, false, false, false, false)]
		[InlineData(0x00008000, 0x00007FFF, false, false, false, false)]
		[InlineData(0x00008000, 0x0000FFFF, true, false, true, false)]
		[InlineData(0x00008000, 0x7FFFFFFF, true, false, true, false)]
		[InlineData(0x00008000, 0xFFFFFFFF, true, false, false, false)]
		[InlineData(0x00000080, 0x80000000, true, false, true, true)]
		[InlineData(0x00000080, 0x00008000, true, false, true, false)]
		[InlineData(0x00000080, 0x00000080, false, true, false, false)]
		[InlineData(0x00000080, 0x00000000, false, false, false, false)]
		[InlineData(0x00000080, 0x0000007F, false, false, false, false)]
		[InlineData(0x00000080, 0x000000FF, true, false, true, false)]
		[InlineData(0x00000080, 0x00007FFF, true, false, true, false)]
		[InlineData(0x00000080, 0x0000FFFF, true, false, true, false)]
		[InlineData(0x00000080, 0x7FFFFFFF, true, false, true, false)]
		[InlineData(0x00000080, 0xFFFFFFFF, true, false, false, false)]
		[InlineData(0x00000000, 0x80000000, true, false, true, true)]
		[InlineData(0x00000000, 0x00008000, true, false, true, false)]
		[InlineData(0x00000000, 0x00000080, true, false, true, false)]
		[InlineData(0x00000000, 0x00000000, false, true, false, false)]
		[InlineData(0x00000000, 0x0000007F, true, false, true, false)]
		[InlineData(0x00000000, 0x000000FF, true, false, true, false)]
		[InlineData(0x00000000, 0x00007FFF, true, false, true, false)]
		[InlineData(0x00000000, 0x0000FFFF, true, false, true, false)]
		[InlineData(0x00000000, 0x7FFFFFFF, true, false, true, false)]
		[InlineData(0x00000000, 0xFFFFFFFF, true, false, false, false)]
		[InlineData(0x0000007F, 0x80000000, true, false, true, true)]
		[InlineData(0x0000007F, 0x00008000, true, false, true, false)]
		[InlineData(0x0000007F, 0x00000080, true, false, true, false)]
		[InlineData(0x0000007F, 0x00000000, false, false, false, false)]
		[InlineData(0x0000007F, 0x0000007F, false, true, false, false)]
		[InlineData(0x0000007F, 0x000000FF, true, false, true, false)]
		[InlineData(0x0000007F, 0x00007FFF, true, false, true, false)]
		[InlineData(0x0000007F, 0x0000FFFF, true, false, true, false)]
		[InlineData(0x0000007F, 0x7FFFFFFF, true, false, true, false)]
		[InlineData(0x0000007F, 0xFFFFFFFF, true, false, false, false)]
		[InlineData(0x000000FF, 0x80000000, true, false, true, true)]
		[InlineData(0x000000FF, 0x00008000, true, false, true, false)]
		[InlineData(0x000000FF, 0x00000080, false, false, false, false)]
		[InlineData(0x000000FF, 0x00000000, false, false, false, false)]
		[InlineData(0x000000FF, 0x0000007F, false, false, false, false)]
		[InlineData(0x000000FF, 0x000000FF, false, true, false, false)]
		[InlineData(0x000000FF, 0x00007FFF, true, false, true, false)]
		[InlineData(0x000000FF, 0x0000FFFF, true, false, true, false)]
		[InlineData(0x000000FF, 0x7FFFFFFF, true, false, true, false)]
		[InlineData(0x000000FF, 0xFFFFFFFF, true, false, false, false)]
		[InlineData(0x00007FFF, 0x80000000, true, false, true, true)]
		[InlineData(0x00007FFF, 0x00008000, true, false, true, false)]
		[InlineData(0x00007FFF, 0x00000080, false, false, false, false)]
		[InlineData(0x00007FFF, 0x00000000, false, false, false, false)]
		[InlineData(0x00007FFF, 0x0000007F, false, false, false, false)]
		[InlineData(0x00007FFF, 0x000000FF, false, false, false, false)]
		[InlineData(0x00007FFF, 0x00007FFF, false, true, false, false)]
		[InlineData(0x00007FFF, 0x0000FFFF, true, false, true, false)]
		[InlineData(0x00007FFF, 0x7FFFFFFF, true, false, true, false)]
		[InlineData(0x00007FFF, 0xFFFFFFFF, true, false, false, false)]
		[InlineData(0x0000FFFF, 0x80000000, true, false, true, true)]
		[InlineData(0x0000FFFF, 0x00008000, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x00000080, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x00000000, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x0000007F, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x000000FF, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x00007FFF, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x0000FFFF, false, true, false, false)]
		[InlineData(0x0000FFFF, 0x7FFFFFFF, true, false, true, false)]
		[InlineData(0x0000FFFF, 0xFFFFFFFF, true, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x80000000, true, false, true, true)]
		[InlineData(0x7FFFFFFF, 0x00008000, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x00000080, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x00000000, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x0000007F, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x000000FF, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x00007FFF, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x0000FFFF, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x7FFFFFFF, false, true, false, false)]
		[InlineData(0x7FFFFFFF, 0xFFFFFFFF, true, false, true, true)]
		[InlineData(0xFFFFFFFF, 0x80000000, false, false, false, false)]
		[InlineData(0xFFFFFFFF, 0x00008000, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x00000080, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x00000000, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x0000007F, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x000000FF, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x00007FFF, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x0000FFFF, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x7FFFFFFF, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0xFFFFFFFF, false, true, false, false)]
		#endregion
		public void Cmp32Test(uint left, uint right, bool carry, bool zero, bool sign, bool overflow)
		{
			_cpu.Cmp(new Register32(left), new Register32(right));
			_cpu.Eflags.Carry.Should().Be(carry);
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().Be(overflow);
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x7FFFFFFF, false, false, true)]
		[InlineData(0x00008000, 0x00007FFF, false, false, false)]
		[InlineData(0x00000080, 0x0000007F, false, false, false)]
		[InlineData(0x00000000, 0xFFFFFFFF, false, true, false)]
		[InlineData(0x0000007F, 0x0000007E, false, false, false)]
		[InlineData(0x000000FF, 0x000000FE, false, false, false)]
		[InlineData(0x00007FFF, 0x00007FFE, false, false, false)]
		[InlineData(0x0000FFFF, 0x0000FFFE, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x7FFFFFFE, false, false, false)]
		[InlineData(0xFFFFFFFF, 0xFFFFFFFE, false, true, false)]
		#endregion
		public void Dec32Test(uint value, uint result, bool zero, bool sign, bool overflow)
		{
			_cpu.Dec(new Register32(value)).Should().Be(new Register32(result));
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().Be(overflow);
		}

		[Fact]
		public void FaddTest()
		{
			_cpu.Fadd(Register64.FromDouble(1234), Register64.FromDouble(5678)).ToDouble().Should().Be(1234.0 + 5678.0);
		}

		[Fact]
		public void FchsTest()
		{
			_cpu.Fchs(Register64.FromDouble(1234.0)).ToDouble().Should().Be(-1234.0);
		}

		[Fact]
		public void FcomTest()
		{
			_cpu.Fpu.Stack.Push(Register64.FromDouble(0));

			_cpu.Fcom(Register64.FromDouble(-1));
			_cpu.Fpu.Status.C3.Should().BeFalse();
			_cpu.Fpu.Status.C2.Should().BeFalse();
			_cpu.Fpu.Status.C0.Should().BeFalse();

			_cpu.Fcom(Register64.FromDouble(0));
			_cpu.Fpu.Status.C3.Should().BeTrue();
			_cpu.Fpu.Status.C2.Should().BeFalse();
			_cpu.Fpu.Status.C0.Should().BeFalse();

			_cpu.Fcom(Register64.FromDouble(1));
			_cpu.Fpu.Status.C3.Should().BeFalse();
			_cpu.Fpu.Status.C2.Should().BeFalse();
			_cpu.Fpu.Status.C0.Should().BeTrue();
		}

		[Fact]
		public void FcosTest()
		{
			_cpu.Fcos(Register64.FromDouble(1234.0)).ToDouble().Should().Be(Math.Cos(1234.0));
		}

		[Fact]
		public void FdivTest()
		{
			_cpu.Fdiv(Register64.FromDouble(1234.0), Register64.FromDouble(5678.0)).ToDouble().Should().Be(1234.0 / 5678.0);
		}
		
		[Fact]
		public void FdivrTest()
		{
			_cpu.Fdivr(Register64.FromDouble(1234.0), Register64.FromDouble(5678.0)).ToDouble().Should().Be(5678.0 / 1234.0);
		}

		[Fact]
		public void FidivTest()
		{
			_cpu.Fidiv(Register64.FromDouble(1234.0), new Register32(5678)).ToDouble().Should().Be(1234.0 / 5678);
		}

		[Fact]
		public void FildTest()
		{
			_cpu.Fild(new Register32(1234));
			_cpu.Fpu.Stack.Pop().ToDouble().Should().Be(1234.0);
		}

		[Fact]
		public void FimulTest()
		{
			_cpu.Fimul(Register64.FromDouble(1234.0), new Register32(5678)).ToDouble().Should().Be(1234.0 * 5678);
		}

		[Fact]
		public void FldTest()
		{
			_cpu.Fld(Register64.FromDouble(1234.0));
			_cpu.Fpu.Stack.Pop().ToDouble().Should().Be(1234.0);
		}

		[Fact]
		public void FpatanTest()
		{
			_cpu.Fpatan(Register64.FromDouble(1234.0), Register64.FromDouble(5678.0)).ToDouble().Should().Be(Math.Atan2(1234.0, 5678.0));
		}

		[Fact]
		public void FmulTest()
		{
			_cpu.Fmul(Register64.FromDouble(1234.0), Register64.FromDouble(5678.0)).ToDouble().Should().Be(1234.0 * 5678.0);
			_cpu.Fmul(Register64.FromDouble(1234.0), Register32.FromSingle(5678.0f)).ToDouble().Should().Be(1234.0 * 5678.0f);
		}

		[Fact]
		public void FsinTest()
		{
			_cpu.Fsin(Register64.FromDouble(1234.0)).ToDouble().Should().Be(Math.Sin(1234.0));
		}

		[Fact]
		public void FsubTest()
		{
			_cpu.Fsub(Register64.FromDouble(1234.0), Register64.FromDouble(5678.0)).ToDouble().Should().Be(1234.0 - 5678.0);
		}

		[Fact]
		public void FsubrTest()
		{
			_cpu.Fsubr(Register64.FromDouble(1234.0), Register64.FromDouble(5678.0)).ToDouble().Should().Be(5678.0 - 1234.0);
		}

		[Fact]
		public void FxchTest()
		{
			_cpu.Fxch(Register64.FromDouble(1234.0), Register64.FromDouble(5678.0)).Should().Be((Register64.FromDouble(5678.0), Register64.FromDouble(1234.0)));
		}

		[Theory]
		#region
		[InlineData(-16777216, -268435456, 0, -16777216)]
		[InlineData(-16777216, 268435456, 0, -16777216)]
		[InlineData(16777216, -268435456, 0, 16777216)]
		[InlineData(16777216, 268435456, 0, 16777216)]
		[InlineData(-72057594037927936, -268435456, 268435456, 0)]
		[InlineData(-72057594037927936, 268435456, -268435456, 0)]
		[InlineData(72057594037927936, -268435456, -268435456, 0)]
		[InlineData(72057594037927936, 268435456, 268435456, 0)]
		#endregion
		public void Idiv32Test(long dividend, int divisor, int quotient, int remainder)
		{
			_cpu.Edx = new Register64((ulong)dividend).High;
			_cpu.Eax = new Register64((ulong)dividend).Low;
			_cpu.Idiv(new Register32((uint)divisor));
			_cpu.Eax.Should().Be(new Register32((uint)quotient));
			_cpu.Edx.Should().Be(new Register32((uint)remainder));
		}

		[Theory]
		#region
		[InlineData(0x_FFFFFFFF_7FFFFFFF, 0x00000001)]
		[InlineData(0x_00000000_80000000, 0x00000001)]
		#endregion
		public void Idiv32Test_ArithmeticException(ulong dividend, uint divisor)
		{
			_cpu.Edx = new Register64(dividend).High;
			_cpu.Eax = new Register64(dividend).Low;
			_cpu.Invoking(cpu => cpu.Idiv(new Register32(divisor))).Should().Throw<ArithmeticException>();
		}

		[Theory]
		#region
		[InlineData(0x_80000000_00000000)]
		[InlineData(0x_00000000_80000000)]
		[InlineData(0x_00000000_00008000)]
		[InlineData(0x_00000000_00000080)]
		[InlineData(0x_00000000_00000000)]
		[InlineData(0x_00000000_0000007F)]
		[InlineData(0x_00000000_000000FF)]
		[InlineData(0x_00000000_00007FFF)]
		[InlineData(0x_00000000_0000FFFF)]
		[InlineData(0x_00000000_7FFFFFFF)]
		[InlineData(0x_00000000_FFFFFFFF)]
		[InlineData(0x_7FFFFFFF_FFFFFFFF)]
		[InlineData(0x_FFFFFFFF_FFFFFFFF)]
		#endregion
		public void Idiv32Test_DivideByZeroException(ulong dividend)
		{
			_cpu.Edx = new Register64(dividend).High;
			_cpu.Eax = new Register64(dividend).Low;
			_cpu.Invoking(cpu => cpu.Idiv(Register32.Empty)).Should().Throw<DivideByZeroException>();
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80000000, 0x_40000000_00000000, true, true)]
		[InlineData(0x80000000, 0x00008000, 0x_FFFFC000_00000000, true, true)]
		[InlineData(0x80000000, 0x00000080, 0x_FFFFFFC0_00000000, true, true)]
		[InlineData(0x80000000, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x80000000, 0x0000007F, 0x_FFFFFFC0_80000000, true, true)]
		[InlineData(0x80000000, 0x000000FF, 0x_FFFFFF80_80000000, true, true)]
		[InlineData(0x80000000, 0x00007FFF, 0x_FFFFC000_80000000, true, true)]
		[InlineData(0x80000000, 0x0000FFFF, 0x_FFFF8000_80000000, true, true)]
		[InlineData(0x80000000, 0x7FFFFFFF, 0x_C0000000_80000000, true, true)]
		[InlineData(0x80000000, 0xFFFFFFFF, 0x_00000000_80000000, true, true)]
		[InlineData(0x00008000, 0x80000000, 0x_FFFFC000_00000000, true, true)]
		[InlineData(0x00008000, 0x00008000, 0x_00000000_40000000, false, false)]
		[InlineData(0x00008000, 0x00000080, 0x_00000000_00400000, false, false)]
		[InlineData(0x00008000, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x00008000, 0x0000007F, 0x_00000000_003F8000, false, false)]
		[InlineData(0x00008000, 0x000000FF, 0x_00000000_007F8000, false, false)]
		[InlineData(0x00008000, 0x00007FFF, 0x_00000000_3FFF8000, false, false)]
		[InlineData(0x00008000, 0x0000FFFF, 0x_00000000_7FFF8000, false, false)]
		[InlineData(0x00008000, 0x7FFFFFFF, 0x_00003FFF_FFFF8000, true, true)]
		[InlineData(0x00008000, 0xFFFFFFFF, 0x_FFFFFFFF_FFFF8000, false, false)]
		[InlineData(0x00000080, 0x80000000, 0x_FFFFFFC0_00000000, true, true)]
		[InlineData(0x00000080, 0x00008000, 0x_00000000_00400000, false, false)]
		[InlineData(0x00000080, 0x00000080, 0x_00000000_00004000, false, false)]
		[InlineData(0x00000080, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000080, 0x0000007F, 0x_00000000_00003F80, false, false)]
		[InlineData(0x00000080, 0x000000FF, 0x_00000000_00007F80, false, false)]
		[InlineData(0x00000080, 0x00007FFF, 0x_00000000_003FFF80, false, false)]
		[InlineData(0x00000080, 0x0000FFFF, 0x_00000000_007FFF80, false, false)]
		[InlineData(0x00000080, 0x7FFFFFFF, 0x_0000003F_FFFFFF80, true, true)]
		[InlineData(0x00000080, 0xFFFFFFFF, 0x_FFFFFFFF_FFFFFF80, false, false)]
		[InlineData(0x00000000, 0x80000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x00008000, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x00000080, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x0000007F, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x000000FF, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x00007FFF, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x0000FFFF, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x7FFFFFFF, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0xFFFFFFFF, 0x_00000000_00000000, false, false)]
		[InlineData(0x0000007F, 0x80000000, 0x_FFFFFFC0_80000000, true, true)]
		[InlineData(0x0000007F, 0x00008000, 0x_00000000_003F8000, false, false)]
		[InlineData(0x0000007F, 0x00000080, 0x_00000000_00003F80, false, false)]
		[InlineData(0x0000007F, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x0000007F, 0x0000007F, 0x_00000000_00003F01, false, false)]
		[InlineData(0x0000007F, 0x000000FF, 0x_00000000_00007E81, false, false)]
		[InlineData(0x0000007F, 0x00007FFF, 0x_00000000_003F7F81, false, false)]
		[InlineData(0x0000007F, 0x0000FFFF, 0x_00000000_007EFF81, false, false)]
		[InlineData(0x0000007F, 0x7FFFFFFF, 0x_0000003F_7FFFFF81, true, true)]
		[InlineData(0x0000007F, 0xFFFFFFFF, 0x_FFFFFFFF_FFFFFF81, false, false)]
		[InlineData(0x000000FF, 0x80000000, 0x_FFFFFF80_80000000, true, true)]
		[InlineData(0x000000FF, 0x00008000, 0x_00000000_007F8000, false, false)]
		[InlineData(0x000000FF, 0x00000080, 0x_00000000_00007F80, false, false)]
		[InlineData(0x000000FF, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x000000FF, 0x0000007F, 0x_00000000_00007E81, false, false)]
		[InlineData(0x000000FF, 0x000000FF, 0x_00000000_0000FE01, false, false)]
		[InlineData(0x000000FF, 0x00007FFF, 0x_00000000_007F7F01, false, false)]
		[InlineData(0x000000FF, 0x0000FFFF, 0x_00000000_00FEFF01, false, false)]
		[InlineData(0x000000FF, 0x7FFFFFFF, 0x_0000007F_7FFFFF01, true, true)]
		[InlineData(0x000000FF, 0xFFFFFFFF, 0x_FFFFFFFF_FFFFFF01, false, false)]
		[InlineData(0x00007FFF, 0x80000000, 0x_FFFFC000_80000000, true, true)]
		[InlineData(0x00007FFF, 0x00008000, 0x_00000000_3FFF8000, false, false)]
		[InlineData(0x00007FFF, 0x00000080, 0x_00000000_003FFF80, false, false)]
		[InlineData(0x00007FFF, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x00007FFF, 0x0000007F, 0x_00000000_003F7F81, false, false)]
		[InlineData(0x00007FFF, 0x000000FF, 0x_00000000_007F7F01, false, false)]
		[InlineData(0x00007FFF, 0x00007FFF, 0x_00000000_3FFF0001, false, false)]
		[InlineData(0x00007FFF, 0x0000FFFF, 0x_00000000_7FFE8001, false, false)]
		[InlineData(0x00007FFF, 0x7FFFFFFF, 0x_00003FFF_7FFF8001, true, true)]
		[InlineData(0x00007FFF, 0xFFFFFFFF, 0x_FFFFFFFF_FFFF8001, false, false)]
		[InlineData(0x0000FFFF, 0x80000000, 0x_FFFF8000_80000000, true, true)]
		[InlineData(0x0000FFFF, 0x00008000, 0x_00000000_7FFF8000, false, false)]
		[InlineData(0x0000FFFF, 0x00000080, 0x_00000000_007FFF80, false, false)]
		[InlineData(0x0000FFFF, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x0000FFFF, 0x0000007F, 0x_00000000_007EFF81, false, false)]
		[InlineData(0x0000FFFF, 0x000000FF, 0x_00000000_00FEFF01, false, false)]
		[InlineData(0x0000FFFF, 0x00007FFF, 0x_00000000_7FFE8001, false, false)]
		[InlineData(0x0000FFFF, 0x0000FFFF, 0x_00000000_FFFE0001, true, true)]
		[InlineData(0x0000FFFF, 0x7FFFFFFF, 0x_00007FFF_7FFF0001, true, true)]
		[InlineData(0x0000FFFF, 0xFFFFFFFF, 0x_FFFFFFFF_FFFF0001, false, false)]
		[InlineData(0x7FFFFFFF, 0x80000000, 0x_C0000000_80000000, true, true)]
		[InlineData(0x7FFFFFFF, 0x00008000, 0x_00003FFF_FFFF8000, true, true)]
		[InlineData(0x7FFFFFFF, 0x00000080, 0x_0000003F_FFFFFF80, true, true)]
		[InlineData(0x7FFFFFFF, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x7FFFFFFF, 0x0000007F, 0x_0000003F_7FFFFF81, true, true)]
		[InlineData(0x7FFFFFFF, 0x000000FF, 0x_0000007F_7FFFFF01, true, true)]
		[InlineData(0x7FFFFFFF, 0x00007FFF, 0x_00003FFF_7FFF8001, true, true)]
		[InlineData(0x7FFFFFFF, 0x0000FFFF, 0x_00007FFF_7FFF0001, true, true)]
		[InlineData(0x7FFFFFFF, 0x7FFFFFFF, 0x_3FFFFFFF_00000001, true, true)]
		[InlineData(0x7FFFFFFF, 0xFFFFFFFF, 0x_FFFFFFFF_80000001, false, false)]
		[InlineData(0xFFFFFFFF, 0x80000000, 0x_00000000_80000000, true, true)]
		[InlineData(0xFFFFFFFF, 0x00008000, 0x_FFFFFFFF_FFFF8000, false, false)]
		[InlineData(0xFFFFFFFF, 0x00000080, 0x_FFFFFFFF_FFFFFF80, false, false)]
		[InlineData(0xFFFFFFFF, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0xFFFFFFFF, 0x0000007F, 0x_FFFFFFFF_FFFFFF81, false, false)]
		[InlineData(0xFFFFFFFF, 0x000000FF, 0x_FFFFFFFF_FFFFFF01, false, false)]
		[InlineData(0xFFFFFFFF, 0x00007FFF, 0x_FFFFFFFF_FFFF8001, false, false)]
		[InlineData(0xFFFFFFFF, 0x0000FFFF, 0x_FFFFFFFF_FFFF0001, false, false)]
		[InlineData(0xFFFFFFFF, 0x7FFFFFFF, 0x_FFFFFFFF_80000001, false, false)]
		[InlineData(0xFFFFFFFF, 0xFFFFFFFF, 0x_00000000_00000001, false, false)]
		#endregion
		public void Imul32OneOperandTest(uint left, uint right, ulong result, bool carry, bool overflow)
		{
			_cpu.Eax = new Register32(left);
			_cpu.Imul(new Register32(right));
			_cpu.Edx.Should().Be(new Register64(result).High);
			_cpu.Eax.Should().Be(new Register64(result).Low);
			_cpu.Eflags.Carry.Should().Be(carry);
			_cpu.Eflags.Overflow.Should().Be(overflow);
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80000000, 0x_40000000_00000000, true, true)]
		[InlineData(0x80000000, 0x00008000, 0x_FFFFC000_00000000, true, true)]
		[InlineData(0x80000000, 0x00000080, 0x_FFFFFFC0_00000000, true, true)]
		[InlineData(0x80000000, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x80000000, 0x0000007F, 0x_FFFFFFC0_80000000, true, true)]
		[InlineData(0x80000000, 0x000000FF, 0x_FFFFFF80_80000000, true, true)]
		[InlineData(0x80000000, 0x00007FFF, 0x_FFFFC000_80000000, true, true)]
		[InlineData(0x80000000, 0x0000FFFF, 0x_FFFF8000_80000000, true, true)]
		[InlineData(0x80000000, 0x7FFFFFFF, 0x_C0000000_80000000, true, true)]
		[InlineData(0x80000000, 0xFFFFFFFF, 0x_00000000_80000000, true, true)]
		[InlineData(0x00008000, 0x80000000, 0x_FFFFC000_00000000, true, true)]
		[InlineData(0x00008000, 0x00008000, 0x_00000000_40000000, false, false)]
		[InlineData(0x00008000, 0x00000080, 0x_00000000_00400000, false, false)]
		[InlineData(0x00008000, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x00008000, 0x0000007F, 0x_00000000_003F8000, false, false)]
		[InlineData(0x00008000, 0x000000FF, 0x_00000000_007F8000, false, false)]
		[InlineData(0x00008000, 0x00007FFF, 0x_00000000_3FFF8000, false, false)]
		[InlineData(0x00008000, 0x0000FFFF, 0x_00000000_7FFF8000, false, false)]
		[InlineData(0x00008000, 0x7FFFFFFF, 0x_00003FFF_FFFF8000, true, true)]
		[InlineData(0x00008000, 0xFFFFFFFF, 0x_FFFFFFFF_FFFF8000, false, false)]
		[InlineData(0x00000080, 0x80000000, 0x_FFFFFFC0_00000000, true, true)]
		[InlineData(0x00000080, 0x00008000, 0x_00000000_00400000, false, false)]
		[InlineData(0x00000080, 0x00000080, 0x_00000000_00004000, false, false)]
		[InlineData(0x00000080, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000080, 0x0000007F, 0x_00000000_00003F80, false, false)]
		[InlineData(0x00000080, 0x000000FF, 0x_00000000_00007F80, false, false)]
		[InlineData(0x00000080, 0x00007FFF, 0x_00000000_003FFF80, false, false)]
		[InlineData(0x00000080, 0x0000FFFF, 0x_00000000_007FFF80, false, false)]
		[InlineData(0x00000080, 0x7FFFFFFF, 0x_0000003F_FFFFFF80, true, true)]
		[InlineData(0x00000080, 0xFFFFFFFF, 0x_FFFFFFFF_FFFFFF80, false, false)]
		[InlineData(0x00000000, 0x80000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x00008000, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x00000080, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x0000007F, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x000000FF, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x00007FFF, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x0000FFFF, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0x7FFFFFFF, 0x_00000000_00000000, false, false)]
		[InlineData(0x00000000, 0xFFFFFFFF, 0x_00000000_00000000, false, false)]
		[InlineData(0x0000007F, 0x80000000, 0x_FFFFFFC0_80000000, true, true)]
		[InlineData(0x0000007F, 0x00008000, 0x_00000000_003F8000, false, false)]
		[InlineData(0x0000007F, 0x00000080, 0x_00000000_00003F80, false, false)]
		[InlineData(0x0000007F, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x0000007F, 0x0000007F, 0x_00000000_00003F01, false, false)]
		[InlineData(0x0000007F, 0x000000FF, 0x_00000000_00007E81, false, false)]
		[InlineData(0x0000007F, 0x00007FFF, 0x_00000000_003F7F81, false, false)]
		[InlineData(0x0000007F, 0x0000FFFF, 0x_00000000_007EFF81, false, false)]
		[InlineData(0x0000007F, 0x7FFFFFFF, 0x_0000003F_7FFFFF81, true, true)]
		[InlineData(0x0000007F, 0xFFFFFFFF, 0x_FFFFFFFF_FFFFFF81, false, false)]
		[InlineData(0x000000FF, 0x80000000, 0x_FFFFFF80_80000000, true, true)]
		[InlineData(0x000000FF, 0x00008000, 0x_00000000_007F8000, false, false)]
		[InlineData(0x000000FF, 0x00000080, 0x_00000000_00007F80, false, false)]
		[InlineData(0x000000FF, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x000000FF, 0x0000007F, 0x_00000000_00007E81, false, false)]
		[InlineData(0x000000FF, 0x000000FF, 0x_00000000_0000FE01, false, false)]
		[InlineData(0x000000FF, 0x00007FFF, 0x_00000000_007F7F01, false, false)]
		[InlineData(0x000000FF, 0x0000FFFF, 0x_00000000_00FEFF01, false, false)]
		[InlineData(0x000000FF, 0x7FFFFFFF, 0x_0000007F_7FFFFF01, true, true)]
		[InlineData(0x000000FF, 0xFFFFFFFF, 0x_FFFFFFFF_FFFFFF01, false, false)]
		[InlineData(0x00007FFF, 0x80000000, 0x_FFFFC000_80000000, true, true)]
		[InlineData(0x00007FFF, 0x00008000, 0x_00000000_3FFF8000, false, false)]
		[InlineData(0x00007FFF, 0x00000080, 0x_00000000_003FFF80, false, false)]
		[InlineData(0x00007FFF, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x00007FFF, 0x0000007F, 0x_00000000_003F7F81, false, false)]
		[InlineData(0x00007FFF, 0x000000FF, 0x_00000000_007F7F01, false, false)]
		[InlineData(0x00007FFF, 0x00007FFF, 0x_00000000_3FFF0001, false, false)]
		[InlineData(0x00007FFF, 0x0000FFFF, 0x_00000000_7FFE8001, false, false)]
		[InlineData(0x00007FFF, 0x7FFFFFFF, 0x_00003FFF_7FFF8001, true, true)]
		[InlineData(0x00007FFF, 0xFFFFFFFF, 0x_FFFFFFFF_FFFF8001, false, false)]
		[InlineData(0x0000FFFF, 0x80000000, 0x_FFFF8000_80000000, true, true)]
		[InlineData(0x0000FFFF, 0x00008000, 0x_00000000_7FFF8000, false, false)]
		[InlineData(0x0000FFFF, 0x00000080, 0x_00000000_007FFF80, false, false)]
		[InlineData(0x0000FFFF, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x0000FFFF, 0x0000007F, 0x_00000000_007EFF81, false, false)]
		[InlineData(0x0000FFFF, 0x000000FF, 0x_00000000_00FEFF01, false, false)]
		[InlineData(0x0000FFFF, 0x00007FFF, 0x_00000000_7FFE8001, false, false)]
		[InlineData(0x0000FFFF, 0x0000FFFF, 0x_00000000_FFFE0001, true, true)]
		[InlineData(0x0000FFFF, 0x7FFFFFFF, 0x_00007FFF_7FFF0001, true, true)]
		[InlineData(0x0000FFFF, 0xFFFFFFFF, 0x_FFFFFFFF_FFFF0001, false, false)]
		[InlineData(0x7FFFFFFF, 0x80000000, 0x_C0000000_80000000, true, true)]
		[InlineData(0x7FFFFFFF, 0x00008000, 0x_00003FFF_FFFF8000, true, true)]
		[InlineData(0x7FFFFFFF, 0x00000080, 0x_0000003F_FFFFFF80, true, true)]
		[InlineData(0x7FFFFFFF, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0x7FFFFFFF, 0x0000007F, 0x_0000003F_7FFFFF81, true, true)]
		[InlineData(0x7FFFFFFF, 0x000000FF, 0x_0000007F_7FFFFF01, true, true)]
		[InlineData(0x7FFFFFFF, 0x00007FFF, 0x_00003FFF_7FFF8001, true, true)]
		[InlineData(0x7FFFFFFF, 0x0000FFFF, 0x_00007FFF_7FFF0001, true, true)]
		[InlineData(0x7FFFFFFF, 0x7FFFFFFF, 0x_3FFFFFFF_00000001, true, true)]
		[InlineData(0x7FFFFFFF, 0xFFFFFFFF, 0x_FFFFFFFF_80000001, false, false)]
		[InlineData(0xFFFFFFFF, 0x80000000, 0x_00000000_80000000, true, true)]
		[InlineData(0xFFFFFFFF, 0x00008000, 0x_FFFFFFFF_FFFF8000, false, false)]
		[InlineData(0xFFFFFFFF, 0x00000080, 0x_FFFFFFFF_FFFFFF80, false, false)]
		[InlineData(0xFFFFFFFF, 0x00000000, 0x_00000000_00000000, false, false)]
		[InlineData(0xFFFFFFFF, 0x0000007F, 0x_FFFFFFFF_FFFFFF81, false, false)]
		[InlineData(0xFFFFFFFF, 0x000000FF, 0x_FFFFFFFF_FFFFFF01, false, false)]
		[InlineData(0xFFFFFFFF, 0x00007FFF, 0x_FFFFFFFF_FFFF8001, false, false)]
		[InlineData(0xFFFFFFFF, 0x0000FFFF, 0x_FFFFFFFF_FFFF0001, false, false)]
		[InlineData(0xFFFFFFFF, 0x7FFFFFFF, 0x_FFFFFFFF_80000001, false, false)]
		[InlineData(0xFFFFFFFF, 0xFFFFFFFF, 0x_00000000_00000001, false, false)]
		#endregion
		public void Imul32TwoOperandTest(uint left, uint right, ulong result, bool carry, bool overflow)
		{
			_cpu.Imul(new Register32(left), new Register32(right)).Should().Be(new Register64(result).Low);
			_cpu.Eflags.Carry.Should().Be(carry);
			_cpu.Eflags.Overflow.Should().Be(overflow);
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80000001, false, true, false)]
		[InlineData(0x00008000, 0x00008001, false, false, false)]
		[InlineData(0x00000080, 0x00000081, false, false, false)]
		[InlineData(0x00000000, 0x00000001, false, false, false)]
		[InlineData(0x0000007F, 0x00000080, false, false, false)]
		[InlineData(0x000000FF, 0x00000100, false, false, false)]
		[InlineData(0x00007FFF, 0x00008000, false, false, false)]
		[InlineData(0x0000FFFF, 0x00010000, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x80000000, false, true, true)]
		[InlineData(0xFFFFFFFF, 0x00000000, true, false, false)]
		#endregion
		public void Inc32Test(uint value, uint result, bool zero, bool sign, bool overflow)
		{
			_cpu.Inc(new Register32(value)).Should().Be(new Register32(result));
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().Be(overflow);
		}

		[Theory]
		#region
		[InlineData(0x8000, 0xFFFF8000)]
		[InlineData(0x0080, 0x00000080)]
		[InlineData(0x0000, 0x00000000)]
		[InlineData(0x007F, 0x0000007F)]
		[InlineData(0x00FF, 0x000000FF)]
		[InlineData(0x7FFF, 0x00007FFF)]
		[InlineData(0xFFFF, 0xFFFFFFFF)]
		#endregion
		public void Movsx16Test(ushort value, uint result)
		{
			_cpu.Movsx(new Register16(value)).Should().Be(new Register32(result));
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80000000, true, false, true, true)]
		[InlineData(0x00008000, 0xFFFF8000, true, false, true, false)]
		[InlineData(0x00000080, 0xFFFFFF80, true, false, true, false)]
		[InlineData(0x00000000, 0x00000000, false, true, false, false)]
		[InlineData(0x0000007F, 0xFFFFFF81, true, false, true, false)]
		[InlineData(0x000000FF, 0xFFFFFF01, true, false, true, false)]
		[InlineData(0x00007FFF, 0xFFFF8001, true, false, true, false)]
		[InlineData(0x0000FFFF, 0xFFFF0001, true, false, true, false)]
		[InlineData(0x7FFFFFFF, 0x80000001, true, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x00000001, true, false, false, false)]
		#endregion
		public void Neg32Test(uint value, uint result, bool carry, bool zero, bool sign, bool overflow)
		{
			_cpu.Neg(new Register32(value)).Should().Be(new Register32(result));
			_cpu.Eflags.Carry.Should().Be(carry);
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().Be(overflow);
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80000000, 0x80000000, false, true)]
		[InlineData(0x80000000, 0x00008000, 0x80008000, false, true)]
		[InlineData(0x80000000, 0x00000080, 0x80000080, false, true)]
		[InlineData(0x80000000, 0x00000000, 0x80000000, false, true)]
		[InlineData(0x80000000, 0x0000007F, 0x8000007F, false, true)]
		[InlineData(0x80000000, 0x000000FF, 0x800000FF, false, true)]
		[InlineData(0x80000000, 0x00007FFF, 0x80007FFF, false, true)]
		[InlineData(0x80000000, 0x0000FFFF, 0x8000FFFF, false, true)]
		[InlineData(0x80000000, 0x7FFFFFFF, 0xFFFFFFFF, false, true)]
		[InlineData(0x80000000, 0xFFFFFFFF, 0xFFFFFFFF, false, true)]
		[InlineData(0x00008000, 0x80000000, 0x80008000, false, true)]
		[InlineData(0x00008000, 0x00008000, 0x00008000, false, false)]
		[InlineData(0x00008000, 0x00000080, 0x00008080, false, false)]
		[InlineData(0x00008000, 0x00000000, 0x00008000, false, false)]
		[InlineData(0x00008000, 0x0000007F, 0x0000807F, false, false)]
		[InlineData(0x00008000, 0x000000FF, 0x000080FF, false, false)]
		[InlineData(0x00008000, 0x00007FFF, 0x0000FFFF, false, false)]
		[InlineData(0x00008000, 0x0000FFFF, 0x0000FFFF, false, false)]
		[InlineData(0x00008000, 0x7FFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x00008000, 0xFFFFFFFF, 0xFFFFFFFF, false, true)]
		[InlineData(0x00000080, 0x80000000, 0x80000080, false, true)]
		[InlineData(0x00000080, 0x00008000, 0x00008080, false, false)]
		[InlineData(0x00000080, 0x00000080, 0x00000080, false, false)]
		[InlineData(0x00000080, 0x00000000, 0x00000080, false, false)]
		[InlineData(0x00000080, 0x0000007F, 0x000000FF, false, false)]
		[InlineData(0x00000080, 0x000000FF, 0x000000FF, false, false)]
		[InlineData(0x00000080, 0x00007FFF, 0x00007FFF, false, false)]
		[InlineData(0x00000080, 0x0000FFFF, 0x0000FFFF, false, false)]
		[InlineData(0x00000080, 0x7FFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x00000080, 0xFFFFFFFF, 0xFFFFFFFF, false, true)]
		[InlineData(0x00000000, 0x80000000, 0x80000000, false, true)]
		[InlineData(0x00000000, 0x00008000, 0x00008000, false, false)]
		[InlineData(0x00000000, 0x00000080, 0x00000080, false, false)]
		[InlineData(0x00000000, 0x00000000, 0x00000000, true, false)]
		[InlineData(0x00000000, 0x0000007F, 0x0000007F, false, false)]
		[InlineData(0x00000000, 0x000000FF, 0x000000FF, false, false)]
		[InlineData(0x00000000, 0x00007FFF, 0x00007FFF, false, false)]
		[InlineData(0x00000000, 0x0000FFFF, 0x0000FFFF, false, false)]
		[InlineData(0x00000000, 0x7FFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x00000000, 0xFFFFFFFF, 0xFFFFFFFF, false, true)]
		[InlineData(0x0000007F, 0x80000000, 0x8000007F, false, true)]
		[InlineData(0x0000007F, 0x00008000, 0x0000807F, false, false)]
		[InlineData(0x0000007F, 0x00000080, 0x000000FF, false, false)]
		[InlineData(0x0000007F, 0x00000000, 0x0000007F, false, false)]
		[InlineData(0x0000007F, 0x0000007F, 0x0000007F, false, false)]
		[InlineData(0x0000007F, 0x000000FF, 0x000000FF, false, false)]
		[InlineData(0x0000007F, 0x00007FFF, 0x00007FFF, false, false)]
		[InlineData(0x0000007F, 0x0000FFFF, 0x0000FFFF, false, false)]
		[InlineData(0x0000007F, 0x7FFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x0000007F, 0xFFFFFFFF, 0xFFFFFFFF, false, true)]
		[InlineData(0x000000FF, 0x80000000, 0x800000FF, false, true)]
		[InlineData(0x000000FF, 0x00008000, 0x000080FF, false, false)]
		[InlineData(0x000000FF, 0x00000080, 0x000000FF, false, false)]
		[InlineData(0x000000FF, 0x00000000, 0x000000FF, false, false)]
		[InlineData(0x000000FF, 0x0000007F, 0x000000FF, false, false)]
		[InlineData(0x000000FF, 0x000000FF, 0x000000FF, false, false)]
		[InlineData(0x000000FF, 0x00007FFF, 0x00007FFF, false, false)]
		[InlineData(0x000000FF, 0x0000FFFF, 0x0000FFFF, false, false)]
		[InlineData(0x000000FF, 0x7FFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x000000FF, 0xFFFFFFFF, 0xFFFFFFFF, false, true)]
		[InlineData(0x00007FFF, 0x80000000, 0x80007FFF, false, true)]
		[InlineData(0x00007FFF, 0x00008000, 0x0000FFFF, false, false)]
		[InlineData(0x00007FFF, 0x00000080, 0x00007FFF, false, false)]
		[InlineData(0x00007FFF, 0x00000000, 0x00007FFF, false, false)]
		[InlineData(0x00007FFF, 0x0000007F, 0x00007FFF, false, false)]
		[InlineData(0x00007FFF, 0x000000FF, 0x00007FFF, false, false)]
		[InlineData(0x00007FFF, 0x00007FFF, 0x00007FFF, false, false)]
		[InlineData(0x00007FFF, 0x0000FFFF, 0x0000FFFF, false, false)]
		[InlineData(0x00007FFF, 0x7FFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x00007FFF, 0xFFFFFFFF, 0xFFFFFFFF, false, true)]
		[InlineData(0x0000FFFF, 0x80000000, 0x8000FFFF, false, true)]
		[InlineData(0x0000FFFF, 0x00008000, 0x0000FFFF, false, false)]
		[InlineData(0x0000FFFF, 0x00000080, 0x0000FFFF, false, false)]
		[InlineData(0x0000FFFF, 0x00000000, 0x0000FFFF, false, false)]
		[InlineData(0x0000FFFF, 0x0000007F, 0x0000FFFF, false, false)]
		[InlineData(0x0000FFFF, 0x000000FF, 0x0000FFFF, false, false)]
		[InlineData(0x0000FFFF, 0x00007FFF, 0x0000FFFF, false, false)]
		[InlineData(0x0000FFFF, 0x0000FFFF, 0x0000FFFF, false, false)]
		[InlineData(0x0000FFFF, 0x7FFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x0000FFFF, 0xFFFFFFFF, 0xFFFFFFFF, false, true)]
		[InlineData(0x7FFFFFFF, 0x80000000, 0xFFFFFFFF, false, true)]
		[InlineData(0x7FFFFFFF, 0x00008000, 0x7FFFFFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x00000080, 0x7FFFFFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x00000000, 0x7FFFFFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x0000007F, 0x7FFFFFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x000000FF, 0x7FFFFFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x00007FFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x0000FFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x7FFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, false, true)]
		[InlineData(0xFFFFFFFF, 0x80000000, 0xFFFFFFFF, false, true)]
		[InlineData(0xFFFFFFFF, 0x00008000, 0xFFFFFFFF, false, true)]
		[InlineData(0xFFFFFFFF, 0x00000080, 0xFFFFFFFF, false, true)]
		[InlineData(0xFFFFFFFF, 0x00000000, 0xFFFFFFFF, false, true)]
		[InlineData(0xFFFFFFFF, 0x0000007F, 0xFFFFFFFF, false, true)]
		[InlineData(0xFFFFFFFF, 0x000000FF, 0xFFFFFFFF, false, true)]
		[InlineData(0xFFFFFFFF, 0x00007FFF, 0xFFFFFFFF, false, true)]
		[InlineData(0xFFFFFFFF, 0x0000FFFF, 0xFFFFFFFF, false, true)]
		[InlineData(0xFFFFFFFF, 0x7FFFFFFF, 0xFFFFFFFF, false, true)]
		[InlineData(0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, false, true)]
		#endregion
		public void Or32Test(uint left, uint right, uint result, bool zero, bool sign)
		{
			_cpu.Or(new Register32(left), new Register32(right)).Should().Be(new Register32(result));
			_cpu.Eflags.Carry.Should().BeFalse();
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().BeFalse();
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80, 0xFFFFFF80)]
		[InlineData(0x80000000, 0x00, 0x80000000)]
		[InlineData(0x80000000, 0x7F, 0x8000007F)]
		[InlineData(0x80000000, 0xFF, 0xFFFFFFFF)]
		[InlineData(0x00008000, 0x80, 0xFFFFFF80)]
		[InlineData(0x00008000, 0x00, 0x00008000)]
		[InlineData(0x00008000, 0x7F, 0x0000807F)]
		[InlineData(0x00008000, 0xFF, 0xFFFFFFFF)]
		[InlineData(0x00000080, 0x80, 0xFFFFFF80)]
		[InlineData(0x00000080, 0x00, 0x00000080)]
		[InlineData(0x00000080, 0x7F, 0x000000FF)]
		[InlineData(0x00000080, 0xFF, 0xFFFFFFFF)]
		[InlineData(0x00000000, 0x80, 0xFFFFFF80)]
		[InlineData(0x00000000, 0x00, 0x00000000)]
		[InlineData(0x00000000, 0x7F, 0x0000007F)]
		[InlineData(0x00000000, 0xFF, 0xFFFFFFFF)]
		[InlineData(0x0000007F, 0x80, 0xFFFFFFFF)]
		[InlineData(0x0000007F, 0x00, 0x0000007F)]
		[InlineData(0x0000007F, 0x7F, 0x0000007F)]
		[InlineData(0x0000007F, 0xFF, 0xFFFFFFFF)]
		[InlineData(0x000000FF, 0x80, 0xFFFFFFFF)]
		[InlineData(0x000000FF, 0x00, 0x000000FF)]
		[InlineData(0x000000FF, 0x7F, 0x000000FF)]
		[InlineData(0x000000FF, 0xFF, 0xFFFFFFFF)]
		[InlineData(0x00007FFF, 0x80, 0xFFFFFFFF)]
		[InlineData(0x00007FFF, 0x00, 0x00007FFF)]
		[InlineData(0x00007FFF, 0x7F, 0x00007FFF)]
		[InlineData(0x00007FFF, 0xFF, 0xFFFFFFFF)]
		[InlineData(0x0000FFFF, 0x80, 0xFFFFFFFF)]
		[InlineData(0x0000FFFF, 0x00, 0x0000FFFF)]
		[InlineData(0x0000FFFF, 0x7F, 0x0000FFFF)]
		[InlineData(0x0000FFFF, 0xFF, 0xFFFFFFFF)]
		[InlineData(0x7FFFFFFF, 0x80, 0xFFFFFFFF)]
		[InlineData(0x7FFFFFFF, 0x00, 0x7FFFFFFF)]
		[InlineData(0x7FFFFFFF, 0x7F, 0x7FFFFFFF)]
		[InlineData(0x7FFFFFFF, 0xFF, 0xFFFFFFFF)]
		[InlineData(0xFFFFFFFF, 0x80, 0xFFFFFFFF)]
		[InlineData(0xFFFFFFFF, 0x00, 0xFFFFFFFF)]
		[InlineData(0xFFFFFFFF, 0x7F, 0xFFFFFFFF)]
		[InlineData(0xFFFFFFFF, 0xFF, 0xFFFFFFFF)]
		#endregion
		public void Or32_8Test(uint left, byte right, uint result)
		{
			_cpu.Or(new Register32(left), new Register8(right)).Should().Be(new Register32(result));
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x01, 0xC0000000, false, false, true, false)]
		[InlineData(0x80000000, 0x04, 0xF8000000, false, false, true, null)]
		[InlineData(0x80000000, 0x20, 0x80000000, null, null, null, null)]
		[InlineData(0x80000000, 0x21, 0xC0000000, false, false, true, false)]
		[InlineData(0x00008000, 0x01, 0x00004000, false, false, false, false)]
		[InlineData(0x00008000, 0x04, 0x00000800, false, false, false, null)]
		[InlineData(0x00008000, 0x20, 0x00008000, null, null, null, null)]
		[InlineData(0x00008000, 0x21, 0x00004000, false, false, false, false)]
		[InlineData(0x00000080, 0x01, 0x00000040, false, false, false, false)]
		[InlineData(0x00000080, 0x04, 0x00000008, false, false, false, null)]
		[InlineData(0x00000080, 0x20, 0x00000080, null, null, null, null)]
		[InlineData(0x00000080, 0x21, 0x00000040, false, false, false, false)]
		[InlineData(0x00000000, 0x01, 0x00000000, false, true, false, false)]
		[InlineData(0x00000000, 0x04, 0x00000000, false, true, false, null)]
		[InlineData(0x00000000, 0x20, 0x00000000, null, null, null, null)]
		[InlineData(0x00000000, 0x21, 0x00000000, false, true, false, false)]
		[InlineData(0x0000007F, 0x01, 0x0000003F, true, false, false, false)]
		[InlineData(0x0000007F, 0x04, 0x00000007, true, false, false, null)]
		[InlineData(0x0000007F, 0x20, 0x0000007F, null, null, null, null)]
		[InlineData(0x0000007F, 0x21, 0x0000003F, true, false, false, false)]
		[InlineData(0x000000FF, 0x01, 0x0000007F, true, false, false, false)]
		[InlineData(0x000000FF, 0x04, 0x0000000F, true, false, false, null)]
		[InlineData(0x000000FF, 0x20, 0x000000FF, null, null, null, null)]
		[InlineData(0x000000FF, 0x21, 0x0000007F, true, false, false, false)]
		[InlineData(0x00007FFF, 0x01, 0x00003FFF, true, false, false, false)]
		[InlineData(0x00007FFF, 0x04, 0x000007FF, true, false, false, null)]
		[InlineData(0x00007FFF, 0x20, 0x00007FFF, null, null, null, null)]
		[InlineData(0x00007FFF, 0x21, 0x00003FFF, true, false, false, false)]
		[InlineData(0x0000FFFF, 0x01, 0x00007FFF, true, false, false, false)]
		[InlineData(0x0000FFFF, 0x04, 0x00000FFF, true, false, false, null)]
		[InlineData(0x0000FFFF, 0x20, 0x0000FFFF, null, null, null, null)]
		[InlineData(0x0000FFFF, 0x21, 0x00007FFF, true, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x01, 0x3FFFFFFF, true, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x04, 0x07FFFFFF, true, false, false, null)]
		[InlineData(0x7FFFFFFF, 0x20, 0x7FFFFFFF, null, null, null, null)]
		[InlineData(0x7FFFFFFF, 0x21, 0x3FFFFFFF, true, false, false, false)]
		[InlineData(0xFFFFFFFF, 0x01, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x04, 0xFFFFFFFF, true, false, true, null)]
		[InlineData(0xFFFFFFFF, 0x20, 0xFFFFFFFF, null, null, null, null)]
		[InlineData(0xFFFFFFFF, 0x21, 0xFFFFFFFF, true, false, true, false)]
		#endregion
		public void Sar32Test(uint value, byte count, uint result, bool? carry, bool? zero, bool? sign, bool? overflow)
		{
			_cpu.Sar(new Register32(value), new Register8(count)).Should().Be(new Register32(result));
			if (carry.HasValue)
				_cpu.Eflags.Carry.Should().Be(carry.Value);
			if (zero.HasValue)
				_cpu.Eflags.Zero.Should().Be(zero.Value);
			if (sign.HasValue)
				_cpu.Eflags.Sign.Should().Be(sign.Value);
			if (overflow.HasValue)
				_cpu.Eflags.Overflow.Should().Be(overflow.Value);
		}

		[Theory]
		#region
		[InlineData(false, 0x80000000, 0x80000000, 0x00000000, false, true, false, false)]
		[InlineData(false, 0x80000000, 0x00008000, 0x7FFF8000, false, false, false, true)]
		[InlineData(false, 0x80000000, 0x00000080, 0x7FFFFF80, false, false, false, true)]
		[InlineData(false, 0x80000000, 0x00000000, 0x80000000, false, false, true, false)]
		[InlineData(false, 0x80000000, 0x0000007F, 0x7FFFFF81, false, false, false, true)]
		[InlineData(false, 0x80000000, 0x000000FF, 0x7FFFFF01, false, false, false, true)]
		[InlineData(false, 0x80000000, 0x00007FFF, 0x7FFF8001, false, false, false, true)]
		[InlineData(false, 0x80000000, 0x0000FFFF, 0x7FFF0001, false, false, false, true)]
		[InlineData(false, 0x80000000, 0x7FFFFFFF, 0x00000001, false, false, false, true)]
		[InlineData(false, 0x80000000, 0xFFFFFFFF, 0x80000001, true, false, true, false)]
		[InlineData(false, 0x00008000, 0x80000000, 0x80008000, true, false, true, true)]
		[InlineData(false, 0x00008000, 0x00008000, 0x00000000, false, true, false, false)]
		[InlineData(false, 0x00008000, 0x00000080, 0x00007F80, false, false, false, false)]
		[InlineData(false, 0x00008000, 0x00000000, 0x00008000, false, false, false, false)]
		[InlineData(false, 0x00008000, 0x0000007F, 0x00007F81, false, false, false, false)]
		[InlineData(false, 0x00008000, 0x000000FF, 0x00007F01, false, false, false, false)]
		[InlineData(false, 0x00008000, 0x00007FFF, 0x00000001, false, false, false, false)]
		[InlineData(false, 0x00008000, 0x0000FFFF, 0xFFFF8001, true, false, true, false)]
		[InlineData(false, 0x00008000, 0x7FFFFFFF, 0x80008001, true, false, true, false)]
		[InlineData(false, 0x00008000, 0xFFFFFFFF, 0x00008001, true, false, false, false)]
		[InlineData(false, 0x00000080, 0x80000000, 0x80000080, true, false, true, true)]
		[InlineData(false, 0x00000080, 0x00008000, 0xFFFF8080, true, false, true, false)]
		[InlineData(false, 0x00000080, 0x00000080, 0x00000000, false, true, false, false)]
		[InlineData(false, 0x00000080, 0x00000000, 0x00000080, false, false, false, false)]
		[InlineData(false, 0x00000080, 0x0000007F, 0x00000001, false, false, false, false)]
		[InlineData(false, 0x00000080, 0x000000FF, 0xFFFFFF81, true, false, true, false)]
		[InlineData(false, 0x00000080, 0x00007FFF, 0xFFFF8081, true, false, true, false)]
		[InlineData(false, 0x00000080, 0x0000FFFF, 0xFFFF0081, true, false, true, false)]
		[InlineData(false, 0x00000080, 0x7FFFFFFF, 0x80000081, true, false, true, false)]
		[InlineData(false, 0x00000080, 0xFFFFFFFF, 0x00000081, true, false, false, false)]
		[InlineData(false, 0x00000000, 0x80000000, 0x80000000, true, false, true, true)]
		[InlineData(false, 0x00000000, 0x00008000, 0xFFFF8000, true, false, true, false)]
		[InlineData(false, 0x00000000, 0x00000080, 0xFFFFFF80, true, false, true, false)]
		[InlineData(false, 0x00000000, 0x00000000, 0x00000000, false, true, false, false)]
		[InlineData(false, 0x00000000, 0x0000007F, 0xFFFFFF81, true, false, true, false)]
		[InlineData(false, 0x00000000, 0x000000FF, 0xFFFFFF01, true, false, true, false)]
		[InlineData(false, 0x00000000, 0x00007FFF, 0xFFFF8001, true, false, true, false)]
		[InlineData(false, 0x00000000, 0x0000FFFF, 0xFFFF0001, true, false, true, false)]
		[InlineData(false, 0x00000000, 0x7FFFFFFF, 0x80000001, true, false, true, false)]
		[InlineData(false, 0x00000000, 0xFFFFFFFF, 0x00000001, true, false, false, false)]
		[InlineData(false, 0x0000007F, 0x80000000, 0x8000007F, true, false, true, true)]
		[InlineData(false, 0x0000007F, 0x00008000, 0xFFFF807F, true, false, true, false)]
		[InlineData(false, 0x0000007F, 0x00000080, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(false, 0x0000007F, 0x00000000, 0x0000007F, false, false, false, false)]
		[InlineData(false, 0x0000007F, 0x0000007F, 0x00000000, false, true, false, false)]
		[InlineData(false, 0x0000007F, 0x000000FF, 0xFFFFFF80, true, false, true, false)]
		[InlineData(false, 0x0000007F, 0x00007FFF, 0xFFFF8080, true, false, true, false)]
		[InlineData(false, 0x0000007F, 0x0000FFFF, 0xFFFF0080, true, false, true, false)]
		[InlineData(false, 0x0000007F, 0x7FFFFFFF, 0x80000080, true, false, true, false)]
		[InlineData(false, 0x0000007F, 0xFFFFFFFF, 0x00000080, true, false, false, false)]
		[InlineData(false, 0x000000FF, 0x80000000, 0x800000FF, true, false, true, true)]
		[InlineData(false, 0x000000FF, 0x00008000, 0xFFFF80FF, true, false, true, false)]
		[InlineData(false, 0x000000FF, 0x00000080, 0x0000007F, false, false, false, false)]
		[InlineData(false, 0x000000FF, 0x00000000, 0x000000FF, false, false, false, false)]
		[InlineData(false, 0x000000FF, 0x0000007F, 0x00000080, false, false, false, false)]
		[InlineData(false, 0x000000FF, 0x000000FF, 0x00000000, false, true, false, false)]
		[InlineData(false, 0x000000FF, 0x00007FFF, 0xFFFF8100, true, false, true, false)]
		[InlineData(false, 0x000000FF, 0x0000FFFF, 0xFFFF0100, true, false, true, false)]
		[InlineData(false, 0x000000FF, 0x7FFFFFFF, 0x80000100, true, false, true, false)]
		[InlineData(false, 0x000000FF, 0xFFFFFFFF, 0x00000100, true, false, false, false)]
		[InlineData(false, 0x00007FFF, 0x80000000, 0x80007FFF, true, false, true, true)]
		[InlineData(false, 0x00007FFF, 0x00008000, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(false, 0x00007FFF, 0x00000080, 0x00007F7F, false, false, false, false)]
		[InlineData(false, 0x00007FFF, 0x00000000, 0x00007FFF, false, false, false, false)]
		[InlineData(false, 0x00007FFF, 0x0000007F, 0x00007F80, false, false, false, false)]
		[InlineData(false, 0x00007FFF, 0x000000FF, 0x00007F00, false, false, false, false)]
		[InlineData(false, 0x00007FFF, 0x00007FFF, 0x00000000, false, true, false, false)]
		[InlineData(false, 0x00007FFF, 0x0000FFFF, 0xFFFF8000, true, false, true, false)]
		[InlineData(false, 0x00007FFF, 0x7FFFFFFF, 0x80008000, true, false, true, false)]
		[InlineData(false, 0x00007FFF, 0xFFFFFFFF, 0x00008000, true, false, false, false)]
		[InlineData(false, 0x0000FFFF, 0x80000000, 0x8000FFFF, true, false, true, true)]
		[InlineData(false, 0x0000FFFF, 0x00008000, 0x00007FFF, false, false, false, false)]
		[InlineData(false, 0x0000FFFF, 0x00000080, 0x0000FF7F, false, false, false, false)]
		[InlineData(false, 0x0000FFFF, 0x00000000, 0x0000FFFF, false, false, false, false)]
		[InlineData(false, 0x0000FFFF, 0x0000007F, 0x0000FF80, false, false, false, false)]
		[InlineData(false, 0x0000FFFF, 0x000000FF, 0x0000FF00, false, false, false, false)]
		[InlineData(false, 0x0000FFFF, 0x00007FFF, 0x00008000, false, false, false, false)]
		[InlineData(false, 0x0000FFFF, 0x0000FFFF, 0x00000000, false, true, false, false)]
		[InlineData(false, 0x0000FFFF, 0x7FFFFFFF, 0x80010000, true, false, true, false)]
		[InlineData(false, 0x0000FFFF, 0xFFFFFFFF, 0x00010000, true, false, false, false)]
		[InlineData(false, 0x7FFFFFFF, 0x80000000, 0xFFFFFFFF, true, false, true, true)]
		[InlineData(false, 0x7FFFFFFF, 0x00008000, 0x7FFF7FFF, false, false, false, false)]
		[InlineData(false, 0x7FFFFFFF, 0x00000080, 0x7FFFFF7F, false, false, false, false)]
		[InlineData(false, 0x7FFFFFFF, 0x00000000, 0x7FFFFFFF, false, false, false, false)]
		[InlineData(false, 0x7FFFFFFF, 0x0000007F, 0x7FFFFF80, false, false, false, false)]
		[InlineData(false, 0x7FFFFFFF, 0x000000FF, 0x7FFFFF00, false, false, false, false)]
		[InlineData(false, 0x7FFFFFFF, 0x00007FFF, 0x7FFF8000, false, false, false, false)]
		[InlineData(false, 0x7FFFFFFF, 0x0000FFFF, 0x7FFF0000, false, false, false, false)]
		[InlineData(false, 0x7FFFFFFF, 0x7FFFFFFF, 0x00000000, false, true, false, false)]
		[InlineData(false, 0x7FFFFFFF, 0xFFFFFFFF, 0x80000000, true, false, true, true)]
		[InlineData(false, 0xFFFFFFFF, 0x80000000, 0x7FFFFFFF, false, false, false, false)]
		[InlineData(false, 0xFFFFFFFF, 0x00008000, 0xFFFF7FFF, false, false, true, false)]
		[InlineData(false, 0xFFFFFFFF, 0x00000080, 0xFFFFFF7F, false, false, true, false)]
		[InlineData(false, 0xFFFFFFFF, 0x00000000, 0xFFFFFFFF, false, false, true, false)]
		[InlineData(false, 0xFFFFFFFF, 0x0000007F, 0xFFFFFF80, false, false, true, false)]
		[InlineData(false, 0xFFFFFFFF, 0x000000FF, 0xFFFFFF00, false, false, true, false)]
		[InlineData(false, 0xFFFFFFFF, 0x00007FFF, 0xFFFF8000, false, false, true, false)]
		[InlineData(false, 0xFFFFFFFF, 0x0000FFFF, 0xFFFF0000, false, false, true, false)]
		[InlineData(false, 0xFFFFFFFF, 0x7FFFFFFF, 0x80000000, false, false, true, false)]
		[InlineData(false, 0xFFFFFFFF, 0xFFFFFFFF, 0x00000000, false, true, false, false)]
		[InlineData(true, 0x80000000, 0x80000000, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(true, 0x80000000, 0x00008000, 0x7FFF7FFF, false, false, false, true)]
		[InlineData(true, 0x80000000, 0x00000080, 0x7FFFFF7F, false, false, false, true)]
		[InlineData(true, 0x80000000, 0x00000000, 0x7FFFFFFF, false, false, false, true)]
		[InlineData(true, 0x80000000, 0x0000007F, 0x7FFFFF80, false, false, false, true)]
		[InlineData(true, 0x80000000, 0x000000FF, 0x7FFFFF00, false, false, false, true)]
		[InlineData(true, 0x80000000, 0x00007FFF, 0x7FFF8000, false, false, false, true)]
		[InlineData(true, 0x80000000, 0x0000FFFF, 0x7FFF0000, false, false, false, true)]
		[InlineData(true, 0x80000000, 0x7FFFFFFF, 0x00000000, false, true, false, true)]
		[InlineData(true, 0x80000000, 0xFFFFFFFF, 0x80000000, true, false, true, false)]
		[InlineData(true, 0x00008000, 0x80000000, 0x80007FFF, true, false, true, true)]
		[InlineData(true, 0x00008000, 0x00008000, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(true, 0x00008000, 0x00000080, 0x00007F7F, false, false, false, false)]
		[InlineData(true, 0x00008000, 0x00000000, 0x00007FFF, false, false, false, false)]
		[InlineData(true, 0x00008000, 0x0000007F, 0x00007F80, false, false, false, false)]
		[InlineData(true, 0x00008000, 0x000000FF, 0x00007F00, false, false, false, false)]
		[InlineData(true, 0x00008000, 0x00007FFF, 0x00000000, false, true, false, false)]
		[InlineData(true, 0x00008000, 0x0000FFFF, 0xFFFF8000, true, false, true, false)]
		[InlineData(true, 0x00008000, 0x7FFFFFFF, 0x80008000, true, false, true, false)]
		[InlineData(true, 0x00008000, 0xFFFFFFFF, 0x00008000, true, false, false, false)]
		[InlineData(true, 0x00000080, 0x80000000, 0x8000007F, true, false, true, true)]
		[InlineData(true, 0x00000080, 0x00008000, 0xFFFF807F, true, false, true, false)]
		[InlineData(true, 0x00000080, 0x00000080, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(true, 0x00000080, 0x00000000, 0x0000007F, false, false, false, false)]
		[InlineData(true, 0x00000080, 0x0000007F, 0x00000000, false, true, false, false)]
		[InlineData(true, 0x00000080, 0x000000FF, 0xFFFFFF80, true, false, true, false)]
		[InlineData(true, 0x00000080, 0x00007FFF, 0xFFFF8080, true, false, true, false)]
		[InlineData(true, 0x00000080, 0x0000FFFF, 0xFFFF0080, true, false, true, false)]
		[InlineData(true, 0x00000080, 0x7FFFFFFF, 0x80000080, true, false, true, false)]
		[InlineData(true, 0x00000080, 0xFFFFFFFF, 0x00000080, true, false, false, false)]
		[InlineData(true, 0x00000000, 0x80000000, 0x7FFFFFFF, true, false, false, false)]
		[InlineData(true, 0x00000000, 0x00008000, 0xFFFF7FFF, true, false, true, false)]
		[InlineData(true, 0x00000000, 0x00000080, 0xFFFFFF7F, true, false, true, false)]
		[InlineData(true, 0x00000000, 0x00000000, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(true, 0x00000000, 0x0000007F, 0xFFFFFF80, true, false, true, false)]
		[InlineData(true, 0x00000000, 0x000000FF, 0xFFFFFF00, true, false, true, false)]
		[InlineData(true, 0x00000000, 0x00007FFF, 0xFFFF8000, true, false, true, false)]
		[InlineData(true, 0x00000000, 0x0000FFFF, 0xFFFF0000, true, false, true, false)]
		[InlineData(true, 0x00000000, 0x7FFFFFFF, 0x80000000, true, false, true, false)]
		[InlineData(true, 0x00000000, 0xFFFFFFFF, 0x00000000, true, true, false, false)]
		[InlineData(true, 0x0000007F, 0x80000000, 0x8000007E, true, false, true, true)]
		[InlineData(true, 0x0000007F, 0x00008000, 0xFFFF807E, true, false, true, false)]
		[InlineData(true, 0x0000007F, 0x00000080, 0xFFFFFFFE, true, false, true, false)]
		[InlineData(true, 0x0000007F, 0x00000000, 0x0000007E, false, false, false, false)]
		[InlineData(true, 0x0000007F, 0x0000007F, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(true, 0x0000007F, 0x000000FF, 0xFFFFFF7F, true, false, true, false)]
		[InlineData(true, 0x0000007F, 0x00007FFF, 0xFFFF807F, true, false, true, false)]
		[InlineData(true, 0x0000007F, 0x0000FFFF, 0xFFFF007F, true, false, true, false)]
		[InlineData(true, 0x0000007F, 0x7FFFFFFF, 0x8000007F, true, false, true, false)]
		[InlineData(true, 0x0000007F, 0xFFFFFFFF, 0x0000007F, true, false, false, false)]
		[InlineData(true, 0x000000FF, 0x80000000, 0x800000FE, true, false, true, true)]
		[InlineData(true, 0x000000FF, 0x00008000, 0xFFFF80FE, true, false, true, false)]
		[InlineData(true, 0x000000FF, 0x00000080, 0x0000007E, false, false, false, false)]
		[InlineData(true, 0x000000FF, 0x00000000, 0x000000FE, false, false, false, false)]
		[InlineData(true, 0x000000FF, 0x0000007F, 0x0000007F, false, false, false, false)]
		[InlineData(true, 0x000000FF, 0x000000FF, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(true, 0x000000FF, 0x00007FFF, 0xFFFF80FF, true, false, true, false)]
		[InlineData(true, 0x000000FF, 0x0000FFFF, 0xFFFF00FF, true, false, true, false)]
		[InlineData(true, 0x000000FF, 0x7FFFFFFF, 0x800000FF, true, false, true, false)]
		[InlineData(true, 0x000000FF, 0xFFFFFFFF, 0x000000FF, true, false, false, false)]
		[InlineData(true, 0x00007FFF, 0x80000000, 0x80007FFE, true, false, true, true)]
		[InlineData(true, 0x00007FFF, 0x00008000, 0xFFFFFFFE, true, false, true, false)]
		[InlineData(true, 0x00007FFF, 0x00000080, 0x00007F7E, false, false, false, false)]
		[InlineData(true, 0x00007FFF, 0x00000000, 0x00007FFE, false, false, false, false)]
		[InlineData(true, 0x00007FFF, 0x0000007F, 0x00007F7F, false, false, false, false)]
		[InlineData(true, 0x00007FFF, 0x000000FF, 0x00007EFF, false, false, false, false)]
		[InlineData(true, 0x00007FFF, 0x00007FFF, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(true, 0x00007FFF, 0x0000FFFF, 0xFFFF7FFF, true, false, true, false)]
		[InlineData(true, 0x00007FFF, 0x7FFFFFFF, 0x80007FFF, true, false, true, false)]
		[InlineData(true, 0x00007FFF, 0xFFFFFFFF, 0x00007FFF, true, false, false, false)]
		[InlineData(true, 0x0000FFFF, 0x80000000, 0x8000FFFE, true, false, true, true)]
		[InlineData(true, 0x0000FFFF, 0x00008000, 0x00007FFE, false, false, false, false)]
		[InlineData(true, 0x0000FFFF, 0x00000080, 0x0000FF7E, false, false, false, false)]
		[InlineData(true, 0x0000FFFF, 0x00000000, 0x0000FFFE, false, false, false, false)]
		[InlineData(true, 0x0000FFFF, 0x0000007F, 0x0000FF7F, false, false, false, false)]
		[InlineData(true, 0x0000FFFF, 0x000000FF, 0x0000FEFF, false, false, false, false)]
		[InlineData(true, 0x0000FFFF, 0x00007FFF, 0x00007FFF, false, false, false, false)]
		[InlineData(true, 0x0000FFFF, 0x0000FFFF, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(true, 0x0000FFFF, 0x7FFFFFFF, 0x8000FFFF, true, false, true, false)]
		[InlineData(true, 0x0000FFFF, 0xFFFFFFFF, 0x0000FFFF, true, false, false, false)]
		[InlineData(true, 0x7FFFFFFF, 0x80000000, 0xFFFFFFFE, true, false, true, true)]
		[InlineData(true, 0x7FFFFFFF, 0x00008000, 0x7FFF7FFE, false, false, false, false)]
		[InlineData(true, 0x7FFFFFFF, 0x00000080, 0x7FFFFF7E, false, false, false, false)]
		[InlineData(true, 0x7FFFFFFF, 0x00000000, 0x7FFFFFFE, false, false, false, false)]
		[InlineData(true, 0x7FFFFFFF, 0x0000007F, 0x7FFFFF7F, false, false, false, false)]
		[InlineData(true, 0x7FFFFFFF, 0x000000FF, 0x7FFFFEFF, false, false, false, false)]
		[InlineData(true, 0x7FFFFFFF, 0x00007FFF, 0x7FFF7FFF, false, false, false, false)]
		[InlineData(true, 0x7FFFFFFF, 0x0000FFFF, 0x7FFEFFFF, false, false, false, false)]
		[InlineData(true, 0x7FFFFFFF, 0x7FFFFFFF, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(true, 0x7FFFFFFF, 0xFFFFFFFF, 0x7FFFFFFF, true, false, false, false)]
		[InlineData(true, 0xFFFFFFFF, 0x80000000, 0x7FFFFFFE, false, false, false, false)]
		[InlineData(true, 0xFFFFFFFF, 0x00008000, 0xFFFF7FFE, false, false, true, false)]
		[InlineData(true, 0xFFFFFFFF, 0x00000080, 0xFFFFFF7E, false, false, true, false)]
		[InlineData(true, 0xFFFFFFFF, 0x00000000, 0xFFFFFFFE, false, false, true, false)]
		[InlineData(true, 0xFFFFFFFF, 0x0000007F, 0xFFFFFF7F, false, false, true, false)]
		[InlineData(true, 0xFFFFFFFF, 0x000000FF, 0xFFFFFEFF, false, false, true, false)]
		[InlineData(true, 0xFFFFFFFF, 0x00007FFF, 0xFFFF7FFF, false, false, true, false)]
		[InlineData(true, 0xFFFFFFFF, 0x0000FFFF, 0xFFFEFFFF, false, false, true, false)]
		[InlineData(true, 0xFFFFFFFF, 0x7FFFFFFF, 0x7FFFFFFF, false, false, false, true)]
		[InlineData(true, 0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, true, false, true, false)]
		#endregion
		public void Sbb32Test(bool borrow, uint left, uint right, uint result, bool carry, bool zero, bool sign, bool overflow)
		{
			_cpu.Eflags.Carry = borrow;
			_cpu.Sbb(new Register32(left), new Register32(right)).Should().Be(new Register32(result));
			_cpu.Eflags.Carry.Should().Be(carry);
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().Be(overflow);
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80000000, 1, 0)]
		[InlineData(0x80000000, 0x00008000, 0, 1)]
		[InlineData(0x80000000, 0x00000080, 0, 1)]
		[InlineData(0x80000000, 0x00000000, 0, 1)]
		[InlineData(0x80000000, 0x0000007F, 0, 1)]
		[InlineData(0x80000000, 0x000000FF, 0, 1)]
		[InlineData(0x80000000, 0x00007FFF, 0, 1)]
		[InlineData(0x80000000, 0x0000FFFF, 0, 1)]
		[InlineData(0x80000000, 0x7FFFFFFF, 0, 1)]
		[InlineData(0x80000000, 0xFFFFFFFF, 0, 0)]
		[InlineData(0x00008000, 0x80000000, 0, 1)]
		[InlineData(0x00008000, 0x00008000, 1, 0)]
		[InlineData(0x00008000, 0x00000080, 0, 1)]
		[InlineData(0x00008000, 0x00000000, 0, 1)]
		[InlineData(0x00008000, 0x0000007F, 0, 1)]
		[InlineData(0x00008000, 0x000000FF, 0, 1)]
		[InlineData(0x00008000, 0x00007FFF, 0, 1)]
		[InlineData(0x00008000, 0x0000FFFF, 0, 0)]
		[InlineData(0x00008000, 0x7FFFFFFF, 0, 0)]
		[InlineData(0x00008000, 0xFFFFFFFF, 0, 0)]
		[InlineData(0x00000080, 0x80000000, 0, 1)]
		[InlineData(0x00000080, 0x00008000, 0, 1)]
		[InlineData(0x00000080, 0x00000080, 1, 0)]
		[InlineData(0x00000080, 0x00000000, 0, 1)]
		[InlineData(0x00000080, 0x0000007F, 0, 1)]
		[InlineData(0x00000080, 0x000000FF, 0, 0)]
		[InlineData(0x00000080, 0x00007FFF, 0, 0)]
		[InlineData(0x00000080, 0x0000FFFF, 0, 0)]
		[InlineData(0x00000080, 0x7FFFFFFF, 0, 0)]
		[InlineData(0x00000080, 0xFFFFFFFF, 0, 0)]
		[InlineData(0x00000000, 0x80000000, 0, 1)]
		[InlineData(0x00000000, 0x00008000, 0, 1)]
		[InlineData(0x00000000, 0x00000080, 0, 1)]
		[InlineData(0x00000000, 0x00000000, 1, 1)]
		[InlineData(0x00000000, 0x0000007F, 0, 1)]
		[InlineData(0x00000000, 0x000000FF, 0, 1)]
		[InlineData(0x00000000, 0x00007FFF, 0, 1)]
		[InlineData(0x00000000, 0x0000FFFF, 0, 1)]
		[InlineData(0x00000000, 0x7FFFFFFF, 0, 1)]
		[InlineData(0x00000000, 0xFFFFFFFF, 0, 1)]
		[InlineData(0x0000007F, 0x80000000, 0, 1)]
		[InlineData(0x0000007F, 0x00008000, 0, 1)]
		[InlineData(0x0000007F, 0x00000080, 0, 1)]
		[InlineData(0x0000007F, 0x00000000, 0, 1)]
		[InlineData(0x0000007F, 0x0000007F, 1, 0)]
		[InlineData(0x0000007F, 0x000000FF, 0, 0)]
		[InlineData(0x0000007F, 0x00007FFF, 0, 0)]
		[InlineData(0x0000007F, 0x0000FFFF, 0, 0)]
		[InlineData(0x0000007F, 0x7FFFFFFF, 0, 0)]
		[InlineData(0x0000007F, 0xFFFFFFFF, 0, 0)]
		[InlineData(0x000000FF, 0x80000000, 0, 1)]
		[InlineData(0x000000FF, 0x00008000, 0, 1)]
		[InlineData(0x000000FF, 0x00000080, 0, 0)]
		[InlineData(0x000000FF, 0x00000000, 0, 1)]
		[InlineData(0x000000FF, 0x0000007F, 0, 0)]
		[InlineData(0x000000FF, 0x000000FF, 1, 0)]
		[InlineData(0x000000FF, 0x00007FFF, 0, 0)]
		[InlineData(0x000000FF, 0x0000FFFF, 0, 0)]
		[InlineData(0x000000FF, 0x7FFFFFFF, 0, 0)]
		[InlineData(0x000000FF, 0xFFFFFFFF, 0, 0)]
		[InlineData(0x00007FFF, 0x80000000, 0, 1)]
		[InlineData(0x00007FFF, 0x00008000, 0, 1)]
		[InlineData(0x00007FFF, 0x00000080, 0, 0)]
		[InlineData(0x00007FFF, 0x00000000, 0, 1)]
		[InlineData(0x00007FFF, 0x0000007F, 0, 0)]
		[InlineData(0x00007FFF, 0x000000FF, 0, 0)]
		[InlineData(0x00007FFF, 0x00007FFF, 1, 0)]
		[InlineData(0x00007FFF, 0x0000FFFF, 0, 0)]
		[InlineData(0x00007FFF, 0x7FFFFFFF, 0, 0)]
		[InlineData(0x00007FFF, 0xFFFFFFFF, 0, 0)]
		[InlineData(0x0000FFFF, 0x80000000, 0, 1)]
		[InlineData(0x0000FFFF, 0x00008000, 0, 0)]
		[InlineData(0x0000FFFF, 0x00000080, 0, 0)]
		[InlineData(0x0000FFFF, 0x00000000, 0, 1)]
		[InlineData(0x0000FFFF, 0x0000007F, 0, 0)]
		[InlineData(0x0000FFFF, 0x000000FF, 0, 0)]
		[InlineData(0x0000FFFF, 0x00007FFF, 0, 0)]
		[InlineData(0x0000FFFF, 0x0000FFFF, 1, 0)]
		[InlineData(0x0000FFFF, 0x7FFFFFFF, 0, 0)]
		[InlineData(0x0000FFFF, 0xFFFFFFFF, 0, 0)]
		[InlineData(0x7FFFFFFF, 0x80000000, 0, 1)]
		[InlineData(0x7FFFFFFF, 0x00008000, 0, 0)]
		[InlineData(0x7FFFFFFF, 0x00000080, 0, 0)]
		[InlineData(0x7FFFFFFF, 0x00000000, 0, 1)]
		[InlineData(0x7FFFFFFF, 0x0000007F, 0, 0)]
		[InlineData(0x7FFFFFFF, 0x000000FF, 0, 0)]
		[InlineData(0x7FFFFFFF, 0x00007FFF, 0, 0)]
		[InlineData(0x7FFFFFFF, 0x0000FFFF, 0, 0)]
		[InlineData(0x7FFFFFFF, 0x7FFFFFFF, 1, 0)]
		[InlineData(0x7FFFFFFF, 0xFFFFFFFF, 0, 0)]
		[InlineData(0xFFFFFFFF, 0x80000000, 0, 0)]
		[InlineData(0xFFFFFFFF, 0x00008000, 0, 0)]
		[InlineData(0xFFFFFFFF, 0x00000080, 0, 0)]
		[InlineData(0xFFFFFFFF, 0x00000000, 0, 1)]
		[InlineData(0xFFFFFFFF, 0x0000007F, 0, 0)]
		[InlineData(0xFFFFFFFF, 0x000000FF, 0, 0)]
		[InlineData(0xFFFFFFFF, 0x00007FFF, 0, 0)]
		[InlineData(0xFFFFFFFF, 0x0000FFFF, 0, 0)]
		[InlineData(0xFFFFFFFF, 0x7FFFFFFF, 0, 0)]
		[InlineData(0xFFFFFFFF, 0xFFFFFFFF, 1, 0)]
		#endregion
		public void Sete32Test(uint left, uint right, byte cmpResult, byte testResult)
		{
			_cpu.Cmp(new Register32(left), new Register32(right));
			_cpu.Sete().Should().Be(new Register8(cmpResult));

			_cpu.Test(new Register32(left), new Register32(right));
			_cpu.Sete().Should().Be(new Register8(testResult));
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80000000, 0, 0)]
		[InlineData(0x80000000, 0x00008000, 0, 0)]
		[InlineData(0x80000000, 0x00000080, 0, 0)]
		[InlineData(0x80000000, 0x00000000, 0, 0)]
		[InlineData(0x80000000, 0x0000007F, 0, 0)]
		[InlineData(0x80000000, 0x000000FF, 0, 0)]
		[InlineData(0x80000000, 0x00007FFF, 0, 0)]
		[InlineData(0x80000000, 0x0000FFFF, 0, 0)]
		[InlineData(0x80000000, 0x7FFFFFFF, 0, 0)]
		[InlineData(0x80000000, 0xFFFFFFFF, 0, 0)]
		[InlineData(0x00008000, 0x80000000, 1, 0)]
		[InlineData(0x00008000, 0x00008000, 0, 1)]
		[InlineData(0x00008000, 0x00000080, 1, 0)]
		[InlineData(0x00008000, 0x00000000, 1, 0)]
		[InlineData(0x00008000, 0x0000007F, 1, 0)]
		[InlineData(0x00008000, 0x000000FF, 1, 0)]
		[InlineData(0x00008000, 0x00007FFF, 1, 0)]
		[InlineData(0x00008000, 0x0000FFFF, 0, 1)]
		[InlineData(0x00008000, 0x7FFFFFFF, 0, 1)]
		[InlineData(0x00008000, 0xFFFFFFFF, 1, 1)]
		[InlineData(0x00000080, 0x80000000, 1, 0)]
		[InlineData(0x00000080, 0x00008000, 0, 0)]
		[InlineData(0x00000080, 0x00000080, 0, 1)]
		[InlineData(0x00000080, 0x00000000, 1, 0)]
		[InlineData(0x00000080, 0x0000007F, 1, 0)]
		[InlineData(0x00000080, 0x000000FF, 0, 1)]
		[InlineData(0x00000080, 0x00007FFF, 0, 1)]
		[InlineData(0x00000080, 0x0000FFFF, 0, 1)]
		[InlineData(0x00000080, 0x7FFFFFFF, 0, 1)]
		[InlineData(0x00000080, 0xFFFFFFFF, 1, 1)]
		[InlineData(0x00000000, 0x80000000, 1, 0)]
		[InlineData(0x00000000, 0x00008000, 0, 0)]
		[InlineData(0x00000000, 0x00000080, 0, 0)]
		[InlineData(0x00000000, 0x00000000, 0, 0)]
		[InlineData(0x00000000, 0x0000007F, 0, 0)]
		[InlineData(0x00000000, 0x000000FF, 0, 0)]
		[InlineData(0x00000000, 0x00007FFF, 0, 0)]
		[InlineData(0x00000000, 0x0000FFFF, 0, 0)]
		[InlineData(0x00000000, 0x7FFFFFFF, 0, 0)]
		[InlineData(0x00000000, 0xFFFFFFFF, 1, 0)]
		[InlineData(0x0000007F, 0x80000000, 1, 0)]
		[InlineData(0x0000007F, 0x00008000, 0, 0)]
		[InlineData(0x0000007F, 0x00000080, 0, 0)]
		[InlineData(0x0000007F, 0x00000000, 1, 0)]
		[InlineData(0x0000007F, 0x0000007F, 0, 1)]
		[InlineData(0x0000007F, 0x000000FF, 0, 1)]
		[InlineData(0x0000007F, 0x00007FFF, 0, 1)]
		[InlineData(0x0000007F, 0x0000FFFF, 0, 1)]
		[InlineData(0x0000007F, 0x7FFFFFFF, 0, 1)]
		[InlineData(0x0000007F, 0xFFFFFFFF, 1, 1)]
		[InlineData(0x000000FF, 0x80000000, 1, 0)]
		[InlineData(0x000000FF, 0x00008000, 0, 0)]
		[InlineData(0x000000FF, 0x00000080, 1, 1)]
		[InlineData(0x000000FF, 0x00000000, 1, 0)]
		[InlineData(0x000000FF, 0x0000007F, 1, 1)]
		[InlineData(0x000000FF, 0x000000FF, 0, 1)]
		[InlineData(0x000000FF, 0x00007FFF, 0, 1)]
		[InlineData(0x000000FF, 0x0000FFFF, 0, 1)]
		[InlineData(0x000000FF, 0x7FFFFFFF, 0, 1)]
		[InlineData(0x000000FF, 0xFFFFFFFF, 1, 1)]
		[InlineData(0x00007FFF, 0x80000000, 1, 0)]
		[InlineData(0x00007FFF, 0x00008000, 0, 0)]
		[InlineData(0x00007FFF, 0x00000080, 1, 1)]
		[InlineData(0x00007FFF, 0x00000000, 1, 0)]
		[InlineData(0x00007FFF, 0x0000007F, 1, 1)]
		[InlineData(0x00007FFF, 0x000000FF, 1, 1)]
		[InlineData(0x00007FFF, 0x00007FFF, 0, 1)]
		[InlineData(0x00007FFF, 0x0000FFFF, 0, 1)]
		[InlineData(0x00007FFF, 0x7FFFFFFF, 0, 1)]
		[InlineData(0x00007FFF, 0xFFFFFFFF, 1, 1)]
		[InlineData(0x0000FFFF, 0x80000000, 1, 0)]
		[InlineData(0x0000FFFF, 0x00008000, 1, 1)]
		[InlineData(0x0000FFFF, 0x00000080, 1, 1)]
		[InlineData(0x0000FFFF, 0x00000000, 1, 0)]
		[InlineData(0x0000FFFF, 0x0000007F, 1, 1)]
		[InlineData(0x0000FFFF, 0x000000FF, 1, 1)]
		[InlineData(0x0000FFFF, 0x00007FFF, 1, 1)]
		[InlineData(0x0000FFFF, 0x0000FFFF, 0, 1)]
		[InlineData(0x0000FFFF, 0x7FFFFFFF, 0, 1)]
		[InlineData(0x0000FFFF, 0xFFFFFFFF, 1, 1)]
		[InlineData(0x7FFFFFFF, 0x80000000, 1, 0)]
		[InlineData(0x7FFFFFFF, 0x00008000, 1, 1)]
		[InlineData(0x7FFFFFFF, 0x00000080, 1, 1)]
		[InlineData(0x7FFFFFFF, 0x00000000, 1, 0)]
		[InlineData(0x7FFFFFFF, 0x0000007F, 1, 1)]
		[InlineData(0x7FFFFFFF, 0x000000FF, 1, 1)]
		[InlineData(0x7FFFFFFF, 0x00007FFF, 1, 1)]
		[InlineData(0x7FFFFFFF, 0x0000FFFF, 1, 1)]
		[InlineData(0x7FFFFFFF, 0x7FFFFFFF, 0, 1)]
		[InlineData(0x7FFFFFFF, 0xFFFFFFFF, 1, 1)]
		[InlineData(0xFFFFFFFF, 0x80000000, 1, 0)]
		[InlineData(0xFFFFFFFF, 0x00008000, 0, 1)]
		[InlineData(0xFFFFFFFF, 0x00000080, 0, 1)]
		[InlineData(0xFFFFFFFF, 0x00000000, 0, 0)]
		[InlineData(0xFFFFFFFF, 0x0000007F, 0, 1)]
		[InlineData(0xFFFFFFFF, 0x000000FF, 0, 1)]
		[InlineData(0xFFFFFFFF, 0x00007FFF, 0, 1)]
		[InlineData(0xFFFFFFFF, 0x0000FFFF, 0, 1)]
		[InlineData(0xFFFFFFFF, 0x7FFFFFFF, 0, 1)]
		[InlineData(0xFFFFFFFF, 0xFFFFFFFF, 0, 0)]
		#endregion
		public void Setg32Test(uint left, uint right, byte cmpResult, byte testResult)
		{
			_cpu.Cmp(new Register32(left), new Register32(right));
			_cpu.Setg().Should().Be(new Register8(cmpResult));

			_cpu.Test(new Register32(left), new Register32(right));
			_cpu.Setg().Should().Be(new Register8(testResult));
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x01, 0x00000000, true, true, false, true)]
		[InlineData(0x80000000, 0x04, 0x00000000, false, true, false, null)]
		[InlineData(0x80000000, 0x20, 0x80000000, null, null, null, null)]
		[InlineData(0x80000000, 0x21, 0x00000000, null, true, false, true)]
		[InlineData(0x00008000, 0x01, 0x00010000, false, false, false, false)]
		[InlineData(0x00008000, 0x04, 0x00080000, false, false, false, null)]
		[InlineData(0x00008000, 0x20, 0x00008000, null, null, null, null)]
		[InlineData(0x00008000, 0x21, 0x00010000, null, false, false, false)]
		[InlineData(0x00000080, 0x01, 0x00000100, false, false, false, false)]
		[InlineData(0x00000080, 0x04, 0x00000800, false, false, false, null)]
		[InlineData(0x00000080, 0x20, 0x00000080, null, null, null, null)]
		[InlineData(0x00000080, 0x21, 0x00000100, null, false, false, false)]
		[InlineData(0x00000000, 0x01, 0x00000000, false, true, false, false)]
		[InlineData(0x00000000, 0x04, 0x00000000, false, true, false, null)]
		[InlineData(0x00000000, 0x20, 0x00000000, null, null, null, null)]
		[InlineData(0x00000000, 0x21, 0x00000000, null, true, false, false)]
		[InlineData(0x0000007F, 0x01, 0x000000FE, false, false, false, false)]
		[InlineData(0x0000007F, 0x04, 0x000007F0, false, false, false, null)]
		[InlineData(0x0000007F, 0x20, 0x0000007F, null, null, null, null)]
		[InlineData(0x0000007F, 0x21, 0x000000FE, null, false, false, false)]
		[InlineData(0x000000FF, 0x01, 0x000001FE, false, false, false, false)]
		[InlineData(0x000000FF, 0x04, 0x00000FF0, false, false, false, null)]
		[InlineData(0x000000FF, 0x20, 0x000000FF, null, null, null, null)]
		[InlineData(0x000000FF, 0x21, 0x000001FE, null, false, false, false)]
		[InlineData(0x00007FFF, 0x01, 0x0000FFFE, false, false, false, false)]
		[InlineData(0x00007FFF, 0x04, 0x0007FFF0, false, false, false, null)]
		[InlineData(0x00007FFF, 0x20, 0x00007FFF, null, null, null, null)]
		[InlineData(0x00007FFF, 0x21, 0x0000FFFE, null, false, false, false)]
		[InlineData(0x0000FFFF, 0x01, 0x0001FFFE, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x04, 0x000FFFF0, false, false, false, null)]
		[InlineData(0x0000FFFF, 0x20, 0x0000FFFF, null, null, null, null)]
		[InlineData(0x0000FFFF, 0x21, 0x0001FFFE, null, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x01, 0xFFFFFFFE, false, false, true, true)]
		[InlineData(0x7FFFFFFF, 0x04, 0xFFFFFFF0, true, false, true, null)]
		[InlineData(0x7FFFFFFF, 0x20, 0x7FFFFFFF, null, null, null, null)]
		[InlineData(0x7FFFFFFF, 0x21, 0xFFFFFFFE, null, false, true, true)]
		[InlineData(0xFFFFFFFF, 0x01, 0xFFFFFFFE, true, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x04, 0xFFFFFFF0, true, false, true, null)]
		[InlineData(0xFFFFFFFF, 0x20, 0xFFFFFFFF, null, null, null, null)]
		[InlineData(0xFFFFFFFF, 0x21, 0xFFFFFFFE, null, false, true, false)]
		#endregion
		public void Shl32Test(uint value, byte count, uint result, bool? carry, bool? zero, bool? sign, bool? overflow)
		{
			_cpu.Shl(new Register32(value), new Register8(count)).Should().Be(new Register32(result));
			if (carry.HasValue)
				_cpu.Eflags.Carry.Should().Be(carry.Value);
			if (zero.HasValue)
				_cpu.Eflags.Zero.Should().Be(zero.Value);
			if (sign.HasValue)
				_cpu.Eflags.Sign.Should().Be(sign.Value);
			if (overflow.HasValue)
				_cpu.Eflags.Overflow.Should().Be(overflow.Value);
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x01, 0x40000000, false, false, false, true)]
		[InlineData(0x80000000, 0x04, 0x08000000, false, false, false, null)]
		[InlineData(0x80000000, 0x20, 0x80000000, null, null, null, null)]
		[InlineData(0x80000000, 0x21, 0x40000000, false, false, false, true)]
		[InlineData(0x00008000, 0x01, 0x00004000, false, false, false, false)]
		[InlineData(0x00008000, 0x04, 0x00000800, false, false, false, null)]
		[InlineData(0x00008000, 0x20, 0x00008000, null, null, null, null)]
		[InlineData(0x00008000, 0x21, 0x00004000, false, false, false, false)]
		[InlineData(0x00000080, 0x01, 0x00000040, false, false, false, false)]
		[InlineData(0x00000080, 0x04, 0x00000008, false, false, false, null)]
		[InlineData(0x00000080, 0x20, 0x00000080, null, null, null, null)]
		[InlineData(0x00000080, 0x21, 0x00000040, false, false, false, false)]
		[InlineData(0x00000000, 0x01, 0x00000000, false, true, false, false)]
		[InlineData(0x00000000, 0x04, 0x00000000, false, true, false, null)]
		[InlineData(0x00000000, 0x20, 0x00000000, null, null, null, null)]
		[InlineData(0x00000000, 0x21, 0x00000000, false, true, false, false)]
		[InlineData(0x0000007F, 0x01, 0x0000003F, true, false, false, false)]
		[InlineData(0x0000007F, 0x04, 0x00000007, true, false, false, null)]
		[InlineData(0x0000007F, 0x20, 0x0000007F, null, null, null, null)]
		[InlineData(0x0000007F, 0x21, 0x0000003F, true, false, false, false)]
		[InlineData(0x000000FF, 0x01, 0x0000007F, true, false, false, false)]
		[InlineData(0x000000FF, 0x04, 0x0000000F, true, false, false, null)]
		[InlineData(0x000000FF, 0x20, 0x000000FF, null, null, null, null)]
		[InlineData(0x000000FF, 0x21, 0x0000007F, true, false, false, false)]
		[InlineData(0x00007FFF, 0x01, 0x00003FFF, true, false, false, false)]
		[InlineData(0x00007FFF, 0x04, 0x000007FF, true, false, false, null)]
		[InlineData(0x00007FFF, 0x20, 0x00007FFF, null, null, null, null)]
		[InlineData(0x00007FFF, 0x21, 0x00003FFF, true, false, false, false)]
		[InlineData(0x0000FFFF, 0x01, 0x00007FFF, true, false, false, false)]
		[InlineData(0x0000FFFF, 0x04, 0x00000FFF, true, false, false, null)]
		[InlineData(0x0000FFFF, 0x20, 0x0000FFFF, null, null, null, null)]
		[InlineData(0x0000FFFF, 0x21, 0x00007FFF, true, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x01, 0x3FFFFFFF, true, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x04, 0x07FFFFFF, true, false, false, null)]
		[InlineData(0x7FFFFFFF, 0x20, 0x7FFFFFFF, null, null, null, null)]
		[InlineData(0x7FFFFFFF, 0x21, 0x3FFFFFFF, true, false, false, false)]
		[InlineData(0xFFFFFFFF, 0x01, 0x7FFFFFFF, true, false, false, true)]
		[InlineData(0xFFFFFFFF, 0x04, 0x0FFFFFFF, true, false, false, null)]
		[InlineData(0xFFFFFFFF, 0x20, 0xFFFFFFFF, null, null, null, null)]
		[InlineData(0xFFFFFFFF, 0x21, 0x7FFFFFFF, true, false, false, true)]
		#endregion
		public void Shr32Test(uint value, byte count, uint result, bool? carry, bool? zero, bool? sign, bool? overflow)
		{
			_cpu.Shr(new Register32(value), new Register8(count)).Should().Be(new Register32(result));
			if (carry.HasValue)
				_cpu.Eflags.Carry.Should().Be(carry.Value);
			if (zero.HasValue)
				_cpu.Eflags.Zero.Should().Be(zero.Value);
			if (sign.HasValue)
				_cpu.Eflags.Sign.Should().Be(sign.Value);
			if (overflow.HasValue)
				_cpu.Eflags.Overflow.Should().Be(overflow.Value);
		}

		[Theory]
		#region
		[InlineData(0x8000, 0x8000, 0x0000, false, true, false, false)]
		[InlineData(0x8000, 0x0080, 0x7F80, false, false, false, true)]
		[InlineData(0x8000, 0x0000, 0x8000, false, false, true, false)]
		[InlineData(0x8000, 0x007F, 0x7F81, false, false, false, true)]
		[InlineData(0x8000, 0x00FF, 0x7F01, false, false, false, true)]
		[InlineData(0x8000, 0x7FFF, 0x0001, false, false, false, true)]
		[InlineData(0x8000, 0xFFFF, 0x8001, true, false, true, false)]
		[InlineData(0x0080, 0x8000, 0x8080, true, false, true, true)]
		[InlineData(0x0080, 0x0080, 0x0000, false, true, false, false)]
		[InlineData(0x0080, 0x0000, 0x0080, false, false, false, false)]
		[InlineData(0x0080, 0x007F, 0x0001, false, false, false, false)]
		[InlineData(0x0080, 0x00FF, 0xFF81, true, false, true, false)]
		[InlineData(0x0080, 0x7FFF, 0x8081, true, false, true, false)]
		[InlineData(0x0080, 0xFFFF, 0x0081, true, false, false, false)]
		[InlineData(0x0000, 0x8000, 0x8000, true, false, true, true)]
		[InlineData(0x0000, 0x0080, 0xFF80, true, false, true, false)]
		[InlineData(0x0000, 0x0000, 0x0000, false, true, false, false)]
		[InlineData(0x0000, 0x007F, 0xFF81, true, false, true, false)]
		[InlineData(0x0000, 0x00FF, 0xFF01, true, false, true, false)]
		[InlineData(0x0000, 0x7FFF, 0x8001, true, false, true, false)]
		[InlineData(0x0000, 0xFFFF, 0x0001, true, false, false, false)]
		[InlineData(0x007F, 0x8000, 0x807F, true, false, true, true)]
		[InlineData(0x007F, 0x0080, 0xFFFF, true, false, true, false)]
		[InlineData(0x007F, 0x0000, 0x007F, false, false, false, false)]
		[InlineData(0x007F, 0x007F, 0x0000, false, true, false, false)]
		[InlineData(0x007F, 0x00FF, 0xFF80, true, false, true, false)]
		[InlineData(0x007F, 0x7FFF, 0x8080, true, false, true, false)]
		[InlineData(0x007F, 0xFFFF, 0x0080, true, false, false, false)]
		[InlineData(0x00FF, 0x8000, 0x80FF, true, false, true, true)]
		[InlineData(0x00FF, 0x0080, 0x007F, false, false, false, false)]
		[InlineData(0x00FF, 0x0000, 0x00FF, false, false, false, false)]
		[InlineData(0x00FF, 0x007F, 0x0080, false, false, false, false)]
		[InlineData(0x00FF, 0x00FF, 0x0000, false, true, false, false)]
		[InlineData(0x00FF, 0x7FFF, 0x8100, true, false, true, false)]
		[InlineData(0x00FF, 0xFFFF, 0x0100, true, false, false, false)]
		[InlineData(0x7FFF, 0x8000, 0xFFFF, true, false, true, true)]
		[InlineData(0x7FFF, 0x0080, 0x7F7F, false, false, false, false)]
		[InlineData(0x7FFF, 0x0000, 0x7FFF, false, false, false, false)]
		[InlineData(0x7FFF, 0x007F, 0x7F80, false, false, false, false)]
		[InlineData(0x7FFF, 0x00FF, 0x7F00, false, false, false, false)]
		[InlineData(0x7FFF, 0x7FFF, 0x0000, false, true, false, false)]
		[InlineData(0x7FFF, 0xFFFF, 0x8000, true, false, true, true)]
		[InlineData(0xFFFF, 0x8000, 0x7FFF, false, false, false, false)]
		[InlineData(0xFFFF, 0x0080, 0xFF7F, false, false, true, false)]
		[InlineData(0xFFFF, 0x0000, 0xFFFF, false, false, true, false)]
		[InlineData(0xFFFF, 0x007F, 0xFF80, false, false, true, false)]
		[InlineData(0xFFFF, 0x00FF, 0xFF00, false, false, true, false)]
		[InlineData(0xFFFF, 0x7FFF, 0x8000, false, false, true, false)]
		[InlineData(0xFFFF, 0xFFFF, 0x0000, false, true, false, false)]
		#endregion
		public void Sub16Test(ushort left, ushort right, ushort result, bool carry, bool zero, bool sign, bool overflow)
		{
			_cpu.Sub(new Register16(left), new Register16(right)).Should().Be(new Register16(result));
			_cpu.Eflags.Carry.Should().Be(carry);
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().Be(overflow);
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80000000, 0x00000000, false, true, false, false)]
		[InlineData(0x80000000, 0x00008000, 0x7FFF8000, false, false, false, true)]
		[InlineData(0x80000000, 0x00000080, 0x7FFFFF80, false, false, false, true)]
		[InlineData(0x80000000, 0x00000000, 0x80000000, false, false, true, false)]
		[InlineData(0x80000000, 0x0000007F, 0x7FFFFF81, false, false, false, true)]
		[InlineData(0x80000000, 0x000000FF, 0x7FFFFF01, false, false, false, true)]
		[InlineData(0x80000000, 0x00007FFF, 0x7FFF8001, false, false, false, true)]
		[InlineData(0x80000000, 0x0000FFFF, 0x7FFF0001, false, false, false, true)]
		[InlineData(0x80000000, 0x7FFFFFFF, 0x00000001, false, false, false, true)]
		[InlineData(0x80000000, 0xFFFFFFFF, 0x80000001, true, false, true, false)]
		[InlineData(0x00008000, 0x80000000, 0x80008000, true, false, true, true)]
		[InlineData(0x00008000, 0x00008000, 0x00000000, false, true, false, false)]
		[InlineData(0x00008000, 0x00000080, 0x00007F80, false, false, false, false)]
		[InlineData(0x00008000, 0x00000000, 0x00008000, false, false, false, false)]
		[InlineData(0x00008000, 0x0000007F, 0x00007F81, false, false, false, false)]
		[InlineData(0x00008000, 0x000000FF, 0x00007F01, false, false, false, false)]
		[InlineData(0x00008000, 0x00007FFF, 0x00000001, false, false, false, false)]
		[InlineData(0x00008000, 0x0000FFFF, 0xFFFF8001, true, false, true, false)]
		[InlineData(0x00008000, 0x7FFFFFFF, 0x80008001, true, false, true, false)]
		[InlineData(0x00008000, 0xFFFFFFFF, 0x00008001, true, false, false, false)]
		[InlineData(0x00000080, 0x80000000, 0x80000080, true, false, true, true)]
		[InlineData(0x00000080, 0x00008000, 0xFFFF8080, true, false, true, false)]
		[InlineData(0x00000080, 0x00000080, 0x00000000, false, true, false, false)]
		[InlineData(0x00000080, 0x00000000, 0x00000080, false, false, false, false)]
		[InlineData(0x00000080, 0x0000007F, 0x00000001, false, false, false, false)]
		[InlineData(0x00000080, 0x000000FF, 0xFFFFFF81, true, false, true, false)]
		[InlineData(0x00000080, 0x00007FFF, 0xFFFF8081, true, false, true, false)]
		[InlineData(0x00000080, 0x0000FFFF, 0xFFFF0081, true, false, true, false)]
		[InlineData(0x00000080, 0x7FFFFFFF, 0x80000081, true, false, true, false)]
		[InlineData(0x00000080, 0xFFFFFFFF, 0x00000081, true, false, false, false)]
		[InlineData(0x00000000, 0x80000000, 0x80000000, true, false, true, true)]
		[InlineData(0x00000000, 0x00008000, 0xFFFF8000, true, false, true, false)]
		[InlineData(0x00000000, 0x00000080, 0xFFFFFF80, true, false, true, false)]
		[InlineData(0x00000000, 0x00000000, 0x00000000, false, true, false, false)]
		[InlineData(0x00000000, 0x0000007F, 0xFFFFFF81, true, false, true, false)]
		[InlineData(0x00000000, 0x000000FF, 0xFFFFFF01, true, false, true, false)]
		[InlineData(0x00000000, 0x00007FFF, 0xFFFF8001, true, false, true, false)]
		[InlineData(0x00000000, 0x0000FFFF, 0xFFFF0001, true, false, true, false)]
		[InlineData(0x00000000, 0x7FFFFFFF, 0x80000001, true, false, true, false)]
		[InlineData(0x00000000, 0xFFFFFFFF, 0x00000001, true, false, false, false)]
		[InlineData(0x0000007F, 0x80000000, 0x8000007F, true, false, true, true)]
		[InlineData(0x0000007F, 0x00008000, 0xFFFF807F, true, false, true, false)]
		[InlineData(0x0000007F, 0x00000080, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(0x0000007F, 0x00000000, 0x0000007F, false, false, false, false)]
		[InlineData(0x0000007F, 0x0000007F, 0x00000000, false, true, false, false)]
		[InlineData(0x0000007F, 0x000000FF, 0xFFFFFF80, true, false, true, false)]
		[InlineData(0x0000007F, 0x00007FFF, 0xFFFF8080, true, false, true, false)]
		[InlineData(0x0000007F, 0x0000FFFF, 0xFFFF0080, true, false, true, false)]
		[InlineData(0x0000007F, 0x7FFFFFFF, 0x80000080, true, false, true, false)]
		[InlineData(0x0000007F, 0xFFFFFFFF, 0x00000080, true, false, false, false)]
		[InlineData(0x000000FF, 0x80000000, 0x800000FF, true, false, true, true)]
		[InlineData(0x000000FF, 0x00008000, 0xFFFF80FF, true, false, true, false)]
		[InlineData(0x000000FF, 0x00000080, 0x0000007F, false, false, false, false)]
		[InlineData(0x000000FF, 0x00000000, 0x000000FF, false, false, false, false)]
		[InlineData(0x000000FF, 0x0000007F, 0x00000080, false, false, false, false)]
		[InlineData(0x000000FF, 0x000000FF, 0x00000000, false, true, false, false)]
		[InlineData(0x000000FF, 0x00007FFF, 0xFFFF8100, true, false, true, false)]
		[InlineData(0x000000FF, 0x0000FFFF, 0xFFFF0100, true, false, true, false)]
		[InlineData(0x000000FF, 0x7FFFFFFF, 0x80000100, true, false, true, false)]
		[InlineData(0x000000FF, 0xFFFFFFFF, 0x00000100, true, false, false, false)]
		[InlineData(0x00007FFF, 0x80000000, 0x80007FFF, true, false, true, true)]
		[InlineData(0x00007FFF, 0x00008000, 0xFFFFFFFF, true, false, true, false)]
		[InlineData(0x00007FFF, 0x00000080, 0x00007F7F, false, false, false, false)]
		[InlineData(0x00007FFF, 0x00000000, 0x00007FFF, false, false, false, false)]
		[InlineData(0x00007FFF, 0x0000007F, 0x00007F80, false, false, false, false)]
		[InlineData(0x00007FFF, 0x000000FF, 0x00007F00, false, false, false, false)]
		[InlineData(0x00007FFF, 0x00007FFF, 0x00000000, false, true, false, false)]
		[InlineData(0x00007FFF, 0x0000FFFF, 0xFFFF8000, true, false, true, false)]
		[InlineData(0x00007FFF, 0x7FFFFFFF, 0x80008000, true, false, true, false)]
		[InlineData(0x00007FFF, 0xFFFFFFFF, 0x00008000, true, false, false, false)]
		[InlineData(0x0000FFFF, 0x80000000, 0x8000FFFF, true, false, true, true)]
		[InlineData(0x0000FFFF, 0x00008000, 0x00007FFF, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x00000080, 0x0000FF7F, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x00000000, 0x0000FFFF, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x0000007F, 0x0000FF80, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x000000FF, 0x0000FF00, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x00007FFF, 0x00008000, false, false, false, false)]
		[InlineData(0x0000FFFF, 0x0000FFFF, 0x00000000, false, true, false, false)]
		[InlineData(0x0000FFFF, 0x7FFFFFFF, 0x80010000, true, false, true, false)]
		[InlineData(0x0000FFFF, 0xFFFFFFFF, 0x00010000, true, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x80000000, 0xFFFFFFFF, true, false, true, true)]
		[InlineData(0x7FFFFFFF, 0x00008000, 0x7FFF7FFF, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x00000080, 0x7FFFFF7F, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x00000000, 0x7FFFFFFF, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x0000007F, 0x7FFFFF80, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x000000FF, 0x7FFFFF00, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x00007FFF, 0x7FFF8000, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x0000FFFF, 0x7FFF0000, false, false, false, false)]
		[InlineData(0x7FFFFFFF, 0x7FFFFFFF, 0x00000000, false, true, false, false)]
		[InlineData(0x7FFFFFFF, 0xFFFFFFFF, 0x80000000, true, false, true, true)]
		[InlineData(0xFFFFFFFF, 0x80000000, 0x7FFFFFFF, false, false, false, false)]
		[InlineData(0xFFFFFFFF, 0x00008000, 0xFFFF7FFF, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x00000080, 0xFFFFFF7F, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x00000000, 0xFFFFFFFF, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x0000007F, 0xFFFFFF80, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x000000FF, 0xFFFFFF00, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x00007FFF, 0xFFFF8000, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x0000FFFF, 0xFFFF0000, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0x7FFFFFFF, 0x80000000, false, false, true, false)]
		[InlineData(0xFFFFFFFF, 0xFFFFFFFF, 0x00000000, false, true, false, false)]
		#endregion
		public void Sub32Test(uint left, uint right, uint result, bool carry, bool zero, bool sign, bool overflow)
		{
			_cpu.Sub(new Register32(left), new Register32(right)).Should().Be(new Register32(result));
			_cpu.Eflags.Carry.Should().Be(carry);
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().Be(overflow);
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80, 0x80000080)]
		[InlineData(0x80000000, 0x00, 0x80000000)]
		[InlineData(0x80000000, 0x7F, 0x7FFFFF81)]
		[InlineData(0x80000000, 0xFF, 0x80000001)]
		[InlineData(0x00008000, 0x80, 0x00008080)]
		[InlineData(0x00008000, 0x00, 0x00008000)]
		[InlineData(0x00008000, 0x7F, 0x00007F81)]
		[InlineData(0x00008000, 0xFF, 0x00008001)]
		[InlineData(0x00000080, 0x80, 0x00000100)]
		[InlineData(0x00000080, 0x00, 0x00000080)]
		[InlineData(0x00000080, 0x7F, 0x00000001)]
		[InlineData(0x00000080, 0xFF, 0x00000081)]
		[InlineData(0x00000000, 0x80, 0x00000080)]
		[InlineData(0x00000000, 0x00, 0x00000000)]
		[InlineData(0x00000000, 0x7F, 0xFFFFFF81)]
		[InlineData(0x00000000, 0xFF, 0x00000001)]
		[InlineData(0x0000007F, 0x80, 0x000000FF)]
		[InlineData(0x0000007F, 0x00, 0x0000007F)]
		[InlineData(0x0000007F, 0x7F, 0x00000000)]
		[InlineData(0x0000007F, 0xFF, 0x00000080)]
		[InlineData(0x000000FF, 0x80, 0x0000017F)]
		[InlineData(0x000000FF, 0x00, 0x000000FF)]
		[InlineData(0x000000FF, 0x7F, 0x00000080)]
		[InlineData(0x000000FF, 0xFF, 0x00000100)]
		[InlineData(0x00007FFF, 0x80, 0x0000807F)]
		[InlineData(0x00007FFF, 0x00, 0x00007FFF)]
		[InlineData(0x00007FFF, 0x7F, 0x00007F80)]
		[InlineData(0x00007FFF, 0xFF, 0x00008000)]
		[InlineData(0x0000FFFF, 0x80, 0x0001007F)]
		[InlineData(0x0000FFFF, 0x00, 0x0000FFFF)]
		[InlineData(0x0000FFFF, 0x7F, 0x0000FF80)]
		[InlineData(0x0000FFFF, 0xFF, 0x00010000)]
		[InlineData(0x7FFFFFFF, 0x80, 0x8000007F)]
		[InlineData(0x7FFFFFFF, 0x00, 0x7FFFFFFF)]
		[InlineData(0x7FFFFFFF, 0x7F, 0x7FFFFF80)]
		[InlineData(0x7FFFFFFF, 0xFF, 0x80000000)]
		[InlineData(0xFFFFFFFF, 0x80, 0x0000007F)]
		[InlineData(0xFFFFFFFF, 0x00, 0xFFFFFFFF)]
		[InlineData(0xFFFFFFFF, 0x7F, 0xFFFFFF80)]
		[InlineData(0xFFFFFFFF, 0xFF, 0x00000000)]
		#endregion
		public void Sub32_8Test(uint left, byte right, uint result)
		{
			_cpu.Sub(new Register32(left), new Register8(right)).Should().Be(new Register32(result));
		}

		[Theory]
		#region
		[InlineData(0x80, 0x80, false, true)]
		[InlineData(0x80, 0x00, true, false)]
		[InlineData(0x80, 0x7F, true, false)]
		[InlineData(0x80, 0xFF, false, true)]
		[InlineData(0x00, 0x80, true, false)]
		[InlineData(0x00, 0x00, true, false)]
		[InlineData(0x00, 0x7F, true, false)]
		[InlineData(0x00, 0xFF, true, false)]
		[InlineData(0x7F, 0x80, true, false)]
		[InlineData(0x7F, 0x00, true, false)]
		[InlineData(0x7F, 0x7F, false, false)]
		[InlineData(0x7F, 0xFF, false, false)]
		[InlineData(0xFF, 0x80, false, true)]
		[InlineData(0xFF, 0x00, true, false)]
		[InlineData(0xFF, 0x7F, false, false)]
		[InlineData(0xFF, 0xFF, false, true)]
		#endregion
		public void Test8Test(byte left, byte right, bool zero, bool sign)
		{
			_cpu.Test(new Register8(left), new Register8(right));
			_cpu.Eflags.Carry.Should().BeFalse();
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().BeFalse();
		}

		[Theory]
		#region
		[InlineData(0x8000, 0x8000, false, true)]
		[InlineData(0x8000, 0x0080, true, false)]
		[InlineData(0x8000, 0x0000, true, false)]
		[InlineData(0x8000, 0x007F, true, false)]
		[InlineData(0x8000, 0x00FF, true, false)]
		[InlineData(0x8000, 0x7FFF, true, false)]
		[InlineData(0x8000, 0xFFFF, false, true)]
		[InlineData(0x0080, 0x8000, true, false)]
		[InlineData(0x0080, 0x0080, false, false)]
		[InlineData(0x0080, 0x0000, true, false)]
		[InlineData(0x0080, 0x007F, true, false)]
		[InlineData(0x0080, 0x00FF, false, false)]
		[InlineData(0x0080, 0x7FFF, false, false)]
		[InlineData(0x0080, 0xFFFF, false, false)]
		[InlineData(0x0000, 0x8000, true, false)]
		[InlineData(0x0000, 0x0080, true, false)]
		[InlineData(0x0000, 0x0000, true, false)]
		[InlineData(0x0000, 0x007F, true, false)]
		[InlineData(0x0000, 0x00FF, true, false)]
		[InlineData(0x0000, 0x7FFF, true, false)]
		[InlineData(0x0000, 0xFFFF, true, false)]
		[InlineData(0x007F, 0x8000, true, false)]
		[InlineData(0x007F, 0x0080, true, false)]
		[InlineData(0x007F, 0x0000, true, false)]
		[InlineData(0x007F, 0x007F, false, false)]
		[InlineData(0x007F, 0x00FF, false, false)]
		[InlineData(0x007F, 0x7FFF, false, false)]
		[InlineData(0x007F, 0xFFFF, false, false)]
		[InlineData(0x00FF, 0x8000, true, false)]
		[InlineData(0x00FF, 0x0080, false, false)]
		[InlineData(0x00FF, 0x0000, true, false)]
		[InlineData(0x00FF, 0x007F, false, false)]
		[InlineData(0x00FF, 0x00FF, false, false)]
		[InlineData(0x00FF, 0x7FFF, false, false)]
		[InlineData(0x00FF, 0xFFFF, false, false)]
		[InlineData(0x7FFF, 0x8000, true, false)]
		[InlineData(0x7FFF, 0x0080, false, false)]
		[InlineData(0x7FFF, 0x0000, true, false)]
		[InlineData(0x7FFF, 0x007F, false, false)]
		[InlineData(0x7FFF, 0x00FF, false, false)]
		[InlineData(0x7FFF, 0x7FFF, false, false)]
		[InlineData(0x7FFF, 0xFFFF, false, false)]
		[InlineData(0xFFFF, 0x8000, false, true)]
		[InlineData(0xFFFF, 0x0080, false, false)]
		[InlineData(0xFFFF, 0x0000, true, false)]
		[InlineData(0xFFFF, 0x007F, false, false)]
		[InlineData(0xFFFF, 0x00FF, false, false)]
		[InlineData(0xFFFF, 0x7FFF, false, false)]
		[InlineData(0xFFFF, 0xFFFF, false, true)]
		#endregion
		public void Test16Test(ushort left, ushort right, bool zero, bool sign)
		{
			_cpu.Test(new Register16(left), new Register16(right));
			_cpu.Eflags.Carry.Should().BeFalse();
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().BeFalse();
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80000000, false, true)]
		[InlineData(0x80000000, 0x00008000, true, false)]
		[InlineData(0x80000000, 0x00000080, true, false)]
		[InlineData(0x80000000, 0x00000000, true, false)]
		[InlineData(0x80000000, 0x0000007F, true, false)]
		[InlineData(0x80000000, 0x000000FF, true, false)]
		[InlineData(0x80000000, 0x00007FFF, true, false)]
		[InlineData(0x80000000, 0x0000FFFF, true, false)]
		[InlineData(0x80000000, 0x7FFFFFFF, true, false)]
		[InlineData(0x80000000, 0xFFFFFFFF, false, true)]
		[InlineData(0x00008000, 0x80000000, true, false)]
		[InlineData(0x00008000, 0x00008000, false, false)]
		[InlineData(0x00008000, 0x00000080, true, false)]
		[InlineData(0x00008000, 0x00000000, true, false)]
		[InlineData(0x00008000, 0x0000007F, true, false)]
		[InlineData(0x00008000, 0x000000FF, true, false)]
		[InlineData(0x00008000, 0x00007FFF, true, false)]
		[InlineData(0x00008000, 0x0000FFFF, false, false)]
		[InlineData(0x00008000, 0x7FFFFFFF, false, false)]
		[InlineData(0x00008000, 0xFFFFFFFF, false, false)]
		[InlineData(0x00000080, 0x80000000, true, false)]
		[InlineData(0x00000080, 0x00008000, true, false)]
		[InlineData(0x00000080, 0x00000080, false, false)]
		[InlineData(0x00000080, 0x00000000, true, false)]
		[InlineData(0x00000080, 0x0000007F, true, false)]
		[InlineData(0x00000080, 0x000000FF, false, false)]
		[InlineData(0x00000080, 0x00007FFF, false, false)]
		[InlineData(0x00000080, 0x0000FFFF, false, false)]
		[InlineData(0x00000080, 0x7FFFFFFF, false, false)]
		[InlineData(0x00000080, 0xFFFFFFFF, false, false)]
		[InlineData(0x00000000, 0x80000000, true, false)]
		[InlineData(0x00000000, 0x00008000, true, false)]
		[InlineData(0x00000000, 0x00000080, true, false)]
		[InlineData(0x00000000, 0x00000000, true, false)]
		[InlineData(0x00000000, 0x0000007F, true, false)]
		[InlineData(0x00000000, 0x000000FF, true, false)]
		[InlineData(0x00000000, 0x00007FFF, true, false)]
		[InlineData(0x00000000, 0x0000FFFF, true, false)]
		[InlineData(0x00000000, 0x7FFFFFFF, true, false)]
		[InlineData(0x00000000, 0xFFFFFFFF, true, false)]
		[InlineData(0x0000007F, 0x80000000, true, false)]
		[InlineData(0x0000007F, 0x00008000, true, false)]
		[InlineData(0x0000007F, 0x00000080, true, false)]
		[InlineData(0x0000007F, 0x00000000, true, false)]
		[InlineData(0x0000007F, 0x0000007F, false, false)]
		[InlineData(0x0000007F, 0x000000FF, false, false)]
		[InlineData(0x0000007F, 0x00007FFF, false, false)]
		[InlineData(0x0000007F, 0x0000FFFF, false, false)]
		[InlineData(0x0000007F, 0x7FFFFFFF, false, false)]
		[InlineData(0x0000007F, 0xFFFFFFFF, false, false)]
		[InlineData(0x000000FF, 0x80000000, true, false)]
		[InlineData(0x000000FF, 0x00008000, true, false)]
		[InlineData(0x000000FF, 0x00000080, false, false)]
		[InlineData(0x000000FF, 0x00000000, true, false)]
		[InlineData(0x000000FF, 0x0000007F, false, false)]
		[InlineData(0x000000FF, 0x000000FF, false, false)]
		[InlineData(0x000000FF, 0x00007FFF, false, false)]
		[InlineData(0x000000FF, 0x0000FFFF, false, false)]
		[InlineData(0x000000FF, 0x7FFFFFFF, false, false)]
		[InlineData(0x000000FF, 0xFFFFFFFF, false, false)]
		[InlineData(0x00007FFF, 0x80000000, true, false)]
		[InlineData(0x00007FFF, 0x00008000, true, false)]
		[InlineData(0x00007FFF, 0x00000080, false, false)]
		[InlineData(0x00007FFF, 0x00000000, true, false)]
		[InlineData(0x00007FFF, 0x0000007F, false, false)]
		[InlineData(0x00007FFF, 0x000000FF, false, false)]
		[InlineData(0x00007FFF, 0x00007FFF, false, false)]
		[InlineData(0x00007FFF, 0x0000FFFF, false, false)]
		[InlineData(0x00007FFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x00007FFF, 0xFFFFFFFF, false, false)]
		[InlineData(0x0000FFFF, 0x80000000, true, false)]
		[InlineData(0x0000FFFF, 0x00008000, false, false)]
		[InlineData(0x0000FFFF, 0x00000080, false, false)]
		[InlineData(0x0000FFFF, 0x00000000, true, false)]
		[InlineData(0x0000FFFF, 0x0000007F, false, false)]
		[InlineData(0x0000FFFF, 0x000000FF, false, false)]
		[InlineData(0x0000FFFF, 0x00007FFF, false, false)]
		[InlineData(0x0000FFFF, 0x0000FFFF, false, false)]
		[InlineData(0x0000FFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x0000FFFF, 0xFFFFFFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x80000000, true, false)]
		[InlineData(0x7FFFFFFF, 0x00008000, false, false)]
		[InlineData(0x7FFFFFFF, 0x00000080, false, false)]
		[InlineData(0x7FFFFFFF, 0x00000000, true, false)]
		[InlineData(0x7FFFFFFF, 0x0000007F, false, false)]
		[InlineData(0x7FFFFFFF, 0x000000FF, false, false)]
		[InlineData(0x7FFFFFFF, 0x00007FFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x0000FFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0xFFFFFFFF, false, false)]
		[InlineData(0xFFFFFFFF, 0x80000000, false, true)]
		[InlineData(0xFFFFFFFF, 0x00008000, false, false)]
		[InlineData(0xFFFFFFFF, 0x00000080, false, false)]
		[InlineData(0xFFFFFFFF, 0x00000000, true, false)]
		[InlineData(0xFFFFFFFF, 0x0000007F, false, false)]
		[InlineData(0xFFFFFFFF, 0x000000FF, false, false)]
		[InlineData(0xFFFFFFFF, 0x00007FFF, false, false)]
		[InlineData(0xFFFFFFFF, 0x0000FFFF, false, false)]
		[InlineData(0xFFFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0xFFFFFFFF, 0xFFFFFFFF, false, true)]
		#endregion
		public void Test32Test(uint left, uint right, bool zero, bool sign)
		{
			_cpu.Test(new Register32(left), new Register32(right));
			_cpu.Eflags.Carry.Should().BeFalse();
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().BeFalse();
		}

		[Theory]
		#region
		[InlineData(0x80000000, 0x80000000, 0x00000000, true, false)]
		[InlineData(0x80000000, 0x00008000, 0x80008000, false, true)]
		[InlineData(0x80000000, 0x00000080, 0x80000080, false, true)]
		[InlineData(0x80000000, 0x00000000, 0x80000000, false, true)]
		[InlineData(0x80000000, 0x0000007F, 0x8000007F, false, true)]
		[InlineData(0x80000000, 0x000000FF, 0x800000FF, false, true)]
		[InlineData(0x80000000, 0x00007FFF, 0x80007FFF, false, true)]
		[InlineData(0x80000000, 0x0000FFFF, 0x8000FFFF, false, true)]
		[InlineData(0x80000000, 0x7FFFFFFF, 0xFFFFFFFF, false, true)]
		[InlineData(0x80000000, 0xFFFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x00008000, 0x80000000, 0x80008000, false, true)]
		[InlineData(0x00008000, 0x00008000, 0x00000000, true, false)]
		[InlineData(0x00008000, 0x00000080, 0x00008080, false, false)]
		[InlineData(0x00008000, 0x00000000, 0x00008000, false, false)]
		[InlineData(0x00008000, 0x0000007F, 0x0000807F, false, false)]
		[InlineData(0x00008000, 0x000000FF, 0x000080FF, false, false)]
		[InlineData(0x00008000, 0x00007FFF, 0x0000FFFF, false, false)]
		[InlineData(0x00008000, 0x0000FFFF, 0x00007FFF, false, false)]
		[InlineData(0x00008000, 0x7FFFFFFF, 0x7FFF7FFF, false, false)]
		[InlineData(0x00008000, 0xFFFFFFFF, 0xFFFF7FFF, false, true)]
		[InlineData(0x00000080, 0x80000000, 0x80000080, false, true)]
		[InlineData(0x00000080, 0x00008000, 0x00008080, false, false)]
		[InlineData(0x00000080, 0x00000080, 0x00000000, true, false)]
		[InlineData(0x00000080, 0x00000000, 0x00000080, false, false)]
		[InlineData(0x00000080, 0x0000007F, 0x000000FF, false, false)]
		[InlineData(0x00000080, 0x000000FF, 0x0000007F, false, false)]
		[InlineData(0x00000080, 0x00007FFF, 0x00007F7F, false, false)]
		[InlineData(0x00000080, 0x0000FFFF, 0x0000FF7F, false, false)]
		[InlineData(0x00000080, 0x7FFFFFFF, 0x7FFFFF7F, false, false)]
		[InlineData(0x00000080, 0xFFFFFFFF, 0xFFFFFF7F, false, true)]
		[InlineData(0x00000000, 0x80000000, 0x80000000, false, true)]
		[InlineData(0x00000000, 0x00008000, 0x00008000, false, false)]
		[InlineData(0x00000000, 0x00000080, 0x00000080, false, false)]
		[InlineData(0x00000000, 0x00000000, 0x00000000, true, false)]
		[InlineData(0x00000000, 0x0000007F, 0x0000007F, false, false)]
		[InlineData(0x00000000, 0x000000FF, 0x000000FF, false, false)]
		[InlineData(0x00000000, 0x00007FFF, 0x00007FFF, false, false)]
		[InlineData(0x00000000, 0x0000FFFF, 0x0000FFFF, false, false)]
		[InlineData(0x00000000, 0x7FFFFFFF, 0x7FFFFFFF, false, false)]
		[InlineData(0x00000000, 0xFFFFFFFF, 0xFFFFFFFF, false, true)]
		[InlineData(0x0000007F, 0x80000000, 0x8000007F, false, true)]
		[InlineData(0x0000007F, 0x00008000, 0x0000807F, false, false)]
		[InlineData(0x0000007F, 0x00000080, 0x000000FF, false, false)]
		[InlineData(0x0000007F, 0x00000000, 0x0000007F, false, false)]
		[InlineData(0x0000007F, 0x0000007F, 0x00000000, true, false)]
		[InlineData(0x0000007F, 0x000000FF, 0x00000080, false, false)]
		[InlineData(0x0000007F, 0x00007FFF, 0x00007F80, false, false)]
		[InlineData(0x0000007F, 0x0000FFFF, 0x0000FF80, false, false)]
		[InlineData(0x0000007F, 0x7FFFFFFF, 0x7FFFFF80, false, false)]
		[InlineData(0x0000007F, 0xFFFFFFFF, 0xFFFFFF80, false, true)]
		[InlineData(0x000000FF, 0x80000000, 0x800000FF, false, true)]
		[InlineData(0x000000FF, 0x00008000, 0x000080FF, false, false)]
		[InlineData(0x000000FF, 0x00000080, 0x0000007F, false, false)]
		[InlineData(0x000000FF, 0x00000000, 0x000000FF, false, false)]
		[InlineData(0x000000FF, 0x0000007F, 0x00000080, false, false)]
		[InlineData(0x000000FF, 0x000000FF, 0x00000000, true, false)]
		[InlineData(0x000000FF, 0x00007FFF, 0x00007F00, false, false)]
		[InlineData(0x000000FF, 0x0000FFFF, 0x0000FF00, false, false)]
		[InlineData(0x000000FF, 0x7FFFFFFF, 0x7FFFFF00, false, false)]
		[InlineData(0x000000FF, 0xFFFFFFFF, 0xFFFFFF00, false, true)]
		[InlineData(0x00007FFF, 0x80000000, 0x80007FFF, false, true)]
		[InlineData(0x00007FFF, 0x00008000, 0x0000FFFF, false, false)]
		[InlineData(0x00007FFF, 0x00000080, 0x00007F7F, false, false)]
		[InlineData(0x00007FFF, 0x00000000, 0x00007FFF, false, false)]
		[InlineData(0x00007FFF, 0x0000007F, 0x00007F80, false, false)]
		[InlineData(0x00007FFF, 0x000000FF, 0x00007F00, false, false)]
		[InlineData(0x00007FFF, 0x00007FFF, 0x00000000, true, false)]
		[InlineData(0x00007FFF, 0x0000FFFF, 0x00008000, false, false)]
		[InlineData(0x00007FFF, 0x7FFFFFFF, 0x7FFF8000, false, false)]
		[InlineData(0x00007FFF, 0xFFFFFFFF, 0xFFFF8000, false, true)]
		[InlineData(0x0000FFFF, 0x80000000, 0x8000FFFF, false, true)]
		[InlineData(0x0000FFFF, 0x00008000, 0x00007FFF, false, false)]
		[InlineData(0x0000FFFF, 0x00000080, 0x0000FF7F, false, false)]
		[InlineData(0x0000FFFF, 0x00000000, 0x0000FFFF, false, false)]
		[InlineData(0x0000FFFF, 0x0000007F, 0x0000FF80, false, false)]
		[InlineData(0x0000FFFF, 0x000000FF, 0x0000FF00, false, false)]
		[InlineData(0x0000FFFF, 0x00007FFF, 0x00008000, false, false)]
		[InlineData(0x0000FFFF, 0x0000FFFF, 0x00000000, true, false)]
		[InlineData(0x0000FFFF, 0x7FFFFFFF, 0x7FFF0000, false, false)]
		[InlineData(0x0000FFFF, 0xFFFFFFFF, 0xFFFF0000, false, true)]
		[InlineData(0x7FFFFFFF, 0x80000000, 0xFFFFFFFF, false, true)]
		[InlineData(0x7FFFFFFF, 0x00008000, 0x7FFF7FFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x00000080, 0x7FFFFF7F, false, false)]
		[InlineData(0x7FFFFFFF, 0x00000000, 0x7FFFFFFF, false, false)]
		[InlineData(0x7FFFFFFF, 0x0000007F, 0x7FFFFF80, false, false)]
		[InlineData(0x7FFFFFFF, 0x000000FF, 0x7FFFFF00, false, false)]
		[InlineData(0x7FFFFFFF, 0x00007FFF, 0x7FFF8000, false, false)]
		[InlineData(0x7FFFFFFF, 0x0000FFFF, 0x7FFF0000, false, false)]
		[InlineData(0x7FFFFFFF, 0x7FFFFFFF, 0x00000000, true, false)]
		[InlineData(0x7FFFFFFF, 0xFFFFFFFF, 0x80000000, false, true)]
		[InlineData(0xFFFFFFFF, 0x80000000, 0x7FFFFFFF, false, false)]
		[InlineData(0xFFFFFFFF, 0x00008000, 0xFFFF7FFF, false, true)]
		[InlineData(0xFFFFFFFF, 0x00000080, 0xFFFFFF7F, false, true)]
		[InlineData(0xFFFFFFFF, 0x00000000, 0xFFFFFFFF, false, true)]
		[InlineData(0xFFFFFFFF, 0x0000007F, 0xFFFFFF80, false, true)]
		[InlineData(0xFFFFFFFF, 0x000000FF, 0xFFFFFF00, false, true)]
		[InlineData(0xFFFFFFFF, 0x00007FFF, 0xFFFF8000, false, true)]
		[InlineData(0xFFFFFFFF, 0x0000FFFF, 0xFFFF0000, false, true)]
		[InlineData(0xFFFFFFFF, 0x7FFFFFFF, 0x80000000, false, true)]
		[InlineData(0xFFFFFFFF, 0xFFFFFFFF, 0x00000000, true, false)]
		#endregion
		public void Xor32Test(uint left, uint right, uint result, bool zero, bool sign)
		{
			_cpu.Xor(new Register32(left), new Register32(right)).Should().Be(new Register32(result));
			_cpu.Eflags.Carry.Should().BeFalse();
			_cpu.Eflags.Zero.Should().Be(zero);
			_cpu.Eflags.Sign.Should().Be(sign);
			_cpu.Eflags.Overflow.Should().BeFalse();
		}

		[Fact]
		public void ToStringTest()
		{
			_cpu.Eax = new Register32(0x31415926);
			_cpu.Ecx = new Register32(0x53589793);
			_cpu.Edx = new Register32(0x23846264);
			_cpu.Ebx = new Register32(0x33832795);
			_cpu.Esp = new Register32(0x02884197);
			_cpu.Ebp = new Register32(0x16939937);
			_cpu.Esi = new Register32(0x51058209);
			_cpu.Edi = new Register32(0x74944592);

			var builder = new StringBuilder();
			builder.AppendLine($"{nameof(_cpu.Eax)}: 31415926");
			builder.AppendLine($"{nameof(_cpu.Ecx)}: 53589793");
			builder.AppendLine($"{nameof(_cpu.Edx)}: 23846264");
			builder.AppendLine($"{nameof(_cpu.Ebx)}: 33832795");
			builder.AppendLine($"{nameof(_cpu.Esp)}: 02884197");
			builder.AppendLine($"{nameof(_cpu.Ebp)}: 16939937");
			builder.AppendLine($"{nameof(_cpu.Esi)}: 51058209");
			builder.AppendLine($"{nameof(_cpu.Edi)}: 74944592");
			_cpu.ToString().Should().Be(builder.ToString());
		}
	}
}
