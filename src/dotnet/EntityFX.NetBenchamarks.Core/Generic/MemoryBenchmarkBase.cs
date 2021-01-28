using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public abstract class MemoryBenchmarkBase<TResult> : BenchmarkBase<TResult>
    {
        private Random random;

#if PocketPC
        private Dictionary<string, int> intMemTests = new Dictionary<string, int>
            {
                {"1k", 1024 / sizeof(int)}, {"16k", 64 * 1024 / sizeof(int)}, {"64k", 512 * 1024 / sizeof(int)}, {"512K", 1 * 1024 * 1024 / sizeof(int)}
            };

        private Dictionary<string, int> longMemTests = new Dictionary<string, int>
            {
                {"1k", 1024 / sizeof(long)}, {"16k", 64 * 1024 / sizeof(long)}, {"64k", 512 * 1024 / sizeof(long)}, {"512K", 1 * 1024 * 1024 / sizeof(long)}
            };
#else
            Dictionary<string, int> intMemTests = new Dictionary<string, int>
            {
                {"4k", 1024}, {"512k", 131072}, {"8M", 2097152}, {"32M", 32 * 1024 * 1024 / sizeof(int)}
            };

            Dictionary<string, int> longMemTests = new Dictionary<string, int>
            {
                {"4k", 512}, {"512k", 65536}, {"8M", 1048576}, {"32M", 32 * 1024 * 1024 / sizeof(long)}
            };
#endif

        public MemoryBenchmarkBase(bool printToConsole, IWriter writer)
            :base(writer)
        {
            Iterrations = 500000;
            Ratio = 1;
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
            var localWriter = new Writer(null);
            double[] results = new double[intMemTests.Count + longMemTests.Count];

            int idx = 0;
            foreach (var item in intMemTests)
            {
                var res = MeasureArrayRandomRead(item.Value);
                results[idx] = res.Item1;
                localWriter.WriteLine(string.Format("int {0}: {1} MB/s", item.Key, res.Item1.ToString("F2")));

                idx++;
            }

            foreach (var item in longMemTests)
            {
                var res = MeasureArrayRandomLongRead(item.Value);
                results[idx] = res.Item1;
                localWriter.WriteLine(string.Format("long {0}: {1} MB/s", item.Key, res.Item1.ToString("F2")));

                idx++;
            }

            var avg = results.Average();
            localWriter.WriteLine(string.Format("Average: {0} MB/s", avg.ToString("F2")));

            return new MemoryBenchmarkResult()
            {
                Average = avg,
                Output = localWriter.Output
            };
        }

        protected MemoryMeasureResult<double, int[]> MeasureArrayRandomRead(int size)
        {
            int blockSize = 16;
            int[] I = new int[blockSize];
            var sw = new Stopwatch();
            var array = Enumerable.Range(0, size).Select(r => random.Next(int.MinValue, int.MaxValue)).ToArray();
            var end = array.Length - 1;

            var k1 = (size / 1024) == 0 ? 1 : (size / 1024) ;
            var iterInternal = Iterrations / k1;           

            for (int idx = 0; idx < end; idx+=blockSize)
            {
                //System.Buffer.BlockCopy(array, idx, I, 0, blockSizeBytes);
                I[0] = array[idx];
                I[1] = array[idx+1];
                I[2] = array[idx+2];
                I[3] = array[idx+3];
                I[4] = array[idx+4];
                I[5] = array[idx+5];
                I[6] = array[idx+6];
                I[7] = array[idx+7];
                I[8] = array[idx+8];
                I[9] = array[idx+9];
                I[0xA] = array[idx+0xA];
                I[0xB] = array[idx+0xB];
                I[0xC] = array[idx+0xC];
                I[0xD] = array[idx+0xD];
                I[0xE] = array[idx+0xE];
                I[0xF] = array[idx+0xF];
            }
            sw.Start();
            for (int i = 0; i < iterInternal; i++)
            {
                for (int idx = 0; idx < end; idx+=blockSize)
                {
                    //System.Buffer.BlockCopy(array, idx, I, 0, blockSizeBytes);
                    I[0] = array[idx];
                    I[1] = array[idx+1];
                    I[2] = array[idx+2];
                    I[3] = array[idx+3];
                    I[4] = array[idx+4];
                    I[5] = array[idx+5];
                    I[6] = array[idx+6];
                    I[7] = array[idx+7];
                    I[8] = array[idx+8];
                    I[9] = array[idx+9];
                    I[0xA] = array[idx+0xA];
                    I[0xB] = array[idx+0xB];
                    I[0xC] = array[idx+0xC];
                    I[0xD] = array[idx+0xD];
                    I[0xE] = array[idx+0xE];
                    I[0xF] = array[idx+0xF];
                }
            }
            sw.Stop();

            return new MemoryMeasureResult<double, int[]>((long)iterInternal * array.Length * 4 / sw.Elapsed.TotalSeconds / 1024 / 1024, I);
        }

        protected MemoryMeasureResult<double, long[]> MeasureArrayRandomLongRead(int size)
        {
            int blockSize = 8;
            long[] L = new long[blockSize];
            var sw = new Stopwatch();
            var array = Enumerable.Range(0, size).Select(r => (long)random.Next(int.MinValue, int.MaxValue)).ToArray();
            var end = array.Length - 1;
            var k1 = (size / 1024) == 0 ? 1 : (size / 1024) ;
            var iterInternal = Iterrations / k1;
            for (int idx = 0; idx < end; idx+=blockSize)
            {
                L[0] = array[idx];
                L[1] = array[idx+1];
                L[2] = array[idx+2];
                L[3] = array[idx+3];
                L[4] = array[idx+4];
                L[5] = array[idx+5];
                L[6] = array[idx+6];
                L[7] = array[idx+7];
            }
            sw.Start();
            for (int i = 0; i < iterInternal; i++)
            {
                for (int idx = 0; idx < end; idx+=blockSize)
                {
                    //System.Buffer.BlockCopy(array, idx, I, 0, blockSizeBytes);
                    L[0] = array[idx];
                    L[1] = array[idx+1];
                    L[2] = array[idx+2];
                    L[3] = array[idx+3];
                    L[4] = array[idx+4];
                    L[5] = array[idx+5];
                    L[6] = array[idx+6];
                    L[7] = array[idx+7];
                }
            }
            sw.Stop();

            return new MemoryMeasureResult<double, long[]>((long)iterInternal * array.Length * 8 / sw.Elapsed.TotalSeconds / 1024 / 1024, L);
        }
    }
}
