<?php

namespace EntityFX\NetBenchmark\Core\Scimark2 {

    class MonteCarlo {
        const SEED = 113;

        public static function num_flops($Num_samples) {
            // 3 flops in x^2+y^2 and 1 flop in random routine

            return ((double)$Num_samples) * 4.0;

        }

        public static function integrate($Num_samples) {
            $under_curve = 0;
            for ($count = 0; $count < $Num_samples; $count++)
            {
                $x = mt_rand() / mt_getrandmax();
                $y = mt_rand() / mt_getrandmax();

                if ($x * $x + $y * $y <= 1.0)
                    $under_curve++;

            }

            return ((double)$under_curve / $Num_samples) * 4.0;
        }
    }
}