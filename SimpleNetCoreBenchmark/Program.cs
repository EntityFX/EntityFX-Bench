using EntityFX.NetBenchmark.Core;
using EntityFX.NetBenchmark.Core.Dhrystone;
using EntityFX.NetBenchmark.Core.Generic;
using EntityFX.NetBenchmark.Core.Whetstone;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark
{
    class MainClass
    {
        private static Writer writer = new Writer();

        public static void Main(string[] args)
        {
            var useCrypto = !(args.Length > 0 && args[0] == "0");

            var benchMarks = new IBenchamrk[]
            {
                new DhrystoneBenchmark(),
                new WhetstoneBenchmark(),
                new ArithemticsBenchmark(),
                new ParallelArithemticsBenchmark(),
                new MathBenchmark(),
                new ParallelMathBenchmark(),
                new CallBenchmark(),
                new IfElseBenchmark(),
                new StringManipulation(),
                new ParallelStringManipulation()
            };

            GCSettings.LatencyMode = GCLatencyMode.LowLatency;


            if (useCrypto)
            {
                benchMarks = benchMarks.Concat(new IBenchamrk[]
                { new HashBenchmark(), new ParallelHashBenchmark() }).ToArray();
            }

            TimeSpan total = TimeSpan.Zero;
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
                total += r.Elapsed;

                WriteResult(r);
                result.Add(r);
                i++;
            }


            writer.WriteLine($"Total: {total} ms");

            writer.WriteLine();
            writer.WriteTitle($"{Environment.OSVersion};{Environment.Version};{Environment.ProcessorCount};{Environment.WorkingSet}");
            result.ForEach(r => writer.WriteValue($";{r.Elapsed.TotalMilliseconds}"));
            writer.WriteLine($";{total}");
        }

        private static void WriteResult(BenchResult benchResult)
        {
            writer.WriteTitle("{0,-25}", benchResult.BenchmarkName);
            writer.WriteValue("{0,25} ms", benchResult.Elapsed);
            writer.WriteValue("{0,15} pts", string.Format("{0:F2}", benchResult.Result));
            writer.WriteLine();
        }
    }
}
