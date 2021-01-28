using System;

namespace EntityFX.NetBenchmark.Core.Scimark2
{

    public class Scimark2
    {
        IWriter output;

        public Scimark2(bool printToConsole)
        {
            output = new Writer(null);
            output.UseConsole = printToConsole;
        }

        public Scimark2Result Bench(double min_time, bool isLarge)
        {
            // default to the (small) cache-contained version

            int FFT_size = Constants.FFT_SIZE;
            int SOR_size = Constants.SOR_SIZE;
            int Sparse_size_M = Constants.SPARSE_SIZE_M;
            int Sparse_size_nz = Constants.SPARSE_SIZE_nz;
            int LU_size = Constants.LU_SIZE;

            // look for runtime options

            int current_arg = 0;
            if (isLarge)
            {
                FFT_size = Constants.LG_FFT_SIZE;
                SOR_size = Constants.LG_SOR_SIZE;
                Sparse_size_M = Constants.LG_SPARSE_SIZE_M;
                Sparse_size_nz = Constants.LG_SPARSE_SIZE_nz;
                LU_size = Constants.LG_LU_SIZE;

                current_arg++;
            }


            // run the benchmark

            double[] res = new double[6];
            Random R = new Random(Constants.RANDOM_SEED);

            res[1] = kernel.measureFFT(FFT_size, min_time, R);
            res[2] = kernel.measureSOR(SOR_size, min_time, R);
            res[3] = kernel.measureMonteCarlo(min_time, R);
            res[4] = kernel.measureSparseMatmult(Sparse_size_M,
                        Sparse_size_nz, min_time, R);
            res[5] = kernel.measureLU(LU_size, min_time, R);


            res[0] = (res[1] + res[2] + res[3] + res[4] + res[5]) / 5;


            // print out results

            output.WriteLine();
            output.WriteLine("SciMark 2.0a");
            output.WriteLine();
            output.WriteLine("Composite Score: {0}", res[0]);
            output.Write("FFT ({0}): ", FFT_size);
            if (res[1] == 0.0)
                output.WriteLine(" ERROR, INVALID NUMERICAL RESULT!");

            else
                output.WriteLine("{0}", res[1]);

            output.WriteLine("SOR ({0}x{1}):   {2}", SOR_size, SOR_size, res[2]);
            output.WriteLine("Monte Carlo : {0}", res[3]);
            output.WriteLine("Sparse matmult (N={0}, nz={1}): {2}", Sparse_size_M, Sparse_size_nz, res[4]);
            output.Write("LU ({0}x{1}): ", LU_size, LU_size);
            if (res[5] == 0.0)
                output.WriteLine(" ERROR, INVALID NUMERICAL RESULT!");

            else
                output.WriteLine("{0}", res[5]);

            return new Scimark2Result()
            {
                CompositeScore = res[0],
                FFT = res[1],
                SOR = res[2],
                MonteCarlo = res[3],
                SparseMathmult = res[4],
                LU = res[5],
                Output = output.Output
            };
        }

    }
}