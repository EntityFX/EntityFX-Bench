using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class ParallelCallBenchmark : CallBenchmarkBase<BenchResult[]>, IBenchamrk
    {
        public ParallelCallBenchmark()
        {
            IsParallel = true;
        }

        public override BenchResult[] BenchImplementation()
        {
            return BenchInParallel(() => 0, a =>
            {
                return DoCallBench();
            }, (a, r) => { });
        }

        protected override BenchResult[] BenchInParallel<TBench, TBenchResult>(
            Func<TBench> buildFunc, Func<TBench, TBenchResult> benchFunc,
            Action<TBenchResult, BenchResult> setBenchResultFunc)
        {
            var benchs = Enumerable.Range(0, Environment.ProcessorCount)
                .Select(i => buildFunc()).ToArray();

            var results = new BenchResult[Environment.ProcessorCount];

            var sw = new Stopwatch();
            sw.Start();

            var tasks = Enumerable.Range(0, Environment.ProcessorCount)
                .Select(i => Task.Run(() => {
                    var swi = new Stopwatch();
                    sw.Start();
                    var callTime = DoCallBench();
                    results[i] = BuildResult(sw);
                    results[i].Elapsed = TimeSpan.FromMilliseconds(callTime);
                })).ToArray();

            Task.WaitAll(tasks);
            return results;
        }

    }
}
