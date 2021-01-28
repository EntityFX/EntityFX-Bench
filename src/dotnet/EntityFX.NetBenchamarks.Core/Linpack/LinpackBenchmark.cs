using EntityFX.NetBenchmark.Core.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EntityFX.NetBenchmark.Core.Linpack
{
    public class LinpackBenchmark : BenchmarkBase<LinpackResult>, IBenchamrk
    {
        private readonly Linpack linpack;

        public LinpackBenchmark(IWriter writer)
            :base(writer)
        {
            linpack = new Linpack(true);
            Ratio = 10;
        }

        public override LinpackResult BenchImplementation()
        {
            int size = 2000;
#if PocketPC
            size = 500;
#endif
            return linpack.Bench(2000);
        }

        public override BenchResult PopulateResult(BenchResult benchResult, LinpackResult linpackResult)
        {
            benchResult.Result = linpackResult.MFLOPS;
            benchResult.Points = linpackResult.MFLOPS * Ratio;
            benchResult.Units = "MFLOPS";
            benchResult.Output = linpackResult.Output;
            return benchResult;
        }

        public override void Warmup(double aspect)
        {

        }
    }
}
