<?php

namespace EntityFX\NetBenchmark\Core\Dhrystone {

    use EntityFX\NetBenchmark\Core\Writer;
    use EntityFX\NetBenchmark\Core\Generic\BenchmarkBase;

    class DhrystoneBenchmark extends BenchmarkBase {

        private $dhrystone;

        public function __construct($writer, $printToConsole)
        {
            parent::__construct($writer, $printToConsole);
            $this->Ratio = 50;
            $this->dhrystone = new Dhrystone2($printToConsole);
        }

        public function Warmup($aspect = 0.05) {
        }

        protected function BenchImplementation() {
            return $this->dhrystone->bench();
        }

        protected function PopulateResult($benchResult, $result) {
            $benchResult["Points"] = $result["VaxMips"] * $this->Ratio;
            $benchResult["Result"] = $result["VaxMips"];
            $benchResult["Units"] = "DMIPS";
            $benchResult["Output"] = $result["Output"];
            return $benchResult;
        }
    }
}