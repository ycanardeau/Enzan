using System;
using System.Runtime.CompilerServices;

namespace Aigamo.Enzan
{
	internal static class BitHelper
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static byte High(short value) => (byte)(value >> 8);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static byte Low(short value) => (byte)(value & 0xff);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static short High(int value) => (short)(value >> 16);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static short Low(int value) => (short)(value & 0xffff);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static int High(long value) => (int)(value >> 32);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static int Low(long value) => (int)(value & 0xffffffff);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static int High(double value) => High(BitConverter.DoubleToInt64Bits(value));

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static int Low(double value) => Low(BitConverter.DoubleToInt64Bits(value));

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static short MakeShort(byte low, byte high) => (short)((byte)(low & 0xff) | ((byte)(high & 0xff) << 8));

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static int MakeInt(short low, short high) => (low & 0xffff) | ((high & 0xffff) << 16);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static long MakeLong(int low, int high) => (low & 0xffffffff) | ((high & 0xffffffff) << 32);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static double MakeDouble(int low, int high) => BitConverter.Int64BitsToDouble(MakeLong(low, high));
	}
}
