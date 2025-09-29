using System;
namespace EntityFX.NetBenchmark.Core
{
    public static class WriterFactory
    {
        public static IWriter Build()
        {
            return Builder?.Invoke() ?? new Writer(null);
        }

        public static Func<IWriter> Builder;
    }
}
