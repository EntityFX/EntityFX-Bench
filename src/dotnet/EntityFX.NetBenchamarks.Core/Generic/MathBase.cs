using System;
using System.Runtime.CompilerServices;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public abstract class MathBase<TResult> : BenchmarkBase<TResult>
    {
        public MathBase(IWriter writer)
            :base(writer)
        {
            Iterrations = 200000000;
            Ratio = 0.5;
        }
#if NETSTANDARD2_0 || NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected static double DoMath(int i, double li)
        {
            double rev = 1.0 / (i + 1.0);
            return Math.Abs(i) * Math.Acos(rev) * Math.Asin(rev) * Math.Atan(rev) +
                Math.Floor(li) + Math.Exp(rev) * Math.Cos(i) * Math.Sin(i) * Math.PI + Math.Sqrt(i);
        }
    }
}
