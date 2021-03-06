﻿using System.Runtime.CompilerServices;

namespace EntityFX.NetBenchmark.Core.Generic
{
    public abstract class StringManipulationBase<TResult> : BenchmarkBase<TResult>
    {
        public StringManipulationBase(IWriter writer)
            :base(writer)
        {
            Iterrations = 5000000;
            Ratio = 10;
        }
#if NETSTANDARD2_0 || NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected static string DoStringManipilation(string str)
        {
            return (string.Join("/", str.Split(' ')).Replace("/", "_").ToUpper() + "AAA").ToLower().Replace("aaa", ".");
        }
    }
}
