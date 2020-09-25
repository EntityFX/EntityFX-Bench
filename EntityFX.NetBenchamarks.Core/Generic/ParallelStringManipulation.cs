using System.Diagnostics;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class ParallelStringManipulation : StringManipulationBase, IBenchamrk
    {
        public override BenchResult Bench()
        {
            var sw = new Stopwatch();
            sw.Start();
            var str = "the quick brown fox jumps over the lazy dog";
            string str1;
            Parallel.For(0, Iterrations, i =>
            {
                str1 = DoStringManipilation(str);
            });
            return BuildResult(sw);
        }
    }
}
