using System.Runtime.CompilerServices;

namespace EntityFX.NetBenchamarks.Core.Generic
{

    public abstract class ArithmeticsBase : BenchmarkBase
    {
        protected double R;

        public ArithmeticsBase()
        {
            Iterrations = 300000000;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static double DoArithmetics(long i)
        {
            return (i % 10) * (i % 100) * (i % 100) * (i % 100) * 1.11 + (i % 100) * (i % 1000) * (i % 1000) * 2.22 - i * (i % 10000) * 3.33 + i * 5.33;
        }
    }
}
