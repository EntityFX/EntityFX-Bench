using System;
using System.Linq;
using System.Diagnostics;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class RandomMemoryBenchmarkResult
    {
        public double Average { get; set; }

        public string Output { get; set; }
    }

    public abstract class RandomMemoryBenchmarkBase<TResult>: BenchmarkBase<TResult>
    {
        private Random random;

        Writer output = new Writer();

        protected bool UseConsole { get => output.UseConsole; set => output.UseConsole = value; }

        public RandomMemoryBenchmarkBase(bool printToConsole = true)
        {
            Iterrations = 500000;
            Ratio = 2;
            random = new Random((int)DateTime.Now.Ticks);
            UseConsole = printToConsole;
        }

        public override void Warmup(double aspect = 0.05)
        {
            UseConsole = false;
            base.Warmup(aspect);
            UseConsole = true;
        }

        public RandomMemoryBenchmarkResult BenchRandomMemory() 
        {
            var int4k = MeasureArrayRandomRead(1000);
            output.WriteLine($"Random int 4k: {int4k.Item1.ToString("F2")} MB/s");
            var int512k = MeasureArrayRandomRead(131072);
            output.WriteLine($"Random int 512k: {int512k.Item1.ToString("F2")} MB/s");
            var int8m = MeasureArrayRandomRead(2097152);
            output.WriteLine($"Random int 8M: {int8m.Item1.ToString("F2")} MB/s");

            var long4k = MeasureArrayRandomLongRead(1000);
            output.WriteLine($"Random long 4k: {long4k.Item1.ToString("F2")} MB/s");
            var long512k = MeasureArrayRandomLongRead(131072);
            output.WriteLine($"Random long 512k: {long512k.Item1.ToString("F2")} MB/s");
            var long8m = MeasureArrayRandomLongRead(2097152);
            output.WriteLine($"Random long 8M: {long8m.Item1.ToString("F2")} MB/s");

            var avg = (new double[] { int4k.Item1, int512k.Item1, int8m.Item1, long4k.Item1, long512k.Item1, long8m.Item1 }).Average();
            output.WriteLine($"Average: {avg.ToString("F2")} MB/s");

            return new RandomMemoryBenchmarkResult() 
            {
                Average = avg,
                Output = output.Output
            };
        }

        protected Tuple<double, int> MeasureArrayRandomRead(int size)
        {
            int I = 0;
            var sw = new Stopwatch();
            var array = Enumerable.Range(0, size).Select( r => random.Next(int.MinValue, int.MaxValue)).ToArray();
            var end = array.Length - 1;
            var indexes = Enumerable.Range(0, end).Select( r => random.Next(0, end)).ToArray();
            var iterInternal = Iterrations / (size / 1000);
            for (int idx = 0; idx < end; idx++)
            {
                I = array[idx];
            }
            sw.Start();
            for (int i = 0; i < iterInternal; i++)
            {
                foreach(var idx in indexes)
                {
                    I = array[idx];
                }
            }
            sw.Stop();
            
            return new Tuple<double, int>((long)iterInternal * array.Length * 4 / sw.Elapsed.TotalSeconds / 1024 / 1024, I);
        }

        protected Tuple<double, long> MeasureArrayRandomLongRead(int size)
        {
            long L = 0;
            var sw = new Stopwatch();
            var array = Enumerable.Range(0, size).Select( r => (long)random.Next(int.MinValue, int.MaxValue)).ToArray();
            var end = array.Length - 1;
            var indexes = Enumerable.Range(0, end).Select( r => random.Next(0, end)).ToArray();
            var iterInternal = Iterrations / (size / 1000);
            for (int idx = 0; idx < end; idx++)
            {
                L = array[idx];
            }
            sw.Start();
            for (int i = 0; i < iterInternal; i++)
            {
                foreach(var idx in indexes)
                {
                    L = array[idx];
                }
            }
            sw.Stop();
            
            return new Tuple<double, long>((long)iterInternal * array.Length * 8 / sw.Elapsed.TotalSeconds / 1024 / 1024, L);
        }
    }
}
