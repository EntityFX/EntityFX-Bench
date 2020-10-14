using System.Diagnostics;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class LoopsBenchmark : BenchmarkBase<int>, IBenchamrk
    {
        private int r;

        public LoopsBenchmark()
        {
            Iterrations = 2000000000;
        }

        public override int BenchImplementation()
        {
            int i = 0;
            for (i = 0; i < Iterrations; ++i)
            {
                r = i;
            }
            i = 0;
            while (i < Iterrations)
            {
                ++i;
            }
            return i;
        }
    }
}
