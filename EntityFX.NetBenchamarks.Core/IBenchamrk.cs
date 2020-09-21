namespace EntityFX.NetBenchamarks.Core
{
    public interface IBenchamrk
    {
        BenchResult Bench();

        void Warmup(double aspect = 0.05);
    }
}
