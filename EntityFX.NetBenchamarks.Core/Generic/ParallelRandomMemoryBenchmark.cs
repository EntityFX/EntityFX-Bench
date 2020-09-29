using System;
using System.Linq;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class ParallelRandomMemoryBenchmark : RandomMemoryBenchmarkBase<BenchResult[]>, IBenchamrk
    {
        public override BenchResult[] BenchImplementation()
        {
            UseConsole = false;
            return BenchInParallel(() => 0, a =>
            {
                return BenchRandomMemory();
            }, (a, r) => { r.Result = a.Average; r.Output = a.Output; });
        }

        public override BenchResult PopulateResult(BenchResult benchResult, BenchResult[] dhrystoneResult)
        {
            var result = BuildParallelResult(benchResult, dhrystoneResult);
            var sum = dhrystoneResult.Sum(r => Convert.ToDecimal(r.Result));
            result.Result = sum;
            result.Points = Convert.ToDecimal(result.Result * Ratio);
            result.Units = "MB/s";
            result.Output = string.Concat(dhrystoneResult.Select(s => s.Output));
            return result;
        }
    }
}
