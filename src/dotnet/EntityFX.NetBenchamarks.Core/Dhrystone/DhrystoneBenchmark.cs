using EntityFX.NetBenchmark.Core.Generic;
using System;
using System.Diagnostics;
using System.Linq;

namespace EntityFX.NetBenchmark.Core.Dhrystone
{
    public class DhrystoneBenchmark : BenchmarkBase<DhrystoneResult>, IBenchamrk
    {
        private readonly Dhrystone2 dhrystone;

        public DhrystoneBenchmark(IWriter writer)
            :base(writer)
        {
            dhrystone = new Dhrystone2(true, writer);
            Ratio = 4;
        }

        public override DhrystoneResult BenchImplementation()
        {
            return dhrystone.Bench(Dhrystone2.LOOPS);
        }

        public override BenchResult PopulateResult(BenchResult benchResult, DhrystoneResult dhrystoneResult)
        {
            benchResult.Result = dhrystoneResult.VaxMips;
            benchResult.Points = dhrystoneResult.VaxMips * Ratio;
            benchResult.Units = "DMIPS";
            benchResult.Output = dhrystoneResult.Output;
            return benchResult;
        }


        public override void Warmup(double aspect)
        {

        }
    }

}
