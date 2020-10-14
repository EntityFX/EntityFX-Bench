using EntityFX.NetBenchmark.Core.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Dhrystone
{
    public class Scimark2Benchmark : BenchmarkBase<Scimark2.Scimark2Result>, IBenchamrk
    {
        private readonly Scimark2.Scimark2 scimark2 = new Scimark2.Scimark2();

        public Scimark2Benchmark()
        {
            Ratio = 10;
        }

        public override Scimark2.Scimark2Result BenchImplementation()
        {
            return scimark2.Bench();
        }

        public override BenchResult PopulateResult(BenchResult benchResult, Scimark2.Scimark2Result dhrystoneResult)
        {
            benchResult.Result = dhrystoneResult.CompositeScore;
            benchResult.Points = Convert.ToDecimal(dhrystoneResult.CompositeScore * Ratio);
            benchResult.Units = "CompositeScore";
            benchResult.Output = dhrystoneResult.Output;
            return benchResult;
        }


        public override void Warmup(double aspect = 0.05)
        {

        }
    }

}
