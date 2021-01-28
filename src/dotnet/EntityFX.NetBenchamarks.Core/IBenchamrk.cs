namespace EntityFX.NetBenchmark.Core
{
    public interface IBenchamrk
    {
        string Name { get; }

        bool IsParallel { get; }

        BenchResult Bench();

        void Warmup(double aspect);
    }
}
