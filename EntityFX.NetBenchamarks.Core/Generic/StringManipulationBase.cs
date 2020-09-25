using System.Runtime.CompilerServices;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public abstract class StringManipulationBase<TResult> : BenchmarkBase<TResult>
    {
        protected double R;

        public StringManipulationBase()
        {
            Iterrations = 5000000;
            Ratio = 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static string DoStringManipilation(string str)
        {
            return (string.Join("/", str.Split(' ')).Replace("/", "_").ToUpper() + "AAA").ToLower().Replace("aaa", ".");
        }
    }
}
