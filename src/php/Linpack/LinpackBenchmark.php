<?php

namespace EntityFX\NetBenchmark\Core\Linpack {

    use EntityFX\NetBenchmark\Core\Writer;
    use EntityFX\NetBenchmark\Core\Generic\BenchmarkBase;

    class LinpackBenchmark extends BenchmarkBase {

        private $linpack;

        public function __construct($writer, $printToConsole)
        {
            parent::__construct($writer, $printToConsole);
            $this->Ratio = 10;
            $this->linpack = new Linpack($printToConsole);
        }

        public function Warmup($aspect = 0.05) {
        }

        protected function BenchImplementation() {
            return $this->linpack->run_benchmark(2000);
        }

        protected function PopulateResult($benchResult, $result) {
            $benchResult["Points"] = $result["MFLOPS"] * $this->Ratio;
            $benchResult["Result"] = $result["MFLOPS"];
            $benchResult["Units"] = "MFLOPS";
            $benchResult["Output"] = $result["Output"];
            return $benchResult;
        }
    }
}