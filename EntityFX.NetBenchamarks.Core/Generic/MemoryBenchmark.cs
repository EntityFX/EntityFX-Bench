using System;
namespace EntityFX.NetBenchmark.Core.Generic
{
    public class MemoryBenchmark: MemoryBenchmarkBase<MemoryBenchmarkResult>
    {
        public override MemoryBenchmarkResult BenchImplementation() 
        {
            return BenchRandomMemory();
        }

        public override BenchResult PopulateResult(BenchResult benchResult, MemoryBenchmarkResult result)
        {
            benchResult.Points = Convert.ToDecimal(result.Average * Ratio);
            benchResult.Result = Convert.ToDecimal(result.Average);
            benchResult.Units = "MB/s";
            benchResult.Output = result.Output;
            return benchResult;
        }

    }
}
