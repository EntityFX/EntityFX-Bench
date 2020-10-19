<?php

namespace EntityFX\NetBenchmark\Core\Scimark2 {

    class SOR {
        public static function num_flops($M, $N, $num_iterations) {
            $Md = (double)$M;
            $Nd = (double)$N;
            $num_iterD = (double)$num_iterations;

            return ($Md - 1) * ($Nd - 1) * $num_iterD * 6.0;
        }

        public static function execute($omega, array $G, $num_iterations) {
            $M = count($G);
            $N = count($G[0]);

            $omega_over_four = $omega * 0.25;
            $one_minus_omega = 1.0 - $omega;

            // update interior points
            //
            $Mm1 = $M - 1;
            $Nm1 = $N - 1;
            for ($p = 0; $p < $num_iterations; $p++)
            {
                for ($i = 1; $i < $Mm1; $i++)
                {
                    $Gi = $G[$i];
                    $Gim1 = $G[$i - 1];
                    $Gip1 = $G[$i + 1];
                    for ($j = 1; $j < $Nm1; $j++)
                        $Gi[$j] = $omega_over_four * 
                            ($Gim1[$j] + $Gip1[$j] + $Gi[$j - 1]
                                    + $Gi[$j + 1]) + $one_minus_omega * $Gi[$j];
                }
            }
        }
    }
}