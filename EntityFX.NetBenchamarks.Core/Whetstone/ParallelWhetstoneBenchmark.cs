using EntityFX.NetBenchmark.Core.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Whetstone
{
    public class ParallelWhetstoneBenchmark : BenchmarkBase<BenchResult[]>, IBenchamrk
    {
        public ParallelWhetstoneBenchmark()
        {
        }

        public override BenchResult[] BenchImplementation()
        {
            return BenchInParallel(() => new Whetstone(false), a =>
            a.Bench(), (a, r) => r.Points = Convert.ToDecimal(a.MWIPS));
        }

        public override BenchResult PopulateResult(BenchResult benchResult, BenchResult[] dhrystoneResult)
        {
            var result = BuildParallelResult(benchResult, dhrystoneResult);
            result.Result = dhrystoneResult.Sum(r => Convert.ToDecimal(r.Result));
            result.Units = "MWIPS";
            return result;
        }


        public override void Warmup(double aspect = 0.05)
        {

        }
    }
}
