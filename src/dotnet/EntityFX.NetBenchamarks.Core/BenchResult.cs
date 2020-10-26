using System;
using System.IO;

namespace EntityFX.NetBenchmark.Core
{

    public class BenchResult
    {
        public TimeSpan Elapsed { get; set; }

        public double Result { get; set; }

        public string Units { get; set; }

        public double Points { get; set; }

        public double Ratio { get; set; }

        public string Output { get; set; }

        public string BenchmarkName { get; set; }

        public bool IsParallel { get; set; }

        public long Iterrations { get; set; }
    }
}
