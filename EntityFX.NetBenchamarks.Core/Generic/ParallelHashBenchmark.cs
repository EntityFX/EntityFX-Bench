using System.Diagnostics;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class ParallelHashBenchmark : HashBase<BenchResult[]>, IBenchamrk
    {
        public override BenchResult[] BenchImplementation()
        {
            return BenchInParallel(() => 0, a =>
            {
                byte[] hash = new byte[] { };
                for (long i = 0; i < Iterrations; i++)
                {
                    hash = DoHash(i, ref artayOfBytes);
                }
                return hash;
            }, (a, r) => { });
        }
    }
}
