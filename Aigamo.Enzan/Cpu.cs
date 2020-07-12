using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Aigamo.Enzan
{
	/// <summary>
	/// Comments from: Intel® 64 and IA-32 Architectures Software Developer Manual: Vol 2
	/// </summary>
	public sealed class Cpu
	{
		public Memory<byte> Memory { get; }
		public Register32 Offset { get; }
		public Action<Register32>? Callback { get; }

		public Register32 Eax { get; set; }
		public Register32 Ecx { get; set; }
		public Register32 Edx { get; set; }
		public Register32 Ebx { get; set; }
		public Register32 Esp { get; set; }
		public Register32 Ebp { get; set; }
		public Register32 Esi { get; set; }
		public Register32 Edi { get; set; }

		public EflagsRegister Eflags { get; } = new();
		public Fpu Fpu { get; } = new();

		public Cpu(Memory<byte> memory, Register32 offset, Action<Register32>? callback = null)
		{
			Memory = memory;
			Offset = offset;
			Callback = callback;
		}

		public Register16 Ax
		{
			get => Eax.Low;
			set => Eax = Eax.WithLow(value);
		}

		public Register16 Cx
		{
			get => Ecx.Low;
			set => Ecx = Ecx.WithLow(value);
		}

		public Register16 Dx
		{
			get => Edx.Low;
			set => Edx = Edx.WithLow(value);
		}

		public Register16 Bx
		{
			get => Ebx.Low;
			set => Ebx = Ebx.WithLow(value);
		}

		public Register16 Sp
		{
			get => Esp.Low;
			set => Esp = Esp.WithLow(value);
		}

		public Register16 Bp
		{
			get => Ebp.Low;
			set => Ebp = Ebp.WithLow(value);
		}

		public Register16 Si
		{
			get => Esi.Low;
			set => Esi = Esi.WithLow(value);
		}

		public Register16 Di
		{
			get => Edi.Low;
			set => Edi = Edi.WithLow(value);
		}

		public Register8 Al
		{
			get => Ax.Low;
			set => Ax = Ax.WithLow(value);
		}

		public Register8 Cl
		{
			get => Cx.Low;
			set => Cx = Cx.WithLow(value);
		}

		public Register8 Dl
		{
			get => Dx.Low;
			set => Dx = Dx.WithLow(value);
		}

		public Register8 Bl
		{
			get => Bx.Low;
			set => Bx = Bx.WithLow(value);
		}

		public Register8 Ah
		{
			get => Ax.High;
			set => Ax = Ax.WithHigh(value);
		}

		public Register8 Ch
		{
			get => Cx.High;
			set => Cx = Cx.WithHigh(value);
		}

		public Register8 Dh
		{
			get => Dx.High;
			set => Dx = Dx.WithHigh(value);
		}

		public Register8 Bh
		{
			get => Bx.High;
			set => Bx = Bx.WithHigh(value);
		}

		/// <summary>
		/// Add.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Add(Register32 left, Register32 right)
		{
			var tmp = new Register64((ulong)left.Value + (ulong)right.Value);
			var result = tmp.Low;
			Eflags.Update(carry: !tmp.High.IsEmpty, zero: result.IsEmpty, sign: result.Sign, overflow: left.Sign == right.Sign && left.Sign != result.Sign);
			return result;
		}

		/// <summary>
		/// Add.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Add(Register32 left, Register8 right) => Add(left, right.SignExtend().SignExtend());

		/// <summary>
		/// Logical AND.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register8 And(Register8 left, Register8 right)
		{
			var tmp = new Register16((ushort)((ushort)left.Value & (ushort)right.Value));
			var result = tmp.Low;
			Eflags.Update(carry: false, zero: result.IsEmpty, sign: result.Sign, overflow: false);
			return result;
		}

		/// <summary>
		/// Logical AND.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register16 And(Register16 left, Register16 right)
		{
			var tmp = new Register32((uint)left.Value & (uint)right.Value);
			var result = tmp.Low;
			Eflags.Update(carry: false, zero: result.IsEmpty, sign: result.Sign, overflow: false);
			return result;
		}

		/// <summary>
		/// Logical AND.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 And(Register32 left, Register32 right)
		{
			var tmp = new Register64((ulong)left.Value & (ulong)right.Value);
			var result = tmp.Low;
			Eflags.Update(carry: false, zero: result.IsEmpty, sign: result.Sign, overflow: false);
			return result;
		}

		/// <summary>
		/// Logical AND.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 And(Register32 left, Register8 right) => And(left, right.SignExtend().SignExtend());

		/// <summary>
		/// Call procedure.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Call(Register32 value) => Callback?.Invoke(value);

		/// <summary>
		/// Convert doubleword to quadword.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Cdq()
		{
			var result = Eax.SignExtend();
			Edx = result.High;
			Eax = result.Low;
		}

		/// <summary>
		/// Compare two operands.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Cmp(Register16 left, Register16 right) => Sub(left, right);

		/// <summary>
		/// Compare two operands.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Cmp(Register16 left, Register8 right) => Cmp(left, right.SignExtend());

		/// <summary>
		/// Compare two operands.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Cmp(Register32 left, Register32 right) => Sub(left, right);

		/// <summary>
		/// Compare two operands.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Cmp(Register32 left, Register8 right) => Cmp(left, right.SignExtend().SignExtend());

		/// <summary>
		/// Decrement by 1.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Dec(Register32 value)
		{
			var tmp = new Register64((ulong)value.Value - 1);
			var result = tmp.Low;
			Eflags.Update(carry: Eflags.Carry, zero: result.IsEmpty, sign: result.Sign, overflow: value.Sign && value.Sign != result.Sign);
			return result;
		}

		/// <summary>
		/// Add.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Fadd(Register64 left, Register64 right) => Register64.FromDouble(left.ToDouble() + right.ToDouble());

		/// <summary>
		/// Change sign.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Fchs(Register64 value) => Register64.FromDouble(-value.ToDouble());

		/// <summary>
		/// Compare floating point values.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Fcom(Register64 value)
		{
			if (Fpu.Stack[0].ToDouble() > value.ToDouble())
				Fpu.Status.Update(c3: false, c2: false, c0: false);
			else if (Fpu.Stack[0].ToDouble() < value.ToDouble())
				Fpu.Status.Update(c3: false, c2: false, c0: true);
			else if (Fpu.Stack[0].ToDouble() == value.ToDouble())
				Fpu.Status.Update(c3: true, c2: false, c0: false);
		}

		/// <summary>
		/// Cosine.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Fcos(Register64 value) => Register64.FromDouble(Math.Cos(value.ToDouble()));

		/// <summary>
		/// Reverse divide.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Fdiv(Register64 dividend, Register64 divisor) => Register64.FromDouble(dividend.ToDouble() / divisor.ToDouble());

		/// <summary>
		/// Reverse divide.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Fdivr(Register64 divisor, Register64 dividend) => Register64.FromDouble(dividend.ToDouble() / divisor.ToDouble());

		/// <summary>
		/// Divide.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Fidiv(Register64 dividend, Register32 divisor) => Register64.FromDouble(dividend.ToDouble() / (int)divisor.Value);

		/// <summary>
		/// Load integer.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Fild(Register32 value) => Fpu.Stack.Push(Register64.FromDouble((int)value.Value));

		/// <summary>
		/// Multiply.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Fimul(Register64 left, Register32 right) => Register64.FromDouble(left.ToDouble() * (int)right.Value);

		/// <summary>
		/// Load floating point value.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Fld(Register64 value) => Fpu.Stack.Push(value);

		/// <summary>
		/// Store x87 FPU status word.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register16 Fnstsw() => new Register16((ushort)Fpu.Status.Flags);

		/// <summary>
		/// Partial arctangent.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Fpatan(Register64 y, Register64 x) => Register64.FromDouble(Math.Atan2(y.ToDouble(), x.ToDouble()));

		/// <summary>
		/// Multiply.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Fmul(Register64 left, Register64 right) => Register64.FromDouble(left.ToDouble() * right.ToDouble());

		/// <summary>
		/// Multiply.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Fmul(Register64 left, Register32 right) => Register64.FromDouble(left.ToDouble() * right.ToSingle());

		/// <summary>
		/// Sine.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Fsin(Register64 value) => Register64.FromDouble(Math.Sin(value.ToDouble()));

		/// <summary>
		/// Store floating point value.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Fst(Register64 value) => value;

		/// <summary>
		/// Subtract.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Fsub(Register64 left, Register64 right) => Register64.FromDouble(left.ToDouble() - right.ToDouble());

		/// <summary>
		/// Reverse subtract.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register64 Fsubr(Register64 left, Register64 right) => Register64.FromDouble(right.ToDouble() - left.ToDouble());

		/// <summary>
		/// Exchange register contents.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public (Register64, Register64) Fxch(Register64 left, Register64 right) => (right, left);

		/// <summary>
		/// Signed divide.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Idiv(Register32 value)
		{
			if (value == Register32.Empty)
				throw new DivideByZeroException();

			var dividend = BitHelper.MakeLong((int)Eax.Value, (int)Edx.Value);
			var tmp = new Register64((ulong)(dividend / (int)value.Value));
			if ((long)tmp.Value > int.MaxValue || (long)tmp.Value < int.MinValue)
				throw new ArithmeticException();
			Eax = tmp.Low;
			Edx = new Register64((ulong)(dividend % (int)value.Value)).Low;
		}

		/// <summary>
		/// Signed multiply.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Imul(Register32 value)
		{
			var tmp = new Register64((ulong)((long)(int)Eax.Value * (long)(int)value.Value));
			Edx = tmp.High;
			Eax = tmp.Low;
			Eflags.Update(carry: tmp.Low.SignExtend() != tmp, zero: Eflags.Zero, sign: Eflags.Sign, overflow: tmp.Low.SignExtend() != tmp);
		}

		/// <summary>
		/// Signed multiply.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Imul(Register32 left, Register32 right)
		{
			var tmp = new Register64((ulong)((long)(int)left.Value * (long)(int)right.Value));
			Eflags.Update(carry: tmp.Low.SignExtend() != tmp, zero: Eflags.Zero, sign: Eflags.Sign, overflow: tmp.Low.SignExtend() != tmp);
			return tmp.Low;
		}

		/// <summary>
		/// Increment by 1.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Inc(Register32 value)
		{
			var tmp = new Register64((ulong)value.Value + 1);
			var result = tmp.Low;
			Eflags.Update(carry: Eflags.Carry, zero: result.IsEmpty, sign: result.Sign, overflow: !value.Sign && value.Sign != result.Sign);
			return result;
		}

		/// <summary>
		/// Jump if below.
		/// </summary>
		public bool Jb => Eflags.Jb;

		/// <summary>
		/// Jump if equal.
		/// </summary>
		public bool Je => Eflags.Je;

		/// <summary>
		/// Jump if not equal.
		/// </summary>
		public bool Jne => Eflags.Jne;

		/// <summary>
		/// Jump if above.
		/// </summary>
		public bool Ja => Eflags.Ja;

		/// <summary>
		/// Jump if not sign.
		/// </summary>
		public bool Jns => Eflags.Jns;

		/// <summary>
		/// Jump if less.
		/// </summary>
		public bool Jl => Eflags.Jl;

		/// <summary>
		/// Jump if greater or equal.
		/// </summary>
		public bool Jge => Eflags.Jge;

		/// <summary>
		/// Jump if less or equal.
		/// </summary>
		public bool Jle => Eflags.Jle;

		/// <summary>
		/// Jump if greater.
		/// </summary>
		public bool Jg => Eflags.Jg;

		/// <summary>
		/// Jump if above or equal.
		/// </summary>
		public bool Jae => Eflags.Jae;

		/// <summary>
		/// Load effective address.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Lea(Register32 value) => value;

		/// <summary>
		/// Move.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register8 Mov(Register8 value) => value;

		/// <summary>
		/// Move.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register16 Mov(Register16 value) => value;

		/// <summary>
		/// Move.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Mov(Register32 value) => value;

		/// <summary>
		/// Move with sign-extension.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Movsx(Register16 value) => value.SignExtend();

		/// <summary>
		/// Two's complement negation.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Neg(Register32 value)
		{
			var tmp = new Register64((uint)-value.Value);
			var result = tmp.Low;
			Eflags.Update(carry: !value.IsEmpty, zero: result.IsEmpty, sign: result.Sign, overflow: value.Sign && result.Sign);
			return result;
		}

		/// <summary>
		/// Logical inclusive OR.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Or(Register32 left, Register32 right)
		{
			var tmp = new Register64((ulong)left.Value | (ulong)right.Value);
			var result = tmp.Low;
			Eflags.Update(carry: false, zero: result.IsEmpty, sign: result.Sign, overflow: false);
			return result;
		}

		/// <summary>
		/// Logical inclusive OR.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Or(Register32 left, Register8 right) => Or(left, right.SignExtend().SignExtend());

		/// <summary>
		/// Pop a value from the stack.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Pop32()
		{
			var ret = new Register32(BitConverter.ToUInt32(Memory.Slice((int)(Esp - Offset).Value).Span));
			Esp += new Register32(4);
			return ret;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Push(Register8 value) => Push(new Register32(value.Value));

		/// <summary>
		/// Push doubleword onto the stack.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Push(Register32 value)
		{
			Esp -= new Register32(4);
			BitConverter.GetBytes(value.Value).AsMemory().CopyTo(Memory.Slice((int)(Esp - Offset).Value));
		}

		/// <summary>
		/// Arithmetic right shift.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Sar(Register32 value, Register8 count)
		{
			if ((count.Value & 0x1f) == 0)
				return value;

			var tmp = new Register32((uint)((int)value.Value >> (count.Value - 1)));
			var result = new Register32((uint)((int)tmp.Value >> 1));
			Eflags.Update(carry: (tmp.Value & 1) != 0, zero: result.IsEmpty, sign: result.Sign, overflow: (count.Value & 0x1f) switch
			{
				1 => false,
				_ => Eflags.Overflow
			});
			return result;
		}

		/// <summary>
		/// Integer subtraction with borrow.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Sbb(Register32 left, Register32 right)
		{
			var tmp = new Register64((ulong)left.Value - (ulong)(right.Value + (Eflags.Carry ? 1 : 0)));
			var result = tmp.Low;
			Eflags.Update(carry: !tmp.High.IsEmpty, zero: result.IsEmpty, sign: result.Sign, overflow: left.Sign != right.Sign && left.Sign != result.Sign);
			return result;
		}

		/// <summary>
		/// Set byte if equal.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register8 Sete() => Eflags.Zero ? Register8.One : Register8.Empty;

		/// <summary>
		/// Set byte if greater.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register8 Setg() => !Eflags.Zero && Eflags.Sign == Eflags.Overflow ? Register8.One : Register8.Empty;

		/// <summary>
		/// Left shift.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Shl(Register32 value, Register8 count)
		{
			if ((count.Value & 0x1f) == 0)
				return value;

			var tmp = new Register32(value.Value << (count.Value - 1));
			var result = new Register32(tmp.Value << 1);
			Eflags.Update(carry: tmp.Sign, zero: result.IsEmpty, sign: result.Sign, overflow: (count.Value & 0x1f) switch
			{
				1 => result.Sign ^ tmp.Sign,
				_ => Eflags.Overflow
			});
			return result;
		}

		/// <summary>
		/// Logical right shift.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Shr(Register32 value, Register8 count)
		{
			if ((count.Value & 0x1f) == 0)
				return value;

			var tmp = new Register32(value.Value >> (count.Value - 1));
			var result = new Register32(tmp.Value >> 1);
			Eflags.Update(carry: (tmp.Value & 1) != 0, zero: result.IsEmpty, sign: result.Sign, overflow: (count.Value & 0x1f) switch
			{
				1 => value.Sign,
				_ => Eflags.Overflow
			});
			return result;
		}

		/// <summary>
		/// Subtract.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register16 Sub(Register16 left, Register16 right)
		{
			var tmp = new Register32((uint)left.Value - (uint)right.Value);
			var result = tmp.Low;
			Eflags.Update(carry: !tmp.High.IsEmpty, zero: result.IsEmpty, sign: result.Sign, overflow: left.Sign != right.Sign && left.Sign != result.Sign);
			return result;
		}

		/// <summary>
		/// Subtract.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Sub(Register32 left, Register32 right)
		{
			var tmp = new Register64((ulong)left.Value - (ulong)right.Value);
			var result = tmp.Low;
			Eflags.Update(carry: !tmp.High.IsEmpty, zero: result.IsEmpty, sign: result.Sign, overflow: left.Sign != right.Sign && left.Sign != result.Sign);
			return result;
		}

		/// <summary>
		/// Subtract.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Sub(Register32 left, Register8 right) => Sub(left, right.SignExtend().SignExtend());

		/// <summary>
		/// Logical compare.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Test(Register8 left, Register8 right) => And(left, right);

		/// <summary>
		/// Logical compare.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Test(Register16 left, Register16 right) => And(left, right);

		/// <summary>
		/// Logical compare.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Test(Register32 left, Register32 right) => And(left, right);

		/// <summary>
		/// Logical exclusive OR.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Register32 Xor(Register32 left, Register32 right)
		{
			var tmp = new Register64((ulong)left.Value ^ (ulong)right.Value);
			var result = tmp.Low;
			Eflags.Update(carry: false, zero: result.IsEmpty, sign: result.Sign, overflow: false);
			return result;
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine($"{nameof(Eax)}: {Eax:X8}");
			builder.AppendLine($"{nameof(Ecx)}: {Ecx:X8}");
			builder.AppendLine($"{nameof(Edx)}: {Edx:X8}");
			builder.AppendLine($"{nameof(Ebx)}: {Ebx:X8}");
			builder.AppendLine($"{nameof(Esp)}: {Esp:X8}");
			builder.AppendLine($"{nameof(Ebp)}: {Ebp:X8}");
			builder.AppendLine($"{nameof(Esi)}: {Esi:X8}");
			builder.AppendLine($"{nameof(Edi)}: {Edi:X8}");
			return builder.ToString();
		}
	}
}
