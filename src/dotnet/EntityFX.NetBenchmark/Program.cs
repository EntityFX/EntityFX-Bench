using EntityFX.NetBenchmark.Core;
using EntityFX.NetBenchmark.Core.Dhrystone;
using EntityFX.NetBenchmark.Core.Generic;
using EntityFX.NetBenchmark.Core.Linpack;
using EntityFX.NetBenchmark.Core.Scimark2;
using EntityFX.NetBenchmark.Core.Whetstone;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark
{
    class MainClass
    {
        private static Writer writer = new Writer("Output.log");

        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            BenchmarkBase.IterrationsRatio = args.Length > 0 ? Convert.ToDouble(args[0]) : 1.0;

#if DEBUG
            BenchmarkBase.IterrationsRatio = BenchmarkBase.IterrationsRatio * 0.1;
#endif


            var useCrypto = !(args.Length > 1 && args[1] == "0");

            var benchMarks = new IBenchamrk[] 
            {
                new ArithemticsBenchmark(writer),
                new ParallelArithemticsBenchmark(writer),

                new MathBenchmark(writer),
                new ParallelMathBenchmark(writer),

                new CallBenchmark(writer),
                new ParallelCallBenchmark(writer),

                new IfElseBenchmark(writer),
                new ParallelIfElseBenchmark(writer),

                new StringManipulation(writer),
                new ParallelStringManipulation(writer),

                new MemoryBenchmark(true, writer),
                new ParallelMemoryBenchmark(true, writer),

                new RandomMemoryBenchmark(true, writer),
                new ParallelRandomMemoryBenchmark(true, writer),

                new Scimark2Benchmark(writer),
                new ParallelScimark2Benchmark(writer),

                new DhrystoneBenchmark(writer),
                new ParallelDhrystoneBenchmark(writer),

                new WhetstoneBenchmark(writer),
                new ParallelWhetstoneBenchmark(writer),

                new LinpackBenchmark(writer),
                new ParallelLinpackBenchmark(writer),
            };

            GCSettings.LatencyMode = GCLatencyMode.LowLatency;


            if (useCrypto)
            {
                benchMarks = benchMarks.Concat(new IBenchamrk[]
                { new HashBenchmark(writer), new ParallelHashBenchmark(writer) }).ToArray();
            }

            TimeSpan singleThreadTotal = TimeSpan.Zero;
            TimeSpan total = TimeSpan.Zero;

            double singleThreadTotalPoints = 0;
            double totalPoints = 0;


            List<BenchResult> result = new List<BenchResult>();


            writer.WriteHeader("Warmup");
            foreach (var bench in benchMarks)
            {
                bench.Warmup(0.05);
                writer.Write(".");
            }

            writer.WriteLine();
            writer.WriteHeader("Bench");
            int i = 1;
            foreach (var bench in benchMarks)
            {
                writer.WriteHeader($"[{i}] {bench.Name}");
                var r = bench.Bench();
                if (!bench.IsParallel)
                {
                    singleThreadTotal += r.Elapsed;
                    singleThreadTotalPoints += r.Points;
                }
                total += r.Elapsed;
                totalPoints += r.Points;
                WriteResult(r);
                result.Add(r);
                i++;
            }

            writer.WriteLine();
            writer.WriteTitle("{0,-30}", $"Total:");
            writer.WriteValue("{0,15} ms", string.Format("{0:F2}", total.TotalMilliseconds));
            writer.WriteValue("{0,15} pts", string.Format("{0:F2}", totalPoints));
            writer.WriteLine();

            var headerCommon = "Operating System,Runtime,Threads Count,Memory Used";
            var headerTotals = ",Total Points,Total Time (ms)";

            writer.WriteLine();
            writer.WriteHeader("Single-thread results");
            writer.WriteTitle(headerCommon);
            result.Where(r => !r.IsParallel).ToList().ForEach(r => writer.WriteTitle($",{r.BenchmarkName}"));
            writer.WriteTitle(headerTotals);
            writer.WriteLine();
            writer.WriteTitle($"{Environment.OSVersion},{Environment.Version},{Environment.ProcessorCount},{Environment.WorkingSet}");
            result.Where(r => !r.IsParallel).ToList().ForEach(r => writer.WriteValue(string.Format(",{0:F2}", r.Points)));
            writer.WriteTitle($",{string.Format("{0:F2}", totalPoints)},{string.Format("{0:F2}", total.TotalMilliseconds)}");

            writer.WriteLine();
            writer.WriteHeader("All results");
            writer.WriteTitle(headerCommon);
            result.ToList().ForEach(r => writer.WriteTitle($",{r.BenchmarkName}"));
            writer.WriteTitle(headerTotals);
            writer.WriteLine();
            writer.WriteTitle($"{Environment.OSVersion};{Environment.Version};{Environment.ProcessorCount},{Environment.WorkingSet}");
            result.ForEach(r => writer.WriteValue(string.Format(",{0:F2}", r.Points)));
            writer.WriteTitle($",{string.Format("{0:F2}", totalPoints)},{string.Format("{0:F2}", total.TotalMilliseconds)}");

            writer.WriteLine();
            writer.WriteHeader("Single-thread  Units results");
            writer.WriteTitle(headerCommon);
            result.Where(r => !r.IsParallel).ToList().ForEach(r => writer.WriteTitle($",{r.BenchmarkName}"));
            writer.WriteTitle(headerTotals);
            writer.WriteLine();
            writer.WriteTitle($"{Environment.OSVersion},{Environment.Version};{Environment.ProcessorCount},{Environment.WorkingSet}");
            result.Where(r => !r.IsParallel).ToList().ForEach(r => writer.WriteValue(string.Format(",{0:F2}", r.Result)));
            writer.WriteTitle($",{string.Format("{0:F2}", totalPoints)},{string.Format("{0:F2}", total.TotalMilliseconds)}");

            writer.WriteLine();
            writer.WriteHeader("Units results");
            writer.WriteTitle(headerCommon);
            result.ToList().ForEach(r => writer.WriteTitle($",{r.BenchmarkName}"));
            writer.WriteTitle(headerTotals);
            writer.WriteLine();
            writer.WriteTitle($"{Environment.OSVersion},{Environment.Version},{Environment.ProcessorCount},{Environment.WorkingSet}");
            result.ForEach(r => writer.WriteValue(string.Format(",{0:F2}", r.Result)));
            writer.WriteTitle($",{string.Format("{0:F2}", totalPoints)},{string.Format("{0:F2}", total.TotalMilliseconds)}");
        }

        private static void WriteResult(BenchResult benchResult)
        {
            writer.WriteTitle("{0,-30}", benchResult.BenchmarkName);
            writer.WriteValue("{0,15} ms", string.Format("{0:F2}", benchResult.Elapsed.TotalMilliseconds));
            writer.WriteValue("{0,15} pts", string.Format("{0:F2}", benchResult.Points));
            writer.WriteValue("{0,15} {1}", string.Format("{0:F2}", benchResult.Result), benchResult.Units);
            writer.WriteLine();
            writer.WriteValue("Iterrations: {0,15}, Ratio: {1,15}", benchResult.Iterrations, benchResult.Ratio);

            writer.WriteLine();
        }
    }
}
