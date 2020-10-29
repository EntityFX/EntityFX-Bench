namespace EntityFX.NetBenchmark.Core.Linpack
{
    public class LinpackResult
    {
        public double Norma { get; internal set; }
        public double Residual { get; internal set; }
        public double NormalisedResidual { get; internal set; }
        public double Epsilon { get; internal set; }
        public double Time { get; internal set; }
        public double MFLOPS { get; internal set; }
        public string Output { get; internal set; }
    }
}