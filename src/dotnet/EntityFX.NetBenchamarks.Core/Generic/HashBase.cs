using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public abstract class HashBase<TResult> : BenchmarkBase<TResult>
    {
        protected double R;

        protected string[] strs = new string[] {
            "the quick brown fox jumps over the lazy dog", 
            "Some red wine", 
            "Candels & Ropes" };

        protected byte[][] artayOfBytes;

        public HashBase(IWriter writer)
            :base(writer)
        {
            Iterrations = 2000000;
            Ratio = 10;
            artayOfBytes = strs.Select(str => Encoding.ASCII.GetBytes(str)).ToArray();
        }

#if NETSTANDARD2_0 || NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected static byte[] DoHash(long i, ref byte[][] preparedBytes)
        {
#if NETSTANDARD2_0 || NET45
            using (var sha = new SHA1Managed())
            using (var sha256 = new SHA256Managed())
            {
                return sha.ComputeHash(preparedBytes[i % 3])
                    .Concat(sha256.ComputeHash(preparedBytes[(i + 1) % 3]))
                    .ToArray();
            }
#else
            using (var sha = new SHA1Managed())
            using (var md5 = new MD5CryptoServiceProvider())
            {
                return sha.ComputeHash(preparedBytes[i % 3])
                    .Concat(md5.ComputeHash(preparedBytes[(i + 1) % 3]))
                    .ToArray();
            }
#endif
        }
    }
}
