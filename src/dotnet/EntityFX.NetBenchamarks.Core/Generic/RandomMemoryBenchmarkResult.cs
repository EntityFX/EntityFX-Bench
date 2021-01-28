namespace EntityFX.NetBenchmark.Core.Generic
{
    public class MemoryBenchmarkResult
    {
        public double Average { get; set; }

        public string Output { get; set; }
    }

    public class MemoryMeasureResult<TItem1, TItem2>
    {
        public TItem1 Item1 { get; set; }

        public TItem2 Item2 { get; set; }

        public MemoryMeasureResult(TItem1 item1, TItem2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
}
