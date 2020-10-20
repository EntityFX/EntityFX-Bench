<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;
    
    abstract class StringManipulationBase  extends BenchmarkBase {
        public function __construct($writer, $printToConsole)
        {
            parent::__construct($writer, $printToConsole);
            $this->Iterrations = 5000000;
            $this->Ratio = 25;
        }

        public static function DoStringManipilation($str)
        {
            return str_replace("aaa", ".", strtolower(
                    strtoupper(
                        str_replace("/", "_", implode("/", explode(' ', $str)))
                    ) . "AAA"
                )
            );
        }
    }
}