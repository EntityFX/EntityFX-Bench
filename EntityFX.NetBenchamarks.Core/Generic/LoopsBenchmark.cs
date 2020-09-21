using System.Diagnostics;

namespace EntityFX.NetBenchamarks.Core.Generic
{
    public class LoopsBenchmark : BenchmarkBase, IBenchamrk
    {
        public LoopsBenchmark()
        {
            Iterrations = 5000000000;
        }

        public override BenchResult Bench()
        {
            var sw = new Stopwatch();
            sw.Start();
            long i = 0;
            for (i = 0; i < Iterrations; ++i)
            {

            }
            i = 0;
            while (i < Iterrations)
            {
                ++i;
            }
            return BuildResult(sw);
        }
    }
}
