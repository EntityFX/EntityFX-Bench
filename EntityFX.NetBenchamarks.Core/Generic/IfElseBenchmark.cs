using System.Diagnostics;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class IfElseBenchmark : BenchmarkBase, IBenchamrk
    {
        public IfElseBenchmark()
        {
            Iterrations = 5000000000;
        }

        public override BenchResult Bench()
        {
            var sw = new Stopwatch();
            sw.Start();
            for (long i = 0, c = -1, d = 0; i < Iterrations; i++, c--)
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
            return BuildResult(sw);
        }
    }
}
