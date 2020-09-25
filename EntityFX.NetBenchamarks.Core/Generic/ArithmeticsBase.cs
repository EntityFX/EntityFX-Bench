using System.Runtime.CompilerServices;

namespace EntityFX.NetBenchmark.Core.Generic
{

    public abstract class ArithmeticsBase<TResult> : BenchmarkBase<TResult>
    {
        protected double R;

        public ArithmeticsBase()
        {
            Iterrations = 300000000;
            Ratio = 0.04;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static double DoArithmetics(long i)
        {
            return (i % 10) * (i % 100) * (i % 100) * (i % 100) * 1.11 + (i % 100) * (i % 1000) * (i % 1000) * 2.22 - i * (i % 10000) * 3.33 + i * 5.33;
        }
 
    }
}
