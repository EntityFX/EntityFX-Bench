<?php

namespace EntityFX\NetBenchmark\Core\Whetstone {
    use EntityFX\NetBenchmark\Core\Writer;
    
    class Whetstone {
        private $loop_time = [];
        private $loop_mops = [];
        private $loop_mflops = [];
        private $TimeUsed = 0;
        private $mwips = 0.0;
        private $headings = [];
        private $Check;
        private $results = [];

        private $output;

        public function __construct($printToConsole) {
            $this->output = new Writer();
            $this->output->UseConsole = $printToConsole;
        }

        public function bench($getinput)
        {
            $count = 10;
            $calibrate = 1;
            $xtra = 1;
            $endit = $section = 0;
            $x100 = 100;
            $duration = 100;
            $general = [];

            $this->WriteLine("%s Precision PHP Whetstone Benchmark", "Double");
            $this->WriteLine("                 PHP version: %s", phpversion());
            if (!$getinput)
            {
                $this->WriteLine("No run time input data\n");
            }
            else
            {
                $this->WriteLine("With run time input data\n");
            }

            $this->WriteLine("Calibrate");

            do
            {
                $this->TimeUsed = 0;

                $this->Whetstones($xtra, $x100, $calibrate);

                $this->WriteLine("%11d Seconds %10d   Passes (x 100)",
                    $this->TimeUsed, $xtra);
                $calibrate = $calibrate + 1;
                $count = $count - 1;
                if ($this->TimeUsed > 2.0)
                    $count = 0;
                else
                    $xtra = $xtra * 5;
            }
            while ($count > 0);

            if ($this->TimeUsed > 0)
                $xtra = (int)((float)($duration * $xtra) / $this->TimeUsed);
            if ($xtra < 1) $xtra = 1;

            $calibrate = 0;

            $this->WriteLine("Use %d  passes (x 100)", $xtra);

            $this->WriteLine("          %s Precision PHP Whetstone Benchmark",
                                                                   "Double");
            $this->output->WriteNewLine();
            $this->WriteLine("Loop content                  Result              MFLOPS " .
                                     "     MOPS   Seconds");

            $this->TimeUsed = 0;
            $this->Whetstones($xtra, $x100, $calibrate);

            $this->output->Write("MWIPS            ");
            if ($this->TimeUsed > 0)
                $this->mwips = (float)($xtra) * (float)($x100) / (10 * (float)$this->TimeUsed);
            else
                $this->mwips = 0;

            $this->output->WriteLine("%39.3f %19.3f", $this->mwips, $this->TimeUsed);

            if ($this->Check == 0)
                $this->WriteLine("Wrong answer  ");

            return [
                "Output" => $this->output,
                "MWIPS" => $this->mwips,
                "TimeUsed" => $this->TimeUsed
            ];
        }

