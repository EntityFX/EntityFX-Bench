using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace EntityFX.NetBenchmark.Core.Generic
{

    public abstract class RandomMemoryBenchmarkBase<TResult> : BenchmarkBase<TResult>
    {

#if PocketPC
        private Dictionary<string, int> intMemTests = new Dictionary<string, int>
        {
            {"1k", 1024 / sizeof(int)}, {"32k", 32 * 1024 / sizeof(int)}, {"256k", 256 * 1024 / sizeof(int)}
        };

        private Dictionary<string, int> longMemTests = new Dictionary<string, int>
        {
            {"1k", 1024 / sizeof(long)}, {"32k", 32 * 1024 / sizeof(long)}, {"256k", 256 * 1024 / sizeof(long)}
        };
#else
        Dictionary<string, int> intMemTests = new Dictionary<string, int>
        {
            {"4k", 1024}, {"512k", 131072}, {"8M", 2097152}
        };

        Dictionary<string, int> longMemTests = new Dictionary<string, int>
        {
            {"4k", 512}, {"512k", 65536}, {"8M", 1048576}
        };
#endif

        private Random random;

        private IWriter localWriter;


        protected bool UseConsole { get { return writer.UseConsole; } set { writer.UseConsole = value; } }

        public RandomMemoryBenchmarkBase(bool printToConsole, IWriter writer)
            :base(writer)
        {
            localWriter = new Writer(null);
            Iterrations = 500000;
            Ratio = 2;
            random = new Random((int)DateTime.Now.Ticks);
            UseConsole = printToConsole;
        }

        public override void Warmup(double aspect)
        {
            UseConsole = false;
            base.Warmup(aspect);
            UseConsole = true;
        }

        public MemoryBenchmarkResult BenchRandomMemory()
        {
            double[] results = new double[intMemTests.Count + longMemTests.Count];

            int idx = 0;
            foreach (var item in intMemTests)
            {
                var res = MeasureArrayRandomRead(item.Value);
                results[idx] = res.Item1;
                localWriter.WriteLine(string.Format("Random int {0}: {1} MB/s", item.Key, res.Item1.ToString("F2")));

                idx++;
            }

            foreach (var item in longMemTests)
            {
                var res = MeasureArrayRandomLongRead(item.Value);
                results[idx] = res.Item1;
                localWriter.WriteLine(string.Format("Random long {0}: {1} MB/s", item.Key, res.Item1.ToString("F2")));

                idx++;
            }

            var avg = results.Average();
            localWriter.WriteLine(string.Format("Average: {0} MB/s", avg.ToString("F2")));
            writer.Write(localWriter.Output);
            return new MemoryBenchmarkResult()
            {
                Average = avg,
                Output = localWriter.Output
            };
        }

        protected MemoryMeasureResult<double, int> MeasureArrayRandomRead(int size)
        {
            int I = 0;
            var sw = new Stopwatch();
            var array = Enumerable.Range(0, size).Select(r => random.Next(int.MinValue, int.MaxValue)).ToArray();
            var end = array.Length - 1;
            var indexes = Enumerable.Range(0, end).Select(r => random.Next(0, end)).ToArray();
            var k1 = (size / 1024) == 0 ? 1 : (size / 1024) ;
            var iterInternal = Iterrations / k1;         
            for (int idx = 0; idx < end; idx++)
            {
                I = array[idx];
            }
            sw.Start();
            for (int i = 0; i < iterInternal; i++)
            {
                foreach (var idx in indexes)
                {
                    I = array[idx];
                }
            }
            sw.Stop();

            return new MemoryMeasureResult<double, int>((long)iterInternal * array.Length * 4 / sw.Elapsed.TotalSeconds / 1024 / 1024, I);
        }

        protected MemoryMeasureResult<double, long> MeasureArrayRandomLongRead(int size)
        {
            long L = 0;
            var sw = new Stopwatch();
            var array = Enumerable.Range(0, size).Select(r => (long)random.Next(int.MinValue, int.MaxValue)).ToArray();
            var end = array.Length - 1;
            var indexes = Enumerable.Range(0, end).Select(r => random.Next(0, end)).ToArray();
            var k1 = (size / 1024) == 0 ? 1 : (size / 1024) ;
            var iterInternal = Iterrations / k1;         
            for (int idx = 0; idx < end; idx++)
            {
                L = array[idx];
            }
            sw.Start();
            for (int i = 0; i < iterInternal; i++)
            {
                foreach (var idx in indexes)
                {
                    L = array[idx];
                }
            }
            sw.Stop();

            return new MemoryMeasureResult<double, long>((long)iterInternal * array.Length * 8 / sw.Elapsed.TotalSeconds / 1024 / 1024, L);
        }
    }
}
