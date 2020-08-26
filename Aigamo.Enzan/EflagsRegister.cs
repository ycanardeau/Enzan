using System.Runtime.CompilerServices;

namespace Aigamo.Enzan
{
	public sealed class EflagsRegister
	{
		public Eflags Eflags { get; set; }

		public bool Carry
		{
			get => Eflags.HasFlag(Eflags.Carry);
			set => Eflags = value ? (Eflags | Eflags.Carry) : (Eflags & ~Eflags.Carry);
		}

		public bool Zero
		{
			get => Eflags.HasFlag(Eflags.Zero);
			set => Eflags = value ? (Eflags | Eflags.Zero) : (Eflags & ~Eflags.Zero);
		}

		public bool Sign
		{
			get => Eflags.HasFlag(Eflags.Sign);
			set => Eflags = value ? (Eflags | Eflags.Sign) : (Eflags & ~Eflags.Sign);
		}

		public bool Overflow
		{
			get => Eflags.HasFlag(Eflags.Overflow);
			set => Eflags = value ? (Eflags | Eflags.Overflow) : (Eflags & ~Eflags.Overflow);
		}

		/// <summary>
		/// Jump if below.
		/// </summary>
		public bool Jb => Carry;

		/// <summary>
		/// Jump if equal.
		/// </summary>
		public bool Je => Zero;

		/// <summary>
		/// Jump if not equal.
		/// </summary>
		public bool Jne => !Zero;

		/// <summary>
		/// Jump if above.
		/// </summary>
		public bool Ja => !Carry && !Zero;

		/// <summary>
		/// Jump if not sign.
		/// </summary>
		public bool Jns => !Sign;

		/// <summary>
		/// Jump if less.
		/// </summary>
		public bool Jl => Sign != Overflow;

		/// <summary>
		/// Jump if greater or equal.
		/// </summary>
		public bool Jge => Sign == Overflow;

		/// <summary>
		/// Jump if less or equal.
		/// </summary>
		public bool Jle => Zero || Sign != Overflow;

		/// <summary>
		/// Jump if greater.
		/// </summary>
		public bool Jg => !Zero && Sign == Overflow;

		/// <summary>
		/// Jump if above or equal.
		/// </summary>
		public bool Jae => !Carry;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void Update(bool carry, bool zero, bool sign, bool overflow)
		{
			Carry = carry;
			Zero = zero;
			Sign = sign;
			Overflow = overflow;
		}
	}
}