        private function whetstones($xtra, $x100, $calibrate)
        {
            $n1= $n2 = $n3 = $n4 = $n5 = $n6 = $n7 = $n8 = $i = $ix = $n1mult = 0;
            $x = $y = $z = 0.0;
            $j = $k = $l = 0;
            $e1 = [];
            $timea = 0.0;
            $timeb = 0.0;

            $t = 0.49999975;
            $t0 = $t;
            $t1 = 0.50000025;
            $t2 = 2;

            $this->Check = 0;

            $n1 = 12 * $x100;
            $n2 = 14 * $x100;
            $n3 = 345 * $x100;
            $n4 = 210 * $x100;
            $n5 = 32 * $x100;
            $n6 = 899 * $x100;
            $n7 = 616 * $x100;
            $n8 = 93 * $x100;
            $n1mult = 10;

            /* Section 1, Array elements */

            $e1[0] = 1;
            $e1[1] = -1;
            $e1[2] = -1;
            $e1[3] = -1;

            $timea = microtime(true);

            for ($ix = 0; $ix < $xtra; $ix++)
            {
                for ($i = 0; $i < $n1 * $n1mult; $i++)
                {
                    $e1[0] = ($e1[0] + $e1[1] + $e1[2] - $e1[3]) * $t;
                    $e1[1] = ($e1[0] + $e1[1] - $e1[2] + $e1[3]) * $t;
                    $e1[2] = ($e1[0] - $e1[1] + $e1[2] + $e1[3]) * $t;
                    $e1[3] = (-$e1[0] + $e1[1] + $e1[2] + $e1[3]) * $t;
                }
                $t = 1 - $t;
            }
            $t = $t0;

            $timeb = (microtime(true) - $timea) / $n1mult;
            $this->Pout("N1 floating point", (float)($n1 * 16) * (float)($xtra),
                             1, $e1[3], $timeb, $calibrate, 1);

            /* Section 2, Array as parameter */

            $timea = microtime(true);
            {
                for ($ix = 0; $ix < $xtra; $ix++)
                {
                    for ($i = 0; $i < $n2; $i++)
                    {
                        self::Pa($e1, $t, $t2);
                    }
                    $t = 1 - $t;
                }
                $t = $t0;
            }
            $timeb = microtime(true) - $timea;
            $this->Pout("N2 floating point", (float)($n2 * 96) * (float)($xtra),
                                 1, $e1[3], $timeb, $calibrate, 2);

            /* Section 3, Conditional jumps */
            $j = 1;
            $timea = microtime(true);
            {
                for ($ix = 0; $ix < $xtra; $ix++)
                {
                    for ($i = 0; $i < $n3; $i++)
                    {
                        if ($j == 1) $j = 2;
                        else $j = 3;
                        if ($j > 2) $j = 0;
                        else $j = 1;
                        if ($j < 1) $j = 1;
                        else $j = 0;
                    }
                }
            }
            $timeb = microtime(true) - $timea;
            $this->Pout("N3 if then else  ", (float)($n3 * 3) * (float)($xtra),
                            2, (float)($j), $timeb, $calibrate, 3);

            /* Section 4, Integer arithmetic */
            $j = 1;
            $k = 2;
            $l = 3;
            $timea = microtime(true);
            {
                for ($ix = 0; $ix < $xtra; $ix++)
                {
                    for ($i = 0; $i < $n4; $i++)
                    {
                        $j = $j * ($k - $j) * ($l - $k);
                        $k = $l * $k - ($l - $j) * $k;
                        $l = ($l - $k) * ($k + $j);
                        $e1[$l - 2] = $j + $k + $l;
                        $e1[$k - 2] = $j * $k * $l;
                    }
                }
            }
            $timeb = microtime(true) - $timea;
            $x = $e1[0] + $e1[1];
            $this->Pout("N4 fixed point   ", (float)($n4 * 15) * (float)($xtra),
                                     2, $x, $timeb, $calibrate, 4);

            /* Section 5, Trig functions */
            $x = 0.5;
            $y = 0.5;
            $timea = microtime(true);
            {
                for ($ix = 0; $ix < $xtra; $ix++)
                {
                    for ($i = 1; $i < $n5; $i++)
                    {
                        $x = (float)($t * atan($t2 * sin($x) * cos($x) / (cos($x + $y) + cos($x - $y) - 1.0)));
                        $y = (float)($t * atan($t2 * sin($y) * cos($y) / (cos($x + $y) + cos($x - $y) - 1.0)));
                    }
                    $t = 1 - $t;
                }
                $t = $t0;
            }
            $timeb = microtime(true) - $timea;
            $this->Pout("N5 sin,cos etc.  ", (float)($n5 * 26) * (float)($xtra),
                                     2, $y, $timeb, $calibrate, 5);

            /* Section 6, Procedure calls */
            $x = 1;
            $y = 1;
            $z = 1;
            $timea = microtime(true);
            {
                for ($ix = 0; $ix < $xtra; $ix++)
                {
                    for ($i = 0; $i < $n6; $i++)
                    {
                        self::P3($x, $y, $z, $t, $t1, $t2);
                    }
                }
            }
            $timeb = microtime(true) - $timea;
            $this->Pout("N6 floating point", (float)($n6 * 6) * (float)($xtra),
                                    1, $z, $timeb, $calibrate, 6);

            /* Section 7, Array refrences */
            $j = 0;
            $k = 1;
            $l = 2;
            $e1[0] = 1;
            $e1[1] = 2;
            $e1[2] = 3;
            $timea = microtime(true);
            {
                for ($ix = 0; $ix < $xtra; $ix++)
                {
                    for ($i = 0; $i < $n7; $i++)
                    {
                        self::Po($e1, $j, $k, $l);
                    }
                }
            }
            $timeb = microtime(true) - $timea;
            $this->Pout("N7 assignments   ", (float)($n7 * 3) * (float)($xtra),
                                2, $e1[2], $timeb, $calibrate, 7);

            /* Section 8, Standard functions */
            $x = 0.75;
            $timea = microtime(true);
            {
                for ($ix = 0; $ix < $xtra; $ix++)
                {
                    for ($i = 0; $i < $n8; $i++)
                    {
                        $x = (float)sqrt(exp(log($x) / $t1));
                    }
                }
            }
            $timeb = microtime(true) - $timea;
            $this->Pout("N8 exp,sqrt etc. ", (float)($n8 * 4) * (float)($xtra),
                                    2, $x, $timeb, $calibrate, 8);

        }

