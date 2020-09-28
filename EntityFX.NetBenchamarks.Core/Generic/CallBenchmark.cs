using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class CallBenchmark : BenchmarkBase<int>, IBenchamrk
    {
        public CallBenchmark()
        {
            Iterrations = 2000000000;
            Ratio = 0.01;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int DoCall(int i)
        {
            return i + 1;
        }

        public override int BenchImplementation()
        {
            int i = 0;
            int a = 0;
            for (i = 0; i < Iterrations; ++i)
            {
                a = DoCall(i);
            }
            return a;
        }
    }
}
