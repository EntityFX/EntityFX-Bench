using System.Diagnostics;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class ArithemticsBenchmark : ArithmeticsBase<double>, IBenchamrk
    {
        public ArithemticsBenchmark(IWriter writer) : base(writer)
        {
        }

        public override double BenchImplementation()
        {
            R = 0;
            for (long i = 0; i < Iterrations; i++)
            {
                R += DoArithmetics(i);
            }
            return R;
        }
    }
}
