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
                new ArithemticsBenchmark(),
                new ParallelArithemticsBenchmark(),

                new MathBenchmark(),
                new ParallelMathBenchmark(),

                new CallBenchmark(),
                new ParallelCallBenchmark(),

                new IfElseBenchmark(),
                new ParallelIfElseBenchmark(),

                new StringManipulation(),
                new ParallelStringManipulation(),

                new MemoryBenchmark(),
                new ParallelMemoryBenchmark(),

                new RandomMemoryBenchmark(),
                new ParallelRandomMemoryBenchmark(),

                new Scimark2Benchmark(),
                new ParallelScimark2Benchmark(),

                new DhrystoneBenchmark(),
                new ParallelDhrystoneBenchmark(),

                new WhetstoneBenchmark(),
                new ParallelWhetstoneBenchmark(),

                new LinpackBenchmark(),
                new ParallelLinpackBenchmark(),
            };

            GCSettings.LatencyMode = GCLatencyMode.LowLatency;


            if (useCrypto)
            {
                benchMarks = benchMarks.Concat(new IBenchamrk[]
                { new HashBenchmark(), new ParallelHashBenchmark() }).ToArray();
            }

            TimeSpan singleThreadTotal = TimeSpan.Zero;
            TimeSpan total = TimeSpan.Zero;

            double singleThreadTotalPoints = 0;
            double totalPoints = 0;


            List<BenchResult> result = new List<BenchResult>();


            writer.WriteHeader("Warmup");
            foreach (var bench in benchMarks)
            {
                bench.Warmup();
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

            writer.WriteLine();
            writer.WriteHeader("Single-thread results");
            writer.WriteTitle($"{Environment.OSVersion};{Environment.Version};{Environment.ProcessorCount};{Environment.WorkingSet}");
            result.Where(r => !r.IsParallel).ToList().ForEach(r => writer.WriteValue(string.Format(";{0:F2}", r.Points)));
            writer.WriteTitle($";{totalPoints};{total.TotalMilliseconds}");

            writer.WriteLine();
            writer.WriteHeader("All results");
            writer.WriteTitle($"{Environment.OSVersion};{Environment.Version};{Environment.ProcessorCount};{Environment.WorkingSet}");
            result.ForEach(r => writer.WriteValue(string.Format(";{0:F2}", r.Points)));
            writer.WriteTitle($";{totalPoints};{total.TotalMilliseconds}");

            writer.WriteLine();
            writer.WriteHeader("Single-thread  Units results");
            writer.WriteTitle($"{Environment.OSVersion};{Environment.Version};{Environment.ProcessorCount};{Environment.WorkingSet}");
            result.Where(r => !r.IsParallel).ToList().ForEach(r => writer.WriteValue(string.Format(";{0:F2}", r.Result)));
            writer.WriteTitle($";{totalPoints};{total.TotalMilliseconds}");

            writer.WriteLine();
            writer.WriteHeader("Units results");
            writer.WriteTitle($"{Environment.OSVersion};{Environment.Version};{Environment.ProcessorCount};{Environment.WorkingSet}");
            result.ForEach(r => writer.WriteValue(string.Format(";{0:F2}", r.Result)));
            writer.WriteTitle($";{totalPoints};{total.TotalMilliseconds}");



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
