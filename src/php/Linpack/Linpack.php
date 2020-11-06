<?php

namespace EntityFX\NetBenchmark\Core\Linpack {
    use EntityFX\NetBenchmark\Core\Writer;

    class Linpack {

        private $output;
      
        public function __construct($printToConsole) {
            $this->output = new Writer(null);
            $this->output->UseConsole = $printToConsole;
        }
      
     
        private function abs($d) {
          return ($d >= 0) ? $d : -$d;
        }
      
        private $second_orig = -1;
      
        private function second() {
          if ($this->second_orig == -1) {
            $this->second_orig = microtime(true);
          }
          return (microtime(true) - $this->second_orig);
        }
      
        public function run_benchmark($array_size) {
          $this->output->writeLine("Running Linpack " . $array_size . "x" . $array_size . " in PHP");
          $mflops_result = 0.0;
          $residn_result = 0.0;
          $time_result = 0.0;
          $eps_result = 0.0;

          $a = [];
          for ($i=0; $i < $array_size; $i++) { 
              $a[$i] = array_fill (0, $array_size, 0.0);
          }
      
          $b = array_fill (0, $array_size, 0.0);
          $x = array_fill (0, $array_size, 0.0);
          $ops = $total = $norma = $normx = 0.0;
          $resid = $time = 0.0;
          $n = $i = $info = $lda = 0;
          $ipvt =  array_fill (0, $array_size, 0);
      
          $lda = $array_size;
          $n = $array_size;
      
          $ops = ((2.0e0 * $n) * $n * $n) / 3.0 + 2.0 * ($n * $n);
      
          $norma = $this->matgen($a, $lda, $n, $b);



          $time = $this->second();
          $info = $this->dgefa($a, $lda, $n, $ipvt);
          $this->dgesl($a, $lda, $n, $ipvt, $b, 0);
          $total = $this->second() - $time;
      
          for ($i = 0; $i < $n; $i++) {
            $x[$i] = $b[$i];
          }
          $norma = $this->matgen($a, $lda, $n, $b);
          for ($i = 0; $i < $n; $i++) {
            $b[$i] = -$b[$i];
          }
          $this->dmxpy($n, $b, $n, $lda, $x, $a);
          $resid = 0.0;
          $normx = 0.0;
          for ($i = 0; $i < $n; $i++) {
            $resid = ($resid > $this->abs($b[$i])) ? $resid : $this->abs($b[$i]);
            $normx = ($normx > $this->abs($x[$i])) ? $normx : $this->abs($x[$i]);
          }
      
          $eps_result = $this->epslon((double) 1.0);
      
          $residn_result = $resid / ($n * $norma * $normx * $eps_result);
          $residn_result += 0.005; // for rounding
          $residn_result = (int) ($residn_result * 100);
          $residn_result /= 100;
      
          $time_result = $total;
          $time_result += 0.005; // for rounding
          $time_result = (int) ($time_result * 100);
          $time_result /= 100;
      
          $mflops_result = $ops / (1.0e6 * $total);
          $mflops_result += 0.0005; // for rounding
          $mflops_result = (int) ($mflops_result * 1000);
          $mflops_result /= 1000;
      
          $this->output->writeLine("Norma is " . $norma);
          $this->output->writeLine("Residual is " . $resid);
          $this->output->writeLine("normalised residual is " . $residn_result);
          $this->output->writeLine("Machine result.Eepsilon is " . $eps_result);
          $this->output->writeLine("x[0]-1 is " . ($x[0] - 1));
          $this->output->writeLine("x[n-1]-1 is " . ($x[$n - 1] - 1));
          $this->output->writeLine("Time is " . $time_result);
          $this->output->writeLine("MFLOPS: " . $mflops_result);
          
          return [
            "Norma" => $norma,
            "Residual" => $resid,
            "NormalisedResidual" => $residn_result,
            "Epsilon" => $eps_result,
            "Time" => $time_result,
            "MFLOPS" => $mflops_result
          ];
        }
      
        private function matgen(&$a, $lda, $n, &$b) {
          $norma = 0.0;
          $i = $j = 0;
          $iseed = [];
      
          /* Magic numbers from original Linpack source */
          $iseed[0] = 1;
          $iseed[1] = 2;
          $iseed[2] = 3;
          $iseed[3] = 1325;
      
          /*
           * Next two for() statements switched. Solver wants matrix in column order.
           * --dmd 3/3/97
           */
          for ($i = 0; $i < $n; $i++) {
            for ($j = 0; $j < $n; $j++) {
              $a[$j][$i] = $this->lran($iseed) - 0.5;
              $norma = ($a[$j][$i] > $norma) ? $a[$j][$i] : $norma;
            }
          }
          for ($i = 0; $i < $n; $i++) {
            $b[$i] = 0.0;
          }
          for ($j = 0; $j < $n; $j++) {
            for ($i = 0; $i < $n; $i++) {
              $b[$i] += $a[$j][$i];
            }
          }
      
          return $norma;
        }
      
