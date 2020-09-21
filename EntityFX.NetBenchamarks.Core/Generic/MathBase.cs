using System;
using System.Runtime.CompilerServices;

namespace EntityFX.NetBenchamarks.Core.Generic
{
    public abstract class MathBase : BenchmarkBase
    {
        protected double R;

        public MathBase()
        {
            Iterrations = 200000000;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static double DoMath(long i, double li)
        {
            double rev = 1.0 / (i + 1.0);
            return Math.Abs(i) * Math.Acos(rev) * Math.Asin(rev) * Math.Atan(rev) +
                Math.Floor(li) + Math.Exp(rev) * Math.Cos(i) * Math.Sin(i) * Math.PI + Math.Sqrt(i);
        }
    }
}
