using EntityFX.NetBenchmark.Core.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Dhrystone
{
    public class DhrystoneBenchmark : BenchmarkBase<DhrystoneResult>, IBenchamrk
    {
        private readonly Dhrystone2 dhrystone = new Dhrystone2();

        public DhrystoneBenchmark()
        {
            Ratio = 4;
        }

        public override DhrystoneResult BenchImplementation()
        {
            return dhrystone.Bench();
        }

        public override BenchResult PopulateResult(BenchResult benchResult, DhrystoneResult dhrystoneResult)
        {
            benchResult.Result = dhrystoneResult.VaxMips;
            benchResult.Points = Convert.ToDecimal(dhrystoneResult.VaxMips * Ratio);
            benchResult.Units = "DMIPS";
            return benchResult;
        }


        public override void Warmup(double aspect = 0.05)
        {

        }
    }

}