        private static function Pa(array $e, $t, $t2)
        {
            $j;
            for ($j = 0; $j < 6; $j++)
            {
                $e[0] = ($e[0] + $e[1] + $e[2] - $e[3]) * $t;
                $e[1] = ($e[0] + $e[1] - $e[2] + $e[3]) * $t;
                $e[2] = ($e[0] - $e[1] + $e[2] + $e[3]) * $t;
                $e[3] = (-$e[0] + $e[1] + $e[2] + $e[3]) / $t2;
            }

            return;
        }

        private static function Po(array $e1, $j, $k, $l)
        {
            $e1[$j] = $e1[$k];
            $e1[$k] = $e1[$l];
            $e1[$l] = $e1[$j];
            return;
        }

        private static function P3(&$x, &$y, &$z, $t, $t1, $t2)
        {
            $x = $y;
            $y = $z;
            $x = $t * ($x + $y);
            $y = $t1 * ($x + $y);
            $z = ($x + $y) / $t2;
            return;
        }

        private function Pout($title, $ops, $type, $checknum,
              $time, $calibrate, $section)
        {
            $mops = 0;
            $mflops = 0.0;

            $this->Check = $this->Check + $checknum;
            $this->loop_time[$section] = $time;
            $this->headings[$section] = $title;
            $this->TimeUsed = $this->TimeUsed + $time;
            if ($calibrate == 1) {
                $this->results[$section] = $checknum;
            }
            if ($calibrate == 0)
            {
                $this->Write("%-18s %24.17f    ", $this->headings[$section], $this->results[$section]);

                if ($type == 1)
                {
                    if ($time > 0)
                    {
                        $mflops = $ops / (1000000 * (float)$time);
                    }
                    else
                    {
                        $mflops = 0;
                    }
                    $this->loop_mops[$section] = 99999;
                    $this->loop_mflops[$section] = $mflops;
                    $this->WriteLine("%9.3f           %9.3f",
                    $this->loop_mflops[$section], ($this->loop_time[$section]));
                }
                else
                {
                    if ($time > 0)
                    {
                        $mops = $ops / (1000000 * (float)$time);
                    }
                    else
                    {
                        $mops = 0;
                    }
                    $this->loop_mops[$section] = $mops;
                    $this->loop_mflops[$section] = 0;
                    $this->WriteLine("           %9.3f%9.3f",
                    $this->loop_mops[$section], ($this->loop_time[$section]));
                }
            }

            return;
        }

        private function WriteLine($text, ...$args)
        {
            $this->output->WriteLine($text, ...$args);
        }

        private function Write($text, ...$args)
        {
            $this->output->Write($text, ...$args);
        }
    }

    $w = new Whetstone(true);
    $w->bench(false);
}