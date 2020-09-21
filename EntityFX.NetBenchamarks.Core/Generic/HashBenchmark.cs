using System.Diagnostics;

namespace EntityFX.NetBenchamarks.Core.Generic
{
    public class HashBenchmark : HashBase, IBenchamrk
    {
        public override BenchResult Bench()
        {
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < Iterrations; i ++)
            {
                byte[] result = DoHash(i, ref artayOfBytes);
            }
            return BuildResult(sw);
        }
    }
}
