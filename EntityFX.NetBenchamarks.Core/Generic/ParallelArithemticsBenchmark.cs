using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class ParallelArithemticsBenchmark : ArithmeticsBase<BenchResult[]>, IBenchamrk
    {
        public override BenchResult[] BenchImplementation()
        {
            return BenchInParallel(() => 0, a =>
            {
                double R2 = 0;
                for (long i = 0; i < Iterrations; i++)
                {
                    R2 += DoArithmetics(i);
                }
                return R2;
            }, (a, r) => { });
        }
    }
}
