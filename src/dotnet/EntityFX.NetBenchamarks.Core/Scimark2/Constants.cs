namespace EntityFX.NetBenchmark.Core.Scimark2
{
    public class Constants
    {

        public const double RESOLUTION_DEFAULT = 2.0;  /*secs*/
        public const int RANDOM_SEED = 101010;

        // default: small (cache-contained) problem sizes
        //
        public const int FFT_SIZE = 1024;  // must be a power of two
        public const int SOR_SIZE = 100; // NxN grid
        public const int SPARSE_SIZE_M = 1000;
        public const int SPARSE_SIZE_nz = 5000;
        public const int LU_SIZE = 100;

        // large (out-of-cache) problem sizes
        //
        public const int LG_FFT_SIZE = 1048576;  // must be a power of two
        public const int LG_SOR_SIZE = 1000; // NxN grid
        public const int LG_SPARSE_SIZE_M = 100000;
        public const int LG_SPARSE_SIZE_nz = 1000000;
        public const int LG_LU_SIZE = 1000;

        // tiny problem sizes (used to mainly to preload network classes 
        //                     for applet, so that network download times
        //                     are factored out of benchmark.)
        //
        public const int TINY_FFT_SIZE = 16;  // must be a power of two
        public const int TINY_SOR_SIZE = 10; // NxN grid
        public const int TINY_SPARSE_SIZE_M = 10;
        public const int TINY_SPARSE_SIZE_N = 10;
        public const int TINY_SPARSE_SIZE_nz = 50;
        public const int TINY_LU_SIZE = 10;

    }
}