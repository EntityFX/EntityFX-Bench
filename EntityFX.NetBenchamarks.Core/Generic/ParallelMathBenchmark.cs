using System.Diagnostics;
using System.Threading.Tasks;

namespace EntityFX.NetBenchamarks.Core.Generic
{
    public class ParallelMathBenchmark : MathBase, IBenchamrk
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
                R += DoMath(i, li);
            });
            return BuildResult(sw);
        }
    }
}
