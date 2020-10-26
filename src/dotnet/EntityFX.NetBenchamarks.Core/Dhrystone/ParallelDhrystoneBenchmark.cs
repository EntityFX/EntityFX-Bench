using EntityFX.NetBenchmark.Core.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Dhrystone
{
    public class ParallelDhrystoneBenchmark : BenchmarkBase<BenchResult[]>, IBenchamrk
    {
        public ParallelDhrystoneBenchmark()
        {
            Ratio = 4;
            IsParallel = true;
        }

        public override BenchResult[] BenchImplementation()
        {
            return BenchInParallel(() => new Dhrystone2(false), a =>
            a.Bench(), (a, r) => {
                r.Points = a.VaxMips * Ratio;
                r.Result = a.VaxMips;
                r.Output = a.Output;
            });
        }

        public override BenchResult PopulateResult(BenchResult benchResult, BenchResult[] dhrystoneResult)
        {
            var result = BuildParallelResult(benchResult, dhrystoneResult);
            result.Result = dhrystoneResult.Sum(r => r.Result);
            result.Units = "DMIPS";
            result.Output = string.Concat(dhrystoneResult.Select(s => s.Output));
            return result;
        }

        public override void Warmup(double aspect = 0.05)
        {

        }
    }

}
