using System;
using System.Diagnostics;

namespace EntityFX.NetBenchamarks.Core.Generic
{
    public abstract class BenchmarkBase : IBenchamrk
    {
        protected long Iterrations;

        public static double DebugAspectRatio = 0.1;

        public string Name => GetType().Name;

        public abstract BenchResult Bench();
        public virtual void Warmup(double aspect = 0.05)
        {
#if DEBUG
            Iterrations = Convert.ToInt64(Iterrations * DebugAspectRatio);
#endif
            var tmp = Iterrations;
            Iterrations = Convert.ToInt64( Iterrations * 0.05);
            Bench();
            Iterrations = tmp;
        }

        protected BenchResult BuildResult(Stopwatch sw)
        {
            return new BenchResult() { BenchmarkName = GetType().Name, Elapsed = sw.Elapsed };
        }
    }
}
