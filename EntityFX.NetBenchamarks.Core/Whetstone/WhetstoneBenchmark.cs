using EntityFX.NetBenchmark.Core.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Whetstone
{
    public class WhetstoneBenchmark : BenchmarkBase<WhetstoneResult>, IBenchamrk
    {
        private readonly WhetstoneDouble whetstone = new WhetstoneDouble();

        public WhetstoneBenchmark()
        {

        }

        public override WhetstoneResult BenchImplementation()
        {
            return whetstone.Bench();
        }

        public override BenchResult PopulateResult(BenchResult benchResult, WhetstoneResult dhrystoneResult)
        {
            benchResult.Result = dhrystoneResult.MWIPS;
            benchResult.Points = Convert.ToDecimal(dhrystoneResult.MWIPS);
            benchResult.Units = "DMIPS";
            return benchResult;
        }

        public override void Warmup(double aspect = 0.05)
        {

        }
    }
}
