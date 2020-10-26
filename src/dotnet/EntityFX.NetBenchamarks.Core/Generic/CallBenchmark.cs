using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EntityFX.NetBenchmark.Core.Generic
{

    public class CallBenchmark : CallBenchmarkBase<float>, IBenchamrk
    {

        public override float BenchImplementation()
        {
            return 0.0f;
        }

        public override BenchResult Bench()
        {
            BeforeBench();
            var sw = new Stopwatch();
            sw.Start();

            var callTime = DoCallBench();


            var result = PopulateResult(BuildResult(sw), callTime);
            result.Elapsed = TimeSpan.FromMilliseconds(callTime);
            DoOutput(result);
            AfterBench(result);
            return result;
        }
    }
}
