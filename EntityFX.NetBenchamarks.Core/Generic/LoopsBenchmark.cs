using System.Diagnostics;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class LoopsBenchmark : BenchmarkBase<long>, IBenchamrk
    {
        public LoopsBenchmark()
        {
            Iterrations = 5000000000;
        }

        public override long BenchImplementation()
        {
            long i = 0;
            for (i = 0; i < Iterrations; ++i)
            {

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
