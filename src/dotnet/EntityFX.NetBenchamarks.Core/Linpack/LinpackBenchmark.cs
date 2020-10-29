using EntityFX.NetBenchmark.Core.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Linpack
{
    public class LinpackBenchmark : BenchmarkBase<LinpackResult>, IBenchamrk
    {
        private readonly Linpack Linpack = new Linpack(true);

        public LinpackBenchmark()
        {
            Ratio = 10;
        }

        public override LinpackResult BenchImplementation()
        {
            return Linpack.Bench(2000);
        }

        public override BenchResult PopulateResult(BenchResult benchResult, LinpackResult linpackResult)
        {
            benchResult.Result = linpackResult.MFLOPS;
            benchResult.Points = linpackResult.MFLOPS * Ratio;
            benchResult.Units = "MFLOPS";
            benchResult.Output = linpackResult.Output;
            return benchResult;
        }

        public override void Warmup(double aspect = 0.05)
        {

        }
    }
}
