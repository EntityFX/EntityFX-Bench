<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;
    
    abstract class ArithmeticsBase  extends BenchmarkBase {
        protected static $R = 0.0;

        public function __construct($writer, $printToConsole)
        {
            parent::__construct($writer, $printToConsole);
            $this->Iterrations = 300000000;
            $this->Ratio = 2;
        }

        public static function DoArithmetics($i)
        {
            return ($i / 10) * ($i / 100) * ($i / 100) * ($i / 100) * 1.11 + ($i / 100) * ($i / 1000) * ($i / 1000) * 2.22 - $i * ($i / 10000) * 3.33 + $i * 5.33;
        }
    }
}