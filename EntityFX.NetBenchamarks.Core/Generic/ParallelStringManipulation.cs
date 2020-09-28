using System.Diagnostics;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class ParallelStringManipulation : StringManipulationBase<BenchResult[]>
    {
        public override BenchResult[] BenchImplementation()
        {
            return BenchInParallel(() => 0, a =>
            {
                var str = "the quick brown fox jumps over the lazy dog";
                string str1 = string.Empty;
                for (int i = 0; i < Iterrations; i++)
                {
                    str1 = DoStringManipilation(str);
                }
                return str1;
            }, (a, r) => { });
        }
    }
}
