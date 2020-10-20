<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;
    
    abstract class MathBase  extends BenchmarkBase {
        public function __construct($writer, $printToConsole)
        {
            parent::__construct($writer, $printToConsole);
            $this->Iterrations = 200000000;
            $this->Ratio = 3.5;
        }

        public static function DoMath($i)
        {
            $rev = 1.0 / ($i + 1.0);
            return abs($i) + acos($rev) + asin($rev) + atan($rev) +
                floor($i) + exp($rev) + cos($i) + sin($i) + M_PI + sqrt($i);
        }
    }
}