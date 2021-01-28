using System.Runtime.CompilerServices;
using System;
namespace EntityFX.NetBenchmark.Core.Generic
{

    public class RandomMemoryBenchmark: RandomMemoryBenchmarkBase<MemoryBenchmarkResult>
    {
        public RandomMemoryBenchmark(bool printToConsole, IWriter writer)
            : base(printToConsole, writer)
        {
        }

        public override MemoryBenchmarkResult BenchImplementation() 
        {
            return BenchRandomMemory();
        }

        public override BenchResult PopulateResult(BenchResult benchResult, MemoryBenchmarkResult result)
        {
            benchResult.Points = result.Average * Ratio;
            benchResult.Result = result.Average;
            benchResult.Units = "MB/s";
            benchResult.Output = result.Output;
            return benchResult;
        }

    }
}
