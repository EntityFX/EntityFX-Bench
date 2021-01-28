using System;
using System.Linq;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class ParallelMemoryBenchmark : MemoryBenchmarkBase<BenchResult[]>, IBenchamrk
    {
        public ParallelMemoryBenchmark(bool printToConsole, IWriter writer) : base(printToConsole, writer)
        {
            IsParallel = true;
        }

        public override BenchResult[] BenchImplementation()
        {
            return BenchInParallel(() => 0, a =>
            {
                return BenchRandomMemory();
            }, (a, r) => { r.Result = a.Average; r.Output = a.Output; });
        }

        public override BenchResult PopulateResult(BenchResult benchResult, BenchResult[] dhrystoneResult)
        {
            var result = BuildParallelResult(benchResult, dhrystoneResult);
            var sum = dhrystoneResult.Sum(r => Convert.ToDouble(r.Result));
            result.Result = sum;
            result.Points = sum * Ratio;
            result.Units = "MB/s";
            result.Output = string.Concat(dhrystoneResult.Select(s => s.Output));
            return result;
        }
    }
}
