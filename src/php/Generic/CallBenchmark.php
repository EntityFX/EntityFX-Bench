<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;
    
    class CallBenchmark extends BenchmarkBase {

        public function __construct($writer, $printToConsole)
        {
            parent::__construct($writer, $printToConsole);
            $this->Iterrations = 2000000000;
            $this->Ratio = 0.5;
        }

        private static function DoCall($i)
        {
            return $i + 1;
        }

        protected function BenchImplementation()
        {
            $a = 0;

            for ($i = 0; $i < $this->Iterrations; $i++)
            {
                $a += self::DoCall($i);
            }
            return $a;
        }
    }
}