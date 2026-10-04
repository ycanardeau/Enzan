using BenchmarkDotNet.Attributes;

namespace Aigamo.Enzan.Benchmarks;

/// <summary>
/// Each benchmark mimics recompiled x86 code driving <see cref="Cpu"/>, running <see cref="Iterations"/> loop iterations.
/// </summary>
[MemoryDiagnoser]
public class CpuBenchmarks
{
	private const int Iterations = 1000;
	private const int OperationsPerInvoke = Iterations;

	private static readonly Register32 Offset = new(0x400000);
	private static readonly Register32 StackTop = Offset + new Register32(0x10000);

	private Cpu _cpu = null!;

	[GlobalSetup]
	public void Setup()
	{
		_cpu = new Cpu(new byte[0x10000], Offset);
		_cpu.Esp = StackTop;
		_cpu.Eax = new Register32(0x31415926);
		_cpu.Ebx = new Register32(0x27182818);
		_cpu.Ecx = Register32.Empty;
		_cpu.Edx = Register32.Empty;
	}

	// mov ecx, N
	// loop: add eax, ebx / xor ebx, eax / shl ebx, 1 / dec ecx / jne loop
	[Benchmark(OperationsPerInvoke = OperationsPerInvoke)]
	public Register32 IntegerAlu()
	{
		var cpu = _cpu;
		cpu.Ecx = new Register32(Iterations);
		do
		{
			cpu.Eax = cpu.Add(cpu.Eax, cpu.Ebx);
			cpu.Ebx = cpu.Xor(cpu.Ebx, cpu.Eax);
			cpu.Ebx = cpu.Shl(cpu.Ebx, Register8.One);
			cpu.Ecx = cpu.Dec(cpu.Ecx);
		}
		while (cpu.Jne);
		return cpu.Eax;
	}

	// loop: cmp eax, ebx / jl skip / sub eax, ebx / skip: inc edx / cmp edx, N / jl loop
	[Benchmark(OperationsPerInvoke = OperationsPerInvoke)]
	public Register32 CompareAndBranch()
	{
		var cpu = _cpu;
		cpu.Edx = Register32.Empty;
		do
		{
			cpu.Cmp(cpu.Eax, cpu.Ebx);
			if (!cpu.Jl)
				cpu.Eax = cpu.Sub(cpu.Eax, cpu.Ebx);
			cpu.Edx = cpu.Inc(cpu.Edx);
			cpu.Cmp(cpu.Edx, new Register32(Iterations));
		}
		while (cpu.Jl);
		return cpu.Eax;
	}

	// Function prologue/epilogue: push ebp / push esi / push edi / ... / pop edi / pop esi / pop ebp
	[Benchmark(OperationsPerInvoke = OperationsPerInvoke)]
	public Register32 PushPop()
	{
		var cpu = _cpu;
		for (var i = 0; i < Iterations; i++)
		{
			cpu.Push(cpu.Ebp);
			cpu.Push(cpu.Esi);
			cpu.Push(cpu.Edi);
			cpu.Edi = cpu.Pop32();
			cpu.Esi = cpu.Pop32();
			cpu.Ebp = cpu.Pop32();
		}
		return cpu.Esp;
	}

	// loop: mov al, ah / and al, bl / mov ah, cl / test al, al / inc edx / ...
	[Benchmark(OperationsPerInvoke = OperationsPerInvoke)]
	public Register8 SubRegisters()
	{
		var cpu = _cpu;
		for (var i = 0; i < Iterations; i++)
		{
			cpu.Al = cpu.Mov(cpu.Ah);
			cpu.Al = cpu.And(cpu.Al, cpu.Bl);
			cpu.Ah = cpu.Mov(cpu.Bh);
			cpu.Bx = cpu.Mov(cpu.Ax);
			cpu.Test(cpu.Al, cpu.Al);
		}
		return cpu.Al;
	}

	// loop: fild [eax] / fmul st, st(1) / fadd / fcom / fnstsw ax / test ah, 41h
	[Benchmark(OperationsPerInvoke = OperationsPerInvoke)]
	public Register64 Fpu()
	{
		var cpu = _cpu;
		var stack = cpu.Fpu.Stack;
		cpu.Fld(Register64.FromDouble(1.0));
		for (var i = 0; i < Iterations; i++)
		{
			cpu.Fild(new Register32((uint)i));
			stack[0] = cpu.Fmul(stack[0], Register64.FromDouble(0.5));
			stack[1] = cpu.Fadd(stack[1], stack.Pop());
			cpu.Fcom(Register64.FromDouble(100.0));
			cpu.Ax = cpu.Fnstsw();
			cpu.Test(cpu.Ah, new Register8(0x41));
		}
		return stack.Pop();
	}
}
