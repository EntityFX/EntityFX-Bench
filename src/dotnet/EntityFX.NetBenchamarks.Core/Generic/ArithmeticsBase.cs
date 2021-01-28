using System;
using System.Runtime.CompilerServices;

namespace EntityFX.NetBenchmark.Core.Generic
{

    public abstract class ArithmeticsBase<TResult> : BenchmarkBase<TResult>
    {
        protected double R;

        public ArithmeticsBase()
        {
            Iterrations = 300000000;
            Ratio = 0.03;
        }

#if NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected static float DoArithmetics(int i)
        {
            return (i / 10) * (i / 100) * (i / 100) * (i / 100) * 1.11f + (i / 100) * (i / 1000) * (i / 1000) * 2.22f - i * (i / 10000) * 3.33f + i * 5.33f;
        }

#if NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected static double DoArithmetics(long i)
        {
            return (i / 10) * (i / 100) * (i / 100) * (i / 100) * 1.11 + (i / 100) * (i / 1000) * (i / 1000) * 2.22 - i * (i / 10000) * 3.33 + i * 5.33;
        }

    }
}
