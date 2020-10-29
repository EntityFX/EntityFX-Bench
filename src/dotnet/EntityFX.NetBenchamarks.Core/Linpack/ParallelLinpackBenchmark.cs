using EntityFX.NetBenchmark.Core.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Linpack
{
    public class ParallelLinpackBenchmark : BenchmarkBase<BenchResult[]>, IBenchamrk
    {
        public ParallelLinpackBenchmark()
        {
            IsParallel = true;
            Ratio = 10;
        }

        public override BenchResult[] BenchImplementation()
        {
            return BenchInParallel(() => new Linpack(false), a =>
            a.Bench(2000), (a, r) => {
                r.Points = a.MFLOPS;
                r.Result = r.Points;
                r.Output = a.Output;
            });
        }

        public override BenchResult PopulateResult(BenchResult benchResult, BenchResult[] dhrystoneResult)
        {
            var result = BuildParallelResult(benchResult, dhrystoneResult);
            result.Result = dhrystoneResult.Sum(r => r.Result);
            result.Units = "MFLOPS";
            result.Output = string.Concat(dhrystoneResult.Select(s => s.Output));
            return result;
        }


        public override void Warmup(double aspect = 0.05)
        {

        }
    }
}
