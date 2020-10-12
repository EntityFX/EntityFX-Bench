namespace EntityFX.NetBenchmark.Core
{
    public interface IBenchamrk
    {
        string Name { get; }

        BenchResult Bench();

        void Warmup(double aspect = 0.05);
    }
}
