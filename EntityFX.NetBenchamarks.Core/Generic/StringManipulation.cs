using System.Diagnostics;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class StringManipulation : StringManipulationBase, IBenchamrk
    {
        public override BenchResult Bench()
        {
            var sw = new Stopwatch();
            sw.Start();
            var str = "the quick brown fox jumps over the lazy dog";
            string str1;
            for (int i = 0; i < Iterrations; i++)
            {
                str1 = DoStringManipilation(str);
            }
            return BuildResult(sw);
        }
    }
}
