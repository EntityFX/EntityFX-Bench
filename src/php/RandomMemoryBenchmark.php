<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;
    
    class RandomMemoryBenchmark extends RandomMemoryBenchmarkBase {

        protected function BenchImplementation() {
            return $this->BenchRandomMemory();
        }

        protected function PopulateResult($benchResult, $result) {
            $benchResult["Points"] = $result["Average"] * $this->Ratio;
            $benchResult["Result"] = $result["Average"];
            $benchResult["Units"] = "MB/s";
            $benchResult["Output"] = $result["Output"];
            return $benchResult;
        }
    }
}