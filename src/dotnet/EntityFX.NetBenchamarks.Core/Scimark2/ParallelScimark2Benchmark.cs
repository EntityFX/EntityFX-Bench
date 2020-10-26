using EntityFX.NetBenchmark.Core.Generic;
using System;
using System.Linq;

namespace EntityFX.NetBenchmark.Core.Scimark2
{
    public class ParallelScimark2Benchmark : BenchmarkBase<BenchResult[]>, IBenchamrk
    {
        public ParallelScimark2Benchmark()
        {
            Ratio = 10;
            IsParallel = true;
        }

        public override BenchResult[] BenchImplementation()
        {
            return BenchInParallel(() => new Scimark2(false), a =>
            a.Bench(), (a, r) => {
                r.Points = a.CompositeScore * Ratio;
                r.Result = a.CompositeScore;
                r.Output = a.Output;
            });
        }

        public override BenchResult PopulateResult(BenchResult benchResult, BenchResult[] scimark2Result)
        {
            var result = BuildParallelResult(benchResult, scimark2Result);
            result.Result = scimark2Result.Sum(r => r.Result);
            result.Units = "CompositeScore";
            result.Output = string.Concat(scimark2Result.Select(s => s.Output));
            return result;
        }

        public override void Warmup(double aspect = 0.05)
        {

        }
    }
}