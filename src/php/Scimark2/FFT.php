<?php

namespace EntityFX\NetBenchmark\Core\Scimark2 {

    class FFT {
        public static function num_flops($N) {
            $Nd = (double)$N;
            $logN = (double)log2($N);

            return (5.0 * $Nd - 2) * $logN + 2 * ($Nd + 1);
        }
    }
}