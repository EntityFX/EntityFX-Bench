using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class ParallelArithemticsBenchmark : ArithmeticsBase<BenchResult[]>, IBenchamrk
    {
        public ParallelArithemticsBenchmark()
        {
            IsParallel = true;
        }

        public override BenchResult[] BenchImplementation()
        {
            return BenchInParallel(() => 0, a =>
            {
                float R2 = 0;
                for (int i = 0; i < Iterrations; i++)
                {
                    R2 += DoArithmetics(i);
                }
                return R2;
            }, (a, r) => { });
        }
    }
}
