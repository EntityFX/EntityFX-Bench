using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System;
namespace EntityFX.NetBenchmark.Core.Generic
{

    public class RandomMemoryBenchmark: RandomMemoryBenchmarkBase<RandomMemoryBenchmarkResult>
    {
        public override RandomMemoryBenchmarkResult BenchImplementation() 
        {
            return BenchRandomMemory();
        }

        public override BenchResult PopulateResult(BenchResult benchResult, RandomMemoryBenchmarkResult result)
        {
            benchResult.Points = Convert.ToDecimal(result.Average * Ratio);
            benchResult.Result = Convert.ToDecimal(result.Average);
            benchResult.Units = "MB/s";
            benchResult.Output = result.Output;
            return benchResult;
        }

    }
}
