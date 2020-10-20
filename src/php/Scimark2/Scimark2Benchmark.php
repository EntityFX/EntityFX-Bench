<?php

namespace EntityFX\NetBenchmark\Core\Scimark2 {

    use EntityFX\NetBenchmark\Core\Writer;
    use EntityFX\NetBenchmark\Core\Generic\BenchmarkBase;

    class Scimark2Benchmark extends BenchmarkBase {

        private $dhrystone;

        public function __construct($writer, $printToConsole)
        {
            parent::__construct($writer, $printToConsole);
            $this->Ratio = 300;
            $this->dhrystone = new Scimark2($printToConsole);
        }

        public function Warmup($aspect = 0.05) {
        }

        protected function BenchImplementation() {
            return $this->dhrystone->bench();
        }

        protected function PopulateResult($benchResult, $result) {
            $benchResult["Points"] = $result["CompositeScore"] * $this->Ratio;
            $benchResult["Result"] = $result["CompositeScore"];
            $benchResult["Units"] = "CompositeScore";
            $benchResult["Output"] = $result["Output"];
            return $benchResult;
        }
    }
}