<?php

namespace EntityFX\NetBenchmark\Core\Scimark2 {

    class kernel
    {
        // each measurement returns approx Mflops


        public static function measureFFT($N, $mintime) {
            // initialize FFT data as complex (N real/img pairs)

            $x = self::RandomVector(2 * $N);
            $oldx = self::NewVectorCopy($x);
            $cycles = 1;

            while (true)
            {
                $start = microtime(true);
                for ($i = 0; $i < $cycles; $i++)
                {
                    FFT::transform($x);   // forward transform
                    FFT::inverse($x);     // backward transform
                }
                $elapsed = microtime(true) - $start;
                if ($elapsed >= $mintime)
                    break;

                $cycles *= 2;
            }
            // approx Mflops

            $EPS = 1.0e-10;
            if (FFT::test($x) / $N > $EPS)
                return 0.0;

            return FFT::num_flops($N) * $cycles / $elapsed * 1.0e-6;
        }


        public static function measureSOR($N, $min_time) {
            $G = self::RandomMatrix($N, $N);

            $cycles = 1;
            while (true)
            {
                $start = microtime(true);
                SOR::execute(1.25, $G, $cycles);
                $elapsed = microtime(true) - $start;
                if ($elapsed >= $min_time) break;

                $cycles *= 2;
            }
            // approx Mflops
            return SOR::num_flops($N, $N, $cycles) / $elapsed * 1.0e-6;
        }

        public static function measureMonteCarlo($min_time) {
            $start = microtime(true);

            $cycles = 1;
            while (true)
            {
                $start = microtime(true);
                MonteCarlo::integrate($cycles);
                $elapsed = microtime(true) - $start;
                if ($elapsed >= $min_time) break;

                $cycles *= 2;
            }
            // approx Mflops
            return MonteCarlo::num_flops($cycles) / $elapsed * 1.0e-6;
        }


        public static function measureSparseMatmult($N, $nz,
                $min_time) {
            // initialize vector multipliers and storage for result
            // y = A*y;

            $x = self::RandomVector($N);
            $y = [];

            // initialize square sparse matrix
            //
            // for this test, we create a sparse matrix wit M/nz nonzeros
            // per row, with spaced-out evenly between the begining of the
            // row to the main diagonal.  Thus, the resulting pattern looks
            // like
            //             +-----------------+
            //             +*                +
            //             +***              +
            //             +* * *            +
            //             +** *  *          +
            //             +**  *   *        +
            //             +* *   *   *      +
            //             +*  *   *    *    +
            //             +*   *    *    *  + 
            //             +-----------------+
            //
            // (as best reproducible with integer artihmetic)
            // Note that the first nr rows will have elements past
            // the diagonal.

            $nr = intdiv($nz, $N);        // average number of nonzeros per row
            $anz = $nr * $N;   // _actual_ number of nonzeros


            $val = self::RandomVector($anz);
            $col = [];
            $row = [];

            $row[0] = 0;
            for ($r = 0; $r < $N; $r++)
            {
                // initialize elements for row r

                $rowr = $row[$r];
                $row[$r + 1] = $rowr + $nr;
                $step = intdiv($r, $nr);
                if ($step < 1) $step = 1;   // take at least unit steps


                for ($i = 0; $i < $nr; $i++)
                    $col[$rowr + $i] = $i * $step;

            }

            $cycles = 1;
            while (true)
            {
                $start = microtime(true);
                SparseCompRow::matmult($y, $val, $row, $col, $x, $cycles);
                $elapsed = microtime(true) - $start;
                if ($elapsed >= $min_time) break;

                $cycles *= 2;
            }
            // approx Mflops
            return SparseCompRow::num_flops($N, $nz, $cycles) / $elapsed * 1.0e-6;
        }


        public static function measureLU($N, $min_time)
        {
            // compute approx Mlfops, or O if LU yields large errors

            $A = self::RandomMatrix($N, $N);
            $lu = [];
            for($i = 0; $i < $N; $i++)
            {
                $lu[$i] = [];
            }
            $pivot = [];


            $cycles = 1;
            while (true)
            {
                $start = microtime(true);
                for ($i = 0; $i < $cycles; $i++)
                {
                    self::CopyMatrix($lu, $A);
                    LU::factor($lu, $pivot);
                }
                $elapsed = microtime(true) - $start;
                if ($elapsed >= $min_time) break;

                $cycles *= 2;
            }

            // verify that LU is correct
            $b = self::RandomVector($N);
            $x = self::NewVectorCopy($b);

            LU::solve_matrix($lu, $pivot, $x);


            $EPS = 1.0e-12;
            if (self::normabs($b, self::matvec($A, $x)) / $N > $EPS)
                return 0.0;


            // else return approx Mflops
            //
            return LU::num_flops($N) * $cycles / $elapsed * 1.0e-6;
        }


        private static function NewVectorCopy(array $x) {
            $N = count($x);

            $y = [];
            for ($i = 0; $i < $N; $i++)
                $y[$i] = $x[$i];

            return $y;
        }

        private static function CopyVector(array $B, array $A) {
            $N = count($A);

            for ($i = 0; $i < $N; $i++)
                $B[$i] = $A[$i];
        }


        private static function normabs(array $x, array $y) {
            $N = count($x);
            $sum = 0.0;

            for ($i = 0; $i < $N; $i++)
                $sum += abd($x[$i] - $y[$i]);

            return $sum;
        }

        private static function CopyMatrix(array &$B, array $A) {
            $M = count($A);
            $N = count($A[0]);

            $remainder = $N & 3;       // N mod 4;

            for ($i = 0; $i < $M; $i++)
            {
                $Bi = $B[$i];
                $Ai = $A[$i];
                for ($j = 0; $j < $remainder; $j++)
                    $Bi[$j] = $Ai[$j];
                for ($j = $remainder; $j < $N; $j += 4)
                {
                    $Bi[$j] = $Ai[$j];
                    $Bi[$j + 1] = $Ai[$j + 1];
                    $Bi[$j + 2] = $Ai[$j + 2];
                    $Bi[$j + 3] = $Ai[$j + 3];
                }
            }
        }

        private static function RandomMatrix($M, $N) {
            $A = [];

            for ($i = 0; $i < $N; $i++)
            {
                $A[$i] = [];
                for ($j = 0; $j < $N; $j++)
                    $A[$i][$j] = mt_rand() / mt_getrandmax();
            }
            return $A;
        }

        private static function RandomVector($N) {
            $A = [];

            for ($i = 0; $i < $N; $i++)
                $A[$i] = mt_rand() / mt_getrandmax();
            return $A;
        }

        private static function matvec(array $A, array $x) {
            $N = count($x);
            $y = [];

            self::matvec2($A, $x, $y);

            return $y;
        }

        private static function matvec2(array $A, array $x, array &$y) {
            $M = count($A);
            $N =  count($A[0]);

            for ($i = 0; $i < $M; $i++)
            {
                $sum = 0.0;
                $Ai = $A[$i];
                for ($j = 0; $j < $N; $j++)
                    $sum += $Ai[$j] * $x[$j];

                $y[$i] = $sum;
            }
        }

    }
}