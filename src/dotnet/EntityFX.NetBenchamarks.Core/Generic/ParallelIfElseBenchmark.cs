namespace EntityFX.NetBenchmark.Core.Generic
{
    public class ParallelIfElseBenchmark : BenchmarkBase<BenchResult[]>, IBenchamrk
    {
        public ParallelIfElseBenchmark(IWriter writer)
            :base(writer)
        {
            Iterrations = 2000000000;
            Ratio = 0.01;
            IsParallel = true;
        }

        public override BenchResult[] BenchImplementation()
        {
            return BenchInParallel(() => 0, a =>
            {
                int d = 0;
                for (long i = 0, c = -1; i < Iterrations; i++, c--)
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
                return d;
            }, (a, r) => { });
        }
    }
}
