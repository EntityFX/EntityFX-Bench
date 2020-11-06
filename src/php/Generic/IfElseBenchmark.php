<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;
    
    class IfElseBenchmark extends BenchmarkBase {

        public function __construct($writer, $printToConsole)
        {
            parent::__construct($writer, $printToConsole);
            $this->Iterrations = 2000000000;
            $this->Ratio = 0.01;
        }


        protected function BenchImplementation()
        {
            $d = 0;

            for ($i = 0, $c = -1; $i < $this->Iterrations; $i++, $c--)
            {
                $c = $c === -4 ? -1 : $c;
                if ($i === -1)
                {
                    $d = 3;
                }
                else if ($i === -2)
                {
                    $d = 2;
                }
                else if ($i === -3)
                {
                    $d = 1;
                }
                $d = $d + 1;
            }
            return $d;
        }
    }
}