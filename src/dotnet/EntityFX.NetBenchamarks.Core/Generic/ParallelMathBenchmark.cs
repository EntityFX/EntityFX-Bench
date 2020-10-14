using System.Diagnostics;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class ParallelMathBenchmark : MathBase<BenchResult[]>, IBenchamrk
    {
        public override BenchResult[] BenchImplementation()
        {
            return BenchInParallel(() => 0, a =>
            {
                double R = 0;
                double li = 0;
                for (int i = 0; i < Iterrations; i++)
                {
                    R += DoMath(i, li);
                }
                return R;
            }, (a, r) => { });
        }
    }
}