        /*
         * Taken from original Linpack source. Claims to be a multiplicative
         * congruential random number generator with modulus 2^48, storing 48 bit
         * integers as four integer array elements, 12 bits per element.
         */
        private function lran(&$seed) {
          $m1 = $m2 = $m3 = $m4 = $ipw2 = 0;
          $it1 = $it2 = $it3 = $it4 = 0;
          $r = $result = 0.0;
      
          $m1 = 494;
          $m2 = 322;
          $m3 = 2508;
          $m4 = 2549;
          $ipw2 = 4096;
      
          $r = 1.0 / $ipw2;
      
          $it4 = $seed[3] * $m4;
          $it3 = (int)($it4 / $ipw2);
          $it4 = $it4 - $ipw2 * $it3;
          $it3 = $it3 + $seed[2] * $m4 + $seed[3] * $m3;
          $it2 = (int)($it3 / $ipw2);
          $it3 = $it3 - $ipw2 * $it2;
          $it2 = $it2 + $seed[1] * $m4 + $seed[2] * $m3 + $seed[3] * $m2;
          $it1 = (int)($it2 / $ipw2);
          $it2 = $it2 - $ipw2 * $it1;
          $it1 = $it1 + $seed[0] * $m4 + $seed[1] * $m3 + $seed[2] * $m2 + $seed[3] * $m1;
          $it1 = $it1 % $ipw2;
      
          $seed[0] = $it1;
          $seed[1] = $it2;
          $seed[2] = $it3;
          $seed[3] = $it4;
      
          $result = $r * ((double) $it1 + $r * ((double) $it2 + $r * ((double) $it3 + $r * (double) $it4)));
      
          return $result;
        }
      
        private function dgefa(&$a, $lda, $n, &$ipvt) {
          $col_k = $col_j = [];
          $t = 0.0;
          $j = $k = $kp1 = $l = $nm1 = 0;
          $info = 0;
      
          // gaussian elimination with partial pivoting
      
          $nm1 = $n - 1;
          if ($nm1 >= 0) {
            for ($k = 0; $k < $nm1; $k++) {
              $col_k = &$a[$k];
              $kp1 = $k + 1;
              // find l = pivot index
      
              $l = $this->idamax($n - $k, $col_k, $k, 1) + $k;
              $ipvt[$k] = $l;
      
              // zero pivot implies this column already triangularized
      
              if ($col_k[$l] != 0) {
      
                // interchange if necessary
      
                if ($l != $k) {
                  $t = $col_k[$l];
                  $col_k[$l] = $col_k[$k];
                  $col_k[$k] = $t;
                }
      
                // compute multipliers
      
                $t = -1.0 / $col_k[$k];
                $this->dscal($n - ($kp1), $t, $col_k, $kp1, 1);
      
                // row elimination with column indexing
      
                for ($j = $kp1; $j < $n; $j++) {
                  $col_j = &$a[$j];
                  $t = $col_j[$l];
                  if ($l != $k) {
                    $col_j[$l] = $col_j[$k];
                    $col_j[$k] = $t;
                  }
                  $this->daxpy($n - ($kp1), $t, $col_k, $kp1, 1, $col_j, $kp1, 1);
                }
              } else {
                $info = $k;
              }
            }
          }

          $ipvt[$n - 1] = $n - 1;
          if ($a[($n - 1)][($n - 1)] == 0)
            $info = $n - 1;
      
          return $info;
        }
      
        private function dgesl(&$a, $lda, $n, &$ipvt, &$b, $job) {
          $t = 0.0;
          $k = $kb = $l = $nm1 = $kp1 = 0;
      
          $nm1 = $n - 1;
          if ($job == 0) {
      
            // job = 0 , solve a * x = b. first solve l*y = b
      
            if ($nm1 >= 1) {
              for ($k = 0; $k < $nm1; $k++) {
                $l = $ipvt[$k];
                $t = $b[$l];
                if ($l != $k) {
                  $b[$l] = $b[$k];
                  $b[$k] = $t;
                }
                $kp1 = $k + 1;
                $this->daxpy($n - ($kp1), $t, $a[$k], $kp1, 1, $b, $kp1, 1);
              }
            }
      
            // now solve u*x = y
      
            for ($kb = 0; $kb < $n; $kb++) {
              $k = $n - ($kb + 1);
              $b[$k] /= $a[$k][$k];
              $t = -$b[$k];
              $this->daxpy($k, $t, $a[$k], 0, 1, $b, 0, 1);
            }
          } else {
      
            // job = nonzero, solve trans(a) * x = b. first solve trans(u)*y = b
      
            for ($k = 0; $k < $n; $k++) {
              $t = $this->ddot($k, $a[$k], 0, 1, $b, 0, 1);
              $b[$k] = ($b[$k] - $t) / $a[$k][$k];
            }
      
            // now solve trans(l)*x = y
      
            if ($nm1 >= 1) {
              for ($kb = 1; $kb < $nm1; $kb++) {
                $k = $n - ($kb + 1);
                $kp1 = $k + 1;
                $b[$k] += $this->ddot($n - ($kp1), $a[$k], $kp1, 1, $b, $kp1, 1);
                $l = $ipvt[$k];
                if ($l != $k) {
                  $t = $b[$l];
                  $b[$l] = $b[$k];
                  $b[$k] = $t;
                }
              }
            }
          }
        }
      
