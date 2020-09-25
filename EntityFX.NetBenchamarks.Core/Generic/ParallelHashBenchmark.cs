using System.Diagnostics;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class ParallelHashBenchmark : HashBase, IBenchamrk
    {
        public override BenchResult Bench()
        {
            var sw = new Stopwatch();
            sw.Start();

            Parallel.For(0, Iterrations, i =>
            {
                byte[] result = DoHash(i, ref artayOfBytes);
            });
            return BuildResult(sw);
        }
    }
}
