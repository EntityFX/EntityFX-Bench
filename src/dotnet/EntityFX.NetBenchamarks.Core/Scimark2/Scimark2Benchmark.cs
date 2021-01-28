using EntityFX.NetBenchmark.Core.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using EntityFX.NetBenchmark.Core.Scimark2;

namespace EntityFX.NetBenchmark.Core.Scimark2
{
    public class Scimark2Benchmark : BenchmarkBase<Scimark2Result>, IBenchamrk
    {
        private readonly Scimark2 scimark2;

        public Scimark2Benchmark(IWriter writer)
            :base(writer)
        {
            scimark2 = new Scimark2(true, writer);
            Ratio = 10;
        }

        public override Scimark2Result BenchImplementation()
        {
            return scimark2.Bench(Constants.RESOLUTION_DEFAULT, false);
        }

        public override BenchResult PopulateResult(BenchResult benchResult, Scimark2Result dhrystoneResult)
        {
            benchResult.Result = dhrystoneResult.CompositeScore;
            benchResult.Points = dhrystoneResult.CompositeScore * Ratio;
            benchResult.Units = "CompositeScore";
            benchResult.Output = dhrystoneResult.Output;
            return benchResult;
        }


        public override void Warmup(double aspect)
        {

        }
    }

}
