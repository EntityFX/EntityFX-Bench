<?php

namespace EntityFX\NetBenchmark\Core\Scimark2 {

    class Constants {

        const RESOLUTION_DEFAULT = 2.0;  /*secs*/
        const RANDOM_SEED = 101010;

        // default: small (cache-contained) problem sizes
        //
        const FFT_SIZE = 1024;  // must be a power of two
        const SOR_SIZE = 100; // NxN grid
        const SPARSE_SIZE_M = 1000;
        const SPARSE_SIZE_nz = 5000;
        const LU_SIZE = 100;

        // large (out-of-cache) problem sizes
        //
        const LG_FFT_SIZE = 1048576;  // must be a power of two
        const LG_SOR_SIZE = 1000; // NxN grid
        const LG_SPARSE_SIZE_M = 100000;
        const LG_SPARSE_SIZE_nz = 1000000;
        const LG_LU_SIZE = 1000;

        // tiny problem sizes (used to mainly to preload network classes 
        //                     for applet, so that network download times
        //                     are factored out of benchmark.)
        //
        const TINY_FFT_SIZE = 16;  // must be a power of two
        const TINY_SOR_SIZE = 10; // NxN grid
        const TINY_SPARSE_SIZE_M = 10;
        const TINY_SPARSE_SIZE_N = 10;
        const TINY_SPARSE_SIZE_nz = 50;
        const TINY_LU_SIZE = 10;
    }
}