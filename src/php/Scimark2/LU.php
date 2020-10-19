<?php

namespace EntityFX\NetBenchmark\Core\Scimark2 {

    class LU {

        private $LU_;
        private $pivot_;

        public static function num_flops($N) {
            // rougly 2/3*N^3

            $Nd = (double)$N;

            return (2.0 * $Nd * $Nd * $Nd / 3.0);
        }

        protected static function new_copy(array $x) {
            $N = count($x);
            $T = [];
            for ($i = 0; $i < $N; $i++)
                $T[$i] = $x[$i];
            return $T;
        }

        protected static function new_copy_matrix(array $A) {
            $M = count($A);
            $N = count($A[0]);
            $T = [];

            for ($i = 0; $i < $M; $i++)
            {
                $Ti = $T[i];
                $Ai = $A[i];
                for ($j = 0; $j < $N; $j++)
                    $Ti[$j] = $Ai[$j];
            }

            return $T;
        }

        protected static function insert_copy(array &$B, array $A) {
            $M = count($A);
            $N = count($A[0]);

            $remainder = N & 3;       // N mod 4;

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

        public function getLU() {
            return self::new_copy_matrix($this->LU_);
        }

        /**
            Returns a <em>copy</em> of the pivot vector.

            @return the pivot vector used in obtaining the
            LU factorzation.  Subsequent solutions must
            permute the right-hand side by this vector.

        */
        public function getPivot() {
            return new_copy($this->pivot_);
        }

        /**
            Initalize LU factorization from matrix.

            @param A (in) the matrix to associate with this
                    factorization.
        */
        public function LU(array $A) {
            $M = count($A);
            $N = count($A[0]);

            //if ( LU_ == null || LU_.length != M || LU_[0].length != N)
            $this->LU_= [];

            self::insert_copy($this->LU_, $A);

            //if (pivot_.length != M)
            $this->pivot_= [];

            self::factor($this->LU_, $this->pivot_);
        }

        /**
            Solve a linear system, with pre-computed factorization.

            @param b (in) the right-hand side.
            @return solution vector.
        */
        public function solve(array $b) {
            $x = self::new_copy($b);

            self::solve_matrix($this->LU_, $this->pivot_, $x);
            return $x;
        }


        /**
            LU factorization (in place).

            @param A (in/out) On input, the matrix to be factored.
                On output, the compact LU factorization.

            @param pivit (out) The pivot vector records the
                reordering of the rows of A during factorization.
                
            @return 0, if OK, nozero value, othewise.
*/
        public static function factor(array &$A, array &$pivot) {
            $N = count($A);
            $M = count($A[0]);

            $minMN = min($M, $N);

            for ($j = 0; $j < $minMN; $j++)
            {
                // find pivot in column j and  test for singularity.

                $jp = $j;

                $t = abs($A[$j][$j]);
                for ($i = $j + 1; $i < $M; $i++)
                {
                    $ab = abs($A[$i][$j]);
                    if ($ab > $t)
                    {
                        $jp = $i;
                        $t = $ab;
                    }
                }

                $pivot[$j] = $jp;

                // jp now has the index of maximum element 
                // of column j, below the diagonal

                if ($A[$jp][$j] == 0)
                    return 1;       // factorization failed because of zero pivot


                if ($jp != $j)
                {
                    // swap rows j and jp
                    $tA = $A[$j];
                    $A[$j] = $A[$jp];
                    $A[$jp] = $tA;
                }

                if ($j < $M - 1)                // compute elements j+1:M of jth column
                {
                    // note A(j,j), was A(jp,p) previously which was
                    // guarranteed not to be zero (Label #1)
                    //
                    $recp = 1.0 / $A[$j][$j];

                    for ($k = $j + 1; $k < $M; $k++)
                        $A[$k][$j] *= $recp;
                }


                if ($j < $minMN - 1)
                {
                    // rank-1 update to trailing submatrix:   E = E - x*y;
                    //
                    // E is the region A(j+1:M, j+1:N)
                    // x is the column vector A(j+1:M,j)
                    // y is row vector A(j,j+1:N)


                    for ($ii = $j + 1; $ii < $M; $ii++)
                    {
                        $Aii = &$A[$ii];
                        $Aj = $A[$j];
                        $AiiJ = $Aii[$j];
                        for ($jj = $j + 1; $jj < $N; $jj++)
                            $Aii[$jj] -= $AiiJ * $Aj[$jj];

                    }
                }
            }

            return 0;
        }


        /**
            Solve a linear system, using a prefactored matrix
                in LU form.


            @param LU (in) the factored matrix in LU form. 
            @param pivot (in) the pivot vector which lists
                the reordering used during the factorization
                stage.
            @param b    (in/out) On input, the right-hand side.
                        On output, the solution vector.
        */
        public static function solve_matrix(array $LU, array $pvt, array &$b)
        {
            $N = count($LU);
            $M = count($LU[0]);
            $ii = 0;

            for ($i = 0; $i < $M; $i++)
            {
                $ip = $pvt[$i];
                $sum = $b[$ip];

                $b[$ip] = $b[$i];
                if ($ii == 0)
                    for ($j = $ii; $j < $i; $j++)
                        $sum -= $LU[$i][$j] * $b[$j];
                else
                    if ($sum == 0.0)
                    $ii = $i;
                $b[$i] = $sum;
            }

            for ($i = $N - 1; $i >= 0; $i--)
            {
                $sum = $b[$i];
                for ($j = $i + 1; $j < $N; $j++)
                    $sum -= $LU[$i][$j] * $b[$j];
                $b[$i] = $sum / $LU[$i][$i];
            }
        }
    }
}