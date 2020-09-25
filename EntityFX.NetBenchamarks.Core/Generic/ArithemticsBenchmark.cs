using System.Diagnostics;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class ArithemticsBenchmark : ArithmeticsBase, IBenchamrk
    {
        public override BenchResult Bench()
        {
            var sw = new Stopwatch();
            sw.Start();
            R = 0;
            double li = 0;
            for (long i = 0; i < Iterrations; li = i, i++)
            {
                R += DoArithmetics(i);
            }
            return BuildResult(sw);
        }
    }
}
