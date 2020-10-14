using System;
using System.Linq;
using System.Diagnostics;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public abstract class MemoryBenchmarkBase<TResult> : BenchmarkBase<TResult>
    {
        private Random random;

        Writer output = new Writer();

        protected bool UseConsole { get => output.UseConsole; set => output.UseConsole = value; }

        public MemoryBenchmarkBase(bool printToConsole = true)
        {
            Iterrations = 500000;
            Ratio = 1;
            random = new Random((int)DateTime.Now.Ticks);
            UseConsole = printToConsole;
        }

        public override void Warmup(double aspect = 0.05)
        {
            UseConsole = false;
            base.Warmup(aspect);
            UseConsole = true;
        }

        public MemoryBenchmarkResult BenchRandomMemory()
        {
            var int4k = MeasureArrayRandomRead(1024);
            output.WriteLine($"int 4k: {int4k.Item1.ToString("F2")} MB/s");
            var int512k = MeasureArrayRandomRead(131072);
            output.WriteLine($"int 512k: {int512k.Item1.ToString("F2")} MB/s");
            var int8m = MeasureArrayRandomRead(2097152);
            output.WriteLine($"int 8M: {int8m.Item1.ToString("F2")} MB/s");
            var int32m = MeasureArrayRandomRead(32 * 1024 * 1024 / sizeof(int));
            output.WriteLine($"int 32M: {int8m.Item1.ToString("F2")} MB/s");

            var long4k = MeasureArrayRandomLongRead(512);
            output.WriteLine($"long 4k: {long4k.Item1.ToString("F2")} MB/s");
            var long512k = MeasureArrayRandomLongRead(65536);
            output.WriteLine($"long 512k: {long512k.Item1.ToString("F2")} MB/s");
            var long8m = MeasureArrayRandomLongRead(1048576);
            output.WriteLine($"long 8M: {long8m.Item1.ToString("F2")} MB/s");
            var long32m = MeasureArrayRandomRead(32 * 1024 * 1024 / sizeof(long));
            output.WriteLine($"long 32M: {int8m.Item1.ToString("F2")} MB/s");

            var avg = (new double[] { int4k.Item1, int512k.Item1, int8m.Item1, long4k.Item1, long512k.Item1, long8m.Item1 }).Average();
            output.WriteLine($"Average: {avg.ToString("F2")} MB/s");

            return new MemoryBenchmarkResult()
            {
                Average = avg,
                Output = output.Output
            };
        }

        protected Tuple<double, int[]> MeasureArrayRandomRead(int size)
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

            return new Tuple<double, int[]>((long)iterInternal * array.Length * 4 / sw.Elapsed.TotalSeconds / 1024 / 1024, I);
        }

        protected Tuple<double, long[]> MeasureArrayRandomLongRead(int size)
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

            return new Tuple<double, long[]>((long)iterInternal * array.Length * 8 / sw.Elapsed.TotalSeconds / 1024 / 1024, L);
        }
    }
}
