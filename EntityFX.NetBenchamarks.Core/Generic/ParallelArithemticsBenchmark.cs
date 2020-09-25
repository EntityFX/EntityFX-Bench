using System.Diagnostics;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class ParallelArithemticsBenchmark : ArithmeticsBase, IBenchamrk
    {
        public override BenchResult Bench()
        {
            var sw = new Stopwatch();
            sw.Start();
            R = 0;
            double li = 0;
            Parallel.For(0, Iterrations, i =>
            {
                li = i;
                R += DoArithmetics(i);
            });
            return BuildResult(sw);
        }
    }
}
