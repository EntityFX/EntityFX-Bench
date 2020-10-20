<?php

namespace EntityFX\NetBenchmark\Core\Whetstone {

    use EntityFX\NetBenchmark\Core\Writer;
    use EntityFX\NetBenchmark\Core\Generic\BenchmarkBase;

    class WhetstoneBenchmark extends BenchmarkBase {

        private $dhrystone;

        public function __construct($writer, $printToConsole)
        {
            parent::__construct($writer, $printToConsole);
            $this->Ratio = 20;
            $this->dhrystone = new Whetstone($printToConsole);
        }

        public function Warmup($aspect = 0.05) {
        }

        protected function BenchImplementation() {
            return $this->dhrystone->bench(false);
        }

        protected function PopulateResult($benchResult, $result) {
            $benchResult["Points"] = $result["MWIPS"] * $this->Ratio;
            $benchResult["Result"] = $result["MWIPS"];
            $benchResult["Units"] = "MWIPS";
            $benchResult["Output"] = $result["Output"];
            return $benchResult;
        }
    }
}