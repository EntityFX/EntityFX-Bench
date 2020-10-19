<?php

namespace EntityFX\NetBenchmark\Core\Scimark2 {

    class SparseCompRow {
        const SEED = 113;

        public static function num_flops($N, $nz, $num_iterations) {
            /* Note that if nz does not divide N evenly, then the
               actual number of nonzeros used is adjusted slightly.
            */
            $actual_nz = intdiv($nz, $N) * $N;
            return ((double)$actual_nz) * 2.0 * ((double)$num_iterations);
        }

        /* computes  a matrix-vector multiply with a sparse matrix
            held in compress-row format.  If the size of the matrix
            in MxN with nz nonzeros, then the val[] is the nz nonzeros,
            with its ith entry in column col[i].  The integer vector row[]
            is of size M+1 and row[i] points to the begining of the
            ith row in col[].  
        */

        public static function matmult(array &$y, array $val, array $row,
            array $col, array $x, $NUM_ITERATIONS) {
            $M = count($row) - 1;

            for ($reps = 0; $reps < $NUM_ITERATIONS; $reps++)
            {

                for ($r = 0; $r < $M; $r++)
                {
                    $sum = 0.0;
                    $rowR = $row[$r];
                    $rowRp1 = $row[$r + 1];
                    for ($i = $rowR; $i < $rowRp1; $i++)
                        $sum += $x[$col[$i]] * $val[$i];
                    $y[$r] = $sum;
                }
            }
        }
    }
}