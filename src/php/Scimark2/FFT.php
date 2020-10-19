<?php

namespace EntityFX\NetBenchmark\Core\Scimark2 {

    class FFT {
        public static function num_flops($N) {
            $Nd = (double)$N;
            $logN = (double)self::log2($N);

            return (5.0 * $Nd - 2) * $logN + 2 * ($Nd + 1);
        }

        /** Compute Fast Fourier Transform of (complex) data, in place.*/
        public static function transform(array $data)
        {
            self::transform_internal($data, -1);
        }

        /** Compute Inverse Fast Fourier Transform of (complex) data, in place.*/
        public static function inverse(array $data)
        {
            self::transform_internal($data, +1);
            // Normalize
            $nd = count($data);
            $n = intdiv($nd, 2);
            $norm = 1 / ((double)$n);
            for ($i = 0; $i < $nd; $i++)
                $data[$i] *= $norm;
        }

        /** Accuracy check on FFT of data. Make a copy of data, Compute the FFT, then
         * the inverse function compare to the original.  Returns the rms difference.*/
        public static function test(array $data) {
            $nd = count($data);
            // Make duplicate for comparison
            $copy = array_slice($data, 0, $nd);
            // Transform & invert
            self::transform($data);
            self::inverse($data);
            // Compute RMS difference.
            $diff = 0.0;
            for ($i = 0; $i < $nd; $i++)
            {
                $d = $data[$i] - $copy[$i];
                $diff += $d * $d;
            }
            return sqrt($diff / $nd);
        }

        /** Make a random array of n (complex) elements. */
        public static function makeRandom($n) {
            $nd = 2 * $n;
            $data = [];
            for ($i = 0; $i < $nd; $i++)
                $data[$i] = mt_rand() / mt_getrandmax();
            return $data;
        }

        /** Simple Test routine. */
        public static function main($args) {
            if (count($args) == 0) {
                $n = 1024;
                echo("n=" . $n . " => RMS Error=" + self::test(self::makeRandom($n)));
            }
            for ($i = 0; $i < count($args); $i++) {
                $n = (int)$args[$i];
                echo("n=" . $n . " => RMS Error=" . self::test(self::makeRandom($n)));
            }
        }

        /* ______________________________________________________________________ */

        protected static function log2($n)
        {
            $log = 0;
            for ($k = 1; $k < $n; $k *= 2, $log++) ;
            if ($n != (1 << $log))
                throw new Exception("FFT: Data length is not a power of 2!: " + $n);
            return $log;
        }

        protected static function transform_internal(array $data, $direction) {
            if (count($data) == 0) return;
            $n = count($data) / 2;
            if ($n == 1) return;         // Identity operation!
            $logn = self::log2($n);

            /* bit reverse the input data for decimation in time algorithm */
            self::bitreverse($data);

            /* apply fft recursion */
            /* this loop executed log2(N) times */
            for ($bit = 0, $dual = 1; $bit < $logn; $bit++, $dual *= 2)
            {
                $w_real = 1.0;
                $w_imag = 0.0;

                $theta = 2.0 * $direction * M_PI / (2.0 * (double)$dual);
                $s = sin($theta);
                $t = sin($theta / 2.0);
                $s2 = 2.0 * $t * $t;

                /* a = 0 */
                for ($b = 0; $b < $n; $b += 2 * $dual)
                {
                    $i = 2 * $b;
                    $j = 2 * ($b + $dual);

                    $wd_real = $data[$j];
                    $wd_imag = $data[$j + 1];

                    $data[$j] = $data[$i] - $wd_real;
                    $data[$j + 1] = $data[$i + 1] - $wd_imag;
                    $data[$i] += $wd_real;
                    $data[$i + 1] += $wd_imag;
                }

                /* a = 1 .. (dual-1) */
                for ($a = 1; $a < $dual; $a++)
                {
                    /* trignometric recurrence for w-> exp(i theta) w */
                    {
                        $tmp_real = $w_real - $s * $w_imag - $s2 * $w_real;
                        $tmp_imag = $w_imag + $s * $w_real - $s2 * $w_imag;
                        $w_real = $tmp_real;
                        $w_imag = $tmp_imag;
                    }
                    for ($b = 0; $b < $n; $b += 2 * $dual)
                    {
                        $i = 2 * ($b + $a);
                        $j = 2 * ($b + $a + $dual);

                        $z1_real = $data[$j];
                        $z1_imag = $data[$j + 1];

                        $wd_real = $w_real * $z1_real - $w_imag * $z1_imag;
                        $wd_imag = $w_real * $z1_imag + $w_imag * $z1_real;

                        $data[$j] = $data[$i] - $wd_real;
                        $data[$j + 1] = $data[$i + 1] - $wd_imag;
                        $data[$i] += $wd_real;
                        $data[$i + 1] += $wd_imag;
                    }
                }
            }
        }

        protected static function bitreverse(array $data) {
            /* This is the Goldrader bit-reversal algorithm */
            $n = intdiv(count($data), 2);
            $nm1 = $n - 1;
            $i = 0;
            $j = 0;
            for (; $i < $nm1; $i++)
            {

                //int ii = 2*i;
                $ii = $i << 1;

                //int jj = 2*j;
                $jj = $j << 1;

                //int k = n / 2 ;
                $k = $n >> 1;

                if ($i < $j)
                {
                    $tmp_real = $data[$ii];
                    $tmp_imag = $data[$ii + 1];
                    $data[$ii] = $data[$jj];
                    $data[$ii + 1] = $data[$jj + 1];
                    $data[$jj] = $tmp_real;
                    $data[$jj + 1] = $tmp_imag;
                }

                while ($k <= $j)
                {
                    //j = j - k ;
                    $j -= $k;

                    //k = k / 2 ; 
                    $k >>= 1;
                }
                $j += $k;
            }
        }
 
    }
}