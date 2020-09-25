using System;
using System.IO;

namespace EntityFX.NetBenchmark.Core
{

    public class BenchResult
    {
        public TimeSpan Elapsed { get; set; }

        public object Result { get; set; }

        public decimal Points { get; set; }

        public string Output { get; set; }

        public string BenchmarkName { get; set; }
    }
}
