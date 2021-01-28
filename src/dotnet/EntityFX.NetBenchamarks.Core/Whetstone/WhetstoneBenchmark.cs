using EntityFX.NetBenchmark.Core.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EntityFX.NetBenchmark.Core.Whetstone
{
    public class WhetstoneBenchmark : BenchmarkBase<WhetstoneResult>, IBenchamrk
    {
        private readonly WhetstoneDouble whetstone;

        public WhetstoneBenchmark(IWriter writer)
            :base(writer)
        {
            whetstone = new WhetstoneDouble(writer);
        }

        public override WhetstoneResult BenchImplementation()
        {
            return whetstone.Bench(true);
        }

        public override BenchResult PopulateResult(BenchResult benchResult, WhetstoneResult dhrystoneResult)
        {
            benchResult.Result = dhrystoneResult.MWIPS;
            benchResult.Points = dhrystoneResult.MWIPS * Ratio;
            benchResult.Units = "MWIPS";
            benchResult.Output = dhrystoneResult.Output;
            return benchResult;
        }

        public override void Warmup(double aspect)
        {

        }
    }
}
