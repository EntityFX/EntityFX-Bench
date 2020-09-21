using System;
using System.IO;

namespace EntityFX.NetBenchamarks.Core
{

    public class BenchResult
    {
        public TimeSpan Elapsed { get; set; }

        public object Result { get; set; }

        public string Output { get; set; }

        public string BenchmarkName { get; set; }
    }
}