        /*
         * constant times a vector plus a vector. jack dongarra, linpack, 3/11/78.
         */
        private function daxpy($n, $da, &$dx, $dx_off, $incx, &$dy, $dy_off, $incy) {
          $i = $ix = $iy = 0;
      
          if (($n > 0) && ($da != 0)) {
            if ($incx != 1 || $incy != 1) {
      
              // code for unequal increments or equal increments not equal to 1
      
              $ix = 0;
              $iy = 0;
              if ($incx < 0)
                $ix = (-$n + 1) * $incx;
              if ($incy < 0)
                $iy = (-$n + 1) * $incy;
              for ($i = 0; $i < $n; $i++) {
                $dy[$iy + $dy_off] += $da * $dx[$ix + $dx_off];
                $ix += $incx;
                $iy += $incy;
              }
              return;
            } else {
      
              // code for both increments equal to 1
      
              for ($i = 0; $i < $n; $i++)
                $dy[$i + $dy_off] += $da * $dx[$i + $dx_off];
            }
          }
        }
      
        /*
         * forms the dot product of two vectors. jack dongarra, linpack, 3/11/78.
         */
        private function ddot($n, &$dx, $dx_off, $incx, &$dy, $dy_off, $incy) {
          $dtemp = 0.0;
          $i = $ix = $iy;
      
          $dtemp = 0;
      
          if ($n > 0) {
      
            if ($incx != 1 || $incy != 1) {
      
              // code for unequal increments or equal increments not equal to 1
      
              $ix = 0;
              $iy = 0;
              if ($incx < 0)
                $ix = (-$n + 1) * $incx;
              if ($incy < 0)
                $iy = (-$n + 1) * $incy;
              for ($i = 0; $i < $n; $i++) {
                $dtemp += $dx[$ix + $dx_off] * $dy[$iy + $dy_off];
                $ix += $incx;
                $iy += $incy;
              }
            } else {
      
              // code for both increments equal to 1
      
              for ($i = 0; $i < $n; $i++)
                $dtemp += $dx[$i + $dx_off] * $dy[$i + $dy_off];
            }
          }
          return ($dtemp);
        }
      
        /*
         * scales a vector by a constant. jack dongarra, linpack, 3/11/78.
         */
        private function dscal($n, $da, &$dx, $dx_off, $incx) {
          $i = $nincx = 0;
      
          if ($n > 0) {
            if ($incx != 1) {
      
              // code for increment not equal to 1
      
              $nincx = $n * $incx;
              for ($i = 0; $i < $nincx; $i += $incx)
                $dx[$i + $dx_off] *= $da;
            } else {
      
              // code for increment equal to 1
      
              for ($i = 0; $i < $n; $i++)
                $dx[$i + $dx_off] *= $da;
            }
          }
        }
      
        /*
         * finds the index of element having max. absolute value. jack dongarra,
         * linpack, 3/11/78.
         */
        private function idamax($n, &$dx, $dx_off, $incx) {
          $dmax = $dtemp = 0.0;
          $i = $ix = $itemp = 0;
      
          if ($n < 1) {
            $itemp = -1;
          } else if ($n == 1) {
            $itemp = 0;
          } else if ($incx != 1) {
      
            // code for increment not equal to 1
      
            $dmax = $this->abs($dx[0 + $dx_off]);
            $ix = 1 + $incx;
            for ($i = 1; $i < $n; $i++) {
              $dtemp = $this->abs($dx[$ix + $dx_off]);
              if ($dtemp > $dmax) {
                $itemp = $i;
                $dmax = $dtemp;
              }
              $ix += $incx;
            }
          } else {
      
            // code for increment equal to 1
      
            $itemp = 0;
            $dmax = $this->abs($dx[0 + $dx_off]);
            for ($i = 1; $i < $n; $i++) {
              $dtemp = $this->abs($dx[$i + $dx_off]);
              if ($dtemp > $dmax) {
                $itemp = $i;
                $dmax = $dtemp;
              }
            }
          }
          return ($itemp);
        }
      
        private function epslon($x) {
          $a = $b = $c = $eps = 0;
      
          $a = 4.0e0 / 3.0e0;
          while ($eps == 0) {
            $b = $a - 1.0;
            $c = $b + $b + $b;
            $eps = $this->abs($c - 1.0);
          }
          return ($eps * $this->abs($x));
        }
      
        private function dmxpy($n1, &$y, $n2, $ldm, &$x, &$m) {
          $j = $i = 0;
      
          // cleanup odd vector
          for ($j = 0; $j < $n2; $j++) {
            for ($i = 0; $i < $n1; $i++) {
              $y[$i] += $x[$j] * $m[$j][$i];
            }
          }
        }
      
      }
}