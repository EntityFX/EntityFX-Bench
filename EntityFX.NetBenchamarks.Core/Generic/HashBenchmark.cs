using System.Diagnostics;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public class HashBenchmark : HashBase<byte[]>, IBenchamrk
    {
        public override byte[] BenchImplementation()
        {
            byte[] result = new byte[] { };
            for (int i = 0; i < Iterrations; i++)
            {
                result = DoHash(i, ref artayOfBytes);
            }
            return result;
        }
    }
}
