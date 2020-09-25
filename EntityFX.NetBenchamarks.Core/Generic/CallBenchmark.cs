using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class CallBenchmark : BenchmarkBase, IBenchamrk
    {
        public CallBenchmark()
        {
            Iterrations = 5000000000;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static long DoCall(long i)
        {
            return i + 1;
        }

        public override BenchResult Bench()
        {
            var sw = new Stopwatch();
            sw.Start();
            long i = 0;
            long a = 0;
            for (i = 0; i < Iterrations; ++i)
            {
                a = DoCall(i);
            }
            return BuildResult(sw);
        }
    }
}
