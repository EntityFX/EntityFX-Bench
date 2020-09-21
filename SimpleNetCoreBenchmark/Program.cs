using EntityFX.NetBenchamarks.Core;
using EntityFX.NetBenchamarks.Core.Dhrystone;
using EntityFX.NetBenchamarks.Core.Generic;
using EntityFX.NetBenchamarks.Core.Whetstone;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetBenchmark
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var useCrypto = args.Length > 0 && args[0] == "0" ? false : true;

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

            foreach (var bench in benchMarks)
            {
                bench.Warmup();
            }

            foreach (var bench in benchMarks)
            {
                var r = bench.Bench();
                total += r.Elapsed;
                Console.WriteLine($"{r.BenchmarkName}: {r.Elapsed} ms, Result: {r.Result}");
                result.Add(r);
            }


            Console.WriteLine($"Total: {total} ms");

            Console.WriteLine();
            Console.Write($"{Environment.OSVersion};{Environment.Version};{Environment.ProcessorCount};{Environment.WorkingSet}");
            result.ForEach(r => Console.Write($";{r.Elapsed.TotalMilliseconds}"));
            Console.WriteLine($";{total}");
        }
    }
}
