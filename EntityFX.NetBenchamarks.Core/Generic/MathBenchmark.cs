using System.Diagnostics;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class MathBenchmark : MathBase<double>, IBenchamrk
    {

        public override double BenchImplementation()
        {
            double R = 0;

            double li = 0;
            for (long i = 0; i < Iterrations; li = i, i++)
            {
                R += DoMath(i, li);
            }
            return R;
        }
    }
}
