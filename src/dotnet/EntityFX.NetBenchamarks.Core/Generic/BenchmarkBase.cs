using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

#if NETSTANDARD2_0 || NET45
using System.Threading.Tasks;
#endif

namespace EntityFX.NetBenchmark.Core.Generic
{
    public abstract class BenchmarkBase     {
        protected int Iterrations;

        protected IWriter writer;

        public static double IterrationsRatio = 1.0;

        public double Ratio = 1.0;

        public bool IsParallel { get; protected set; }

        protected bool UseConsole { get { return writer.UseConsole; } set { writer.UseConsole = value; } }

        public string Name { 
            get { 
                return GetType().Name;
            } 
        }
    }

    public abstract class BenchmarkBase<TResult> : BenchmarkBase, IBenchamrk
    {
        public BenchmarkBase(IWriter writer)
        {
            this.writer = writer;
        }
        
        public virtual BenchResult Bench()
        {
            BeforeBench();
            var sw = new Stopwatch();
            sw.Start();
            var res = BenchImplementation();
            var result = PopulateResult(BuildResult(sw), res);
            DoOutput(result);
            AfterBench(result);
            return result;
        }

        protected void DoOutput(BenchResult result)        
        {
            if (result.Output == null) {
                return;
            }
#if NETSTANDARD2_0 || NET45
            File.WriteAllText(string.Format("{0}.log",GetType().Name) , result.Output);
#else
            using (var sw = new StreamWriter(string.Format("{0}.log", GetType().Name)))
            {
                sw.Write(result.Output);
            }
#endif
        }

    public abstract TResult BenchImplementation();

        protected virtual void BeforeBench()  
        {

        }

        protected virtual void AfterBench(BenchResult result)  
        {

        }

        public virtual void Warmup(double aspect)
        {
            Iterrations = (int)Math.Round(Iterrations * IterrationsRatio);
            var tmp = Iterrations ;
            Iterrations = (int)Math.Round(Iterrations * aspect);
            writer.UseConsole = false;
            Bench();
            writer.UseConsole = true;
            Iterrations = tmp;
        }


#if NETSTANDARD2_0 || NET45

        protected virtual BenchResult[] BenchInParallel<TBench, TBenchResult>(
            Func<TBench> buildFunc, Func<TBench, TBenchResult> benchFunc, 
            Action<TBenchResult, BenchResult> setBenchResultFunc)
        {
            writer.UseConsole = false;
            var benchs = Enumerable.Range(0, Environment.ProcessorCount)
                .Select(i => buildFunc()).ToArray();

            var results = new BenchResult[Environment.ProcessorCount];

            var tasks = Enumerable.Range(0, Environment.ProcessorCount)
                .Select(i => Task.Run(() => {
                    var swi = new Stopwatch();
                    swi.Start();
                    var result = benchFunc(benchs[i]);
                    results[i] = BuildResult(swi);
                    setBenchResultFunc(result, results[i]);

                })).ToArray();

            Task.WaitAll(tasks);
            writer.UseConsole = true;
            return results;
        }
#endif

        public virtual BenchResult PopulateResult(BenchResult benchResult, TResult dhrystoneResult)
        {
            var results = dhrystoneResult as BenchResult[];
            if (results != null)
            {
                return BuildParallelResult(benchResult, results);
            }
            return benchResult;
        }

        protected BenchResult BuildResult(Stopwatch sw)
        {
            double tElapsed = sw.Elapsed.TotalMilliseconds;
            double elapsedSeconds = sw.Elapsed.TotalSeconds;
            return new BenchResult() { 
                BenchmarkName = GetType().Name, Elapsed = sw.Elapsed,
                Points = Iterrations / sw.Elapsed.TotalMilliseconds * Ratio ,
                Result = (double)Iterrations / elapsedSeconds,
                Units = "Iter/s",
                IsParallel = IsParallel,
                Iterrations = Iterrations,
                Ratio = Ratio
            };
        }

        protected BenchResult BuildParallelResult(Stopwatch sw, BenchResult[] results)
        {
            var result = BuildResult(sw);
            result.Points = results.Sum(r => r.Points);
            result.IsParallel = IsParallel;
            return result;
        }

        protected BenchResult BuildParallelResult(BenchResult rootResult, BenchResult[] results)
        {
            rootResult.Points = results.Sum(r => r.Points);
            rootResult.IsParallel = IsParallel;
            rootResult.Iterrations = Iterrations;
            rootResult.Ratio = Ratio;
            return rootResult;
        }
    }
}

