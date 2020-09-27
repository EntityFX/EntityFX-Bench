using System;

namespace EntityFX.NetBenchmark.Core.Scimark2
{


    public class Scimark2
    {
        public void Bench(double min_time = Constants.RESOLUTION_DEFAULT, bool isLarge = false)
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

            Console.WriteLine();
            Console.WriteLine("SciMark 2.0a");
            Console.WriteLine();
            Console.WriteLine("Composite Score: " + res[0]);
            Console.WriteLine("FFT (" + FFT_size + "): ");
            if (res[1] == 0.0)
                Console.WriteLine(" ERROR, INVALID NUMERICAL RESULT!");

            else
                Console.WriteLine(res[1]);

            Console.WriteLine("SOR (" + SOR_size + "x" + SOR_size + "): " + "  " + res[2]);
            Console.WriteLine("Monte Carlo : " + res[3]);
            Console.WriteLine("Sparse matmult (N=" + Sparse_size_M + ", nz=" + Sparse_size_nz + "): " + res[4]);
            Console.WriteLine("LU (" + LU_size + "x" + LU_size + "): ");
            if (res[5] == 0.0)
                Console.WriteLine(" ERROR, INVALID NUMERICAL RESULT!");

            else
                Console.WriteLine(res[5]);


        }

    }
}