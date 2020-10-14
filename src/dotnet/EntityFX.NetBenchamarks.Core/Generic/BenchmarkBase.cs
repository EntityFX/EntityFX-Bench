using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public abstract class BenchmarkBase<TResult> : IBenchamrk
    {
        protected int Iterrations;

        public static double DebugAspectRatio = 0.1;

        public double Ratio = 1.0;

        public string Name => GetType().Name;

        public virtual BenchResult Bench()
        {
            BeforeBench();
            var sw = new Stopwatch();
            sw.Start();
            var res = BenchImplementation();
            var result = PopulateResult(BuildResult(sw), res);
            if (result.Output != null)
            {
                File.WriteAllText($"{GetType().Name}.log", result.Output);
            }
            AfterBench(result);
            return result;
        }

        public abstract TResult BenchImplementation();

        protected virtual void BeforeBench()  
        {

        }

        protected virtual void AfterBench(BenchResult result)  
        {

        }

        public virtual void Warmup(double aspect = 0.05)
        {
#if DEBUG
            Iterrations = Convert.ToInt32(Iterrations * DebugAspectRatio);
#endif
            var tmp = Iterrations;
            Iterrations = Convert.ToInt32( Iterrations * 0.05);
            Bench();
            Iterrations = tmp;
        }


        protected BenchResult[] BenchInParallel<TBench, TBenchResult>(
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
                    var result = benchFunc(benchs[i]);
                    results[i] = BuildResult(sw);
                    setBenchResultFunc(result, results[i]);

                })).ToArray();

            Task.WaitAll(tasks);
            return results;
        }

        public virtual BenchResult PopulateResult(BenchResult benchResult, TResult dhrystoneResult)
        {
            if (dhrystoneResult is BenchResult[] results)
            {
                return BuildParallelResult(benchResult, results);
            }
            return benchResult;
        }

        protected BenchResult BuildResult(Stopwatch sw)
        {
            return new BenchResult() { 
                BenchmarkName = GetType().Name, Elapsed = sw.Elapsed,
                Points = Convert.ToDecimal(Iterrations / sw.Elapsed.TotalMilliseconds * Ratio) };
        }

        protected BenchResult BuildParallelResult(Stopwatch sw, BenchResult[] results)
        {
            var result = BuildResult(sw);
            result.Points = results.Sum(r => r.Points);
            return result;
        }

        protected BenchResult BuildParallelResult(BenchResult rootResult, BenchResult[] results)
        {
            rootResult.Points = results.Sum(r => r.Points);
            return rootResult;
        }
    }
}

