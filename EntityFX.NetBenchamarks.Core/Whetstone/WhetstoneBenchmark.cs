using EntityFX.NetBenchamarks.Core.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EntityFX.NetBenchamarks.Core.Whetstone
{
    public class WhetstoneBenchmark : BenchmarkBase, IBenchamrk
    {
        private readonly Whetstone whetstone;

        public WhetstoneBenchmark()
        {
            whetstone = new Whetstone();
        }

        public override BenchResult Bench()
        {
            var sw = new Stopwatch();
            sw.Start();
            var whetstoneResult = whetstone.Bench();
            var result = BuildResult(sw);
            result.Output = whetstoneResult.Output;
            result.Result = whetstoneResult.MWIPS;
            return result;
        }

        public override void Warmup(double aspect = 0.05)
        {

        }
    }
}
