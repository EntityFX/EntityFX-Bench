using EntityFX.NetBenchmark.Core.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Whetstone
{
    public class ParallelWhetstoneBenchmark : BenchmarkBase<BenchResult[]>, IBenchamrk
    {
        public ParallelWhetstoneBenchmark(IWriter writer)
            :base(writer)
        {
            IsParallel = true;
        }

        public override BenchResult[] BenchImplementation()
        {
            return BenchInParallel(() => new Whetstone(false, writer), a =>
            a.Bench(true), (a, r) => { 
                r.Points = a.MWIPS;
                r.Result = r.Points;
                r.Output = a.Output;
            });
        }

        public override BenchResult PopulateResult(BenchResult benchResult, BenchResult[] dhrystoneResult)
        {
            var result = BuildParallelResult(benchResult, dhrystoneResult);
            result.Result = dhrystoneResult.Sum(r => r.Result);
            result.Units = "MWIPS";
            result.Output = string.Concat(dhrystoneResult.Select(s => s.Output));
            return result;
        }


        public override void Warmup(double aspect = 0.05)
        {

        }
    }
}
