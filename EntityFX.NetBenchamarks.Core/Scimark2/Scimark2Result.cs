namespace EntityFX.NetBenchmark.Core.Scimark2
{
    public class Scimark2Result
    {
        public double CompositeScore { get; set; }

        public double SOR { get; set; }

        public double FFT { get; set; }

        public double MonteCarlo { get; set; }

        public double SparseMathmult { get; set; }

        public double LU { get; set; }

        public string Output { get; set; }
    }
}