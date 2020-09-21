using System.Runtime.CompilerServices;

namespace EntityFX.NetBenchamarks.Core.Generic
{
    public abstract class StringManipulationBase : BenchmarkBase
    {
        protected double R;

        public StringManipulationBase()
        {
            Iterrations = 5000000;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static string DoStringManipilation(string str)
        {
            return (string.Join("/", str.Split(' ')).Replace("/", "_").ToUpper() + "AAA").ToLower().Replace("aaa", ".");
        }
    }
}
