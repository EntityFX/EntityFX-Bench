using System;
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

            GCSettings.LatencyMode = GCLatencyMode.LowLatency;
            TestArithmetics(10000);
            TestParallelArithmetics(10000);
            TestMath(10000);
            TestParallelMath(10000);
            TestCalls(10000);
            TestIfElse(10000);
            TestStringManipulation(10000);
            TestParallelStringManipulation(10000);

            if (useCrypto)
            {
                TestHash(10000);
                TestParallelHash(10000);
            }
            long result, total = 0;
            long parallelResult, parallelTotal = 0;

            var ar = TestArithmetics();
            total += ar;
            Console.WriteLine($"Arithmetics: {ar} ms");

            var par = TestParallelArithmetics();
            total += par;
            Console.WriteLine($"Arithmetics in parallel: {par} ms");

            var mt = TestMath();
            total += mt;
            Console.WriteLine($"Math: {mt} ms");

            var pmt = TestParallelMath();
            total += pmt;
            Console.WriteLine($"Math in parallel: {pmt} ms");

            var lp = TestLoops();
            total += lp;
            Console.WriteLine($"Loops: {lp} ms");

            var cal = TestCalls();
            total += cal;
            Console.WriteLine($"Calls: {cal} ms");

            var ife = TestIfElse();
            total += ife;
            Console.WriteLine($"If Else: {ife} ms");

            var str = TestStringManipulation();
            total += str;
            Console.WriteLine($"String: {str} ms");

            var pstr = TestParallelStringManipulation();
            total += pstr;
            Console.WriteLine($"String in parallel: {pstr} ms");

            long hs = 0, phs = 0;
            if (useCrypto)
            {
                hs = TestHash();
                total += hs;
                Console.WriteLine($"Hash: {hs} ms");

                phs = TestParallelHash();
                total += phs;
                Console.WriteLine($"Hash in parallel: {phs} ms");
            }
            Console.WriteLine($"Total: {total} ms");

            Console.WriteLine();
            Console.WriteLine($"{Environment.OSVersion};{Environment.Version};{Environment.ProcessorCount};{Environment.WorkingSet};{ar};{par};{mt};{pmt};{lp};{cal};{ife};{str};{pstr};{hs};{phs};{total}");

        }

        protected static double R;

        protected static long DoCall(long i)
        {
            return i + 1;
        }

        private static double DoMath(long i, double li)
        {
            double rev = 1.0 / (i + 1.0);
            return Math.Abs(i) * Math.Acos(rev) * Math.Asin(rev) * Math.Atan(rev) +
                Math.Floor(li) + Math.Exp(rev) * Math.Cos(i) * Math.Sin(i) * Math.PI + Math.Sqrt(i);
        }

        private static double DoArithmetics(long i)
        {
            return (i % 10) * (i % 100) * (i % 100) * (i % 100) * 1.11 + (i % 100) * (i % 1000) * (i % 1000) * 2.22 - i * (i % 10000) * 3.33 + i * 5.33;
        }

        public static long TestArithmetics(long count = 300000000)
        {
            var sw = new Stopwatch();
            sw.Start();
            R = 0;
            double li = 0;
            for (long i = 0; i < count; li = i, i++)
            {
                R += DoArithmetics(i);
            }
            return sw.ElapsedMilliseconds;
        }

        public static long TestParallelArithmetics(long count = 300000000)
        {
            var sw = new Stopwatch();
            sw.Start();
            R = 0;
            double li = 0;
            Parallel.For(0, count, i =>
            {
                li = i;
                R += DoArithmetics(i);
            });
            return sw.ElapsedMilliseconds;
        }

        public static long TestMath(long count = 200000000)
        {
            var sw = new Stopwatch();
            sw.Start();
            R = 0;

            double li = 0;
            for (long i = 0; i < count; li = i, i++)
            {
                R += DoMath(i, li);
            }
            return sw.ElapsedMilliseconds;
        }

        public static long TestParallelMath(long count = 200000000)
        {
            var sw = new Stopwatch();
            sw.Start();
            R = 0;

            double li = 0;
            Parallel.For(0, count, i =>
            {
                li = i;
                R += DoMath(i, li);
            });
            return sw.ElapsedMilliseconds;
        }

        public static long TestLoops(long count = 5000000000)
        {
            var sw = new Stopwatch();
            sw.Start();
            long i = 0;
            for (i = 0; i < count; ++i)
            {

            }
            i = 0;
            while (i < count)
            {
                ++i;
            }
            return sw.ElapsedMilliseconds;
        }


        public static long TestCalls(long count = 5000000000)
        {
            var sw = new Stopwatch();
            sw.Start();
            long i = 0;
            long a = 0;
            for (i = 0; i < count; ++i)
            {
                a = DoCall(i);
            }
            return sw.ElapsedMilliseconds;
        }


        public static long TestIfElse(long count = 5000000000)
        {
            var sw = new Stopwatch();
            sw.Start();
            for (long i = 0, c = -1, d = 0; i < count; i++, c--)
            {
                c = c == -4 ? -1 : c;
                if (i == -1)
                {
                    d = 3;
                }
                else if (i == -2)
                {
                    d = 2;
                }
                else if (i == -3)
                {
                    d = 1;
                }
                d = d + 1;
            }
            return sw.ElapsedMilliseconds;
        }

        private static string DoStringManipilation(string str)
        {
            return (string.Join("/", str.Split(' ')).Replace("/", "_").ToUpper() + "AAA").ToLower().Replace("aaa", ".");
        }

        public static long TestStringManipulation(int count = 5000000)
        {
            var sw = new Stopwatch();
            sw.Start();
            var str = "the quick brown fox jumps over the lazy dog";
            string str1;
            for (int i = 0; i < count; i++)
            {
                str1 = DoStringManipilation(str);
            }
            return sw.ElapsedMilliseconds;
        }

        public static long TestParallelStringManipulation(int count = 5000000)
        {
            var sw = new Stopwatch();
            sw.Start();
            var str = "the quick brown fox jumps over the lazy dog";
            string str1;
            Parallel.For(0, count, i =>
            {
                str1 = DoStringManipilation(str);
            });
            return sw.ElapsedMilliseconds;
        }

        public static byte[] DoHash(int i, ref byte[][] preparedBytes)
        {
            using (var sha = new SHA1Managed())
            using (var sha256 = new SHA256Managed())
            {
                return sha.ComputeHash(preparedBytes[i % 3])
                    .Concat(sha256.ComputeHash(preparedBytes[(i + 1) % 3]))
                    .ToArray();
            }
        }

        public static long TestHash(int count = 2000000)
        {
            var sw = new Stopwatch();
            sw.Start();
            var strs = new string[] {
            "the quick brown fox jumps over the lazy dog", "Some red wine", "Candels & Ropes" };
            var artayOfBytes = strs.Select(str => Encoding.ASCII.GetBytes(str)).ToArray();

            for (int i = 0; i < count; i++)
            {
                byte[] result = DoHash(i, ref artayOfBytes);
            }
            return sw.ElapsedMilliseconds;
        }

        public static long TestParallelHash(int count = 2000000)
        {
            var sw = new Stopwatch();
            sw.Start();
            var strs = new string[] {
            "the quick brown fox jumps over the lazy dog", "Some red wine", "Candels & Ropes" };
            var artayOfBytes = strs.Select(str => Encoding.ASCII.GetBytes(str)).ToArray();

            Parallel.For(0, count, i =>
            {
                byte[] result = DoHash(i, ref artayOfBytes);
            });
            return sw.ElapsedMilliseconds;
        }

    }
}
