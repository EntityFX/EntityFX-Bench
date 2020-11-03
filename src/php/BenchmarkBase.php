<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;

if (!defined("ASPECT_RATIO")) {
    define("ASPECT_RATIO", 0.2);
}

if (!defined("DEBUG_ASPECT_RATIO")) {
    define("DEBUG_ASPECT_RATIO", 0.05);
}


if (defined("DEBUG")) {
    if (DEBUG) {
        define("BENCH_ASPECT_RATIO", DEBUG_ASPECT_RATIO);
    } else {
        define("BENCH_ASPECT_RATIO", ASPECT_RATIO);
    }
} else {
    define("BENCH_ASPECT_RATIO", ASPECT_RATIO);
}


    
    abstract class BenchmarkBase {
        protected $Iterrations = 0;

        protected $printToConsole = true;

        public static $AspectRatio = BENCH_ASPECT_RATIO;
    
        public $Ratio = 1.0;
    
        public $Name = "";

        protected $output;

        public function __construct($writer, $printToConsole) {
            $this->printToConsole = ($printToConsole === null) ? true : $printToConsole;
            $this->Iterrations = 1;
            $this->Radio = 1.0;
            $this->Name = (new \ReflectionClass($this))->getShortName();
            $this->output = $writer;
        }

        public function Bench() {
            $this->BeforeBench();
            $start = microtime(true);
            $res = $this->BenchImplementation();
            $elapsed = microtime(true) - $start;
            $result = $this->PopulateResult($this->BuildResult($elapsed), $res);
            if ($result["Output"]) {
                file_put_contents("$this->Name.log", $result["Output"], FILE_APPEND);
            }
            $this->AfterBench($result);
            return $result;
        }

        public function BenchInParallel($buildFunc, $benchFunc, $setBenchResultFunc) {
            $results = [];

            return $results;
        }

        protected function BeforeBench() {
        }

        protected function AfterBench() {
        }

        public function Warmup($aspect = 0.05) {
            $aspect = $aspect != null ? $aspect : 0.05;

            $this->Iterrations *= self::$AspectRatio;

            $tmp = $this->Iterrations;
            $this->Iterrations = $this->Iterrations * $aspect;
            $this->UseConsole(DEBUG);
            $this->Bench();
            $this->UseConsole(true);
            $this->Iterrations = $tmp;
        }
        
        public function UseConsole($printToConsole) {
            $this->printToConsole = $printToConsole != null ? $printToConsole : true;
            $this->output->UseConsole = $printToConsole;
        }
        
        protected function BenchImplementation() {
        }

        public static function getCores() {
            return (int) ((substr(PHP_OS, 0, 3) === 'WIN')?(getenv("NUMBER_OF_PROCESSORS")+0):substr_count(file_get_contents("/proc/cpuinfo"),"processor"));
        }
        
        
        protected function BuildResult($elapsed)
        {
            return [
                "BenchmarkName" => $this->Name, 
                "Elapsed" => $elapsed * 1000,
                "Points" => $this->Iterrations / ($elapsed * 1000) * $this->Ratio,
                "Result" => $this->Iterrations / ($elapsed * 1000),
                "Units" => "Iter/s",
                "Iterrations" => $this->Iterrations,
                "Ratio" => $this->Ratio,
                "Output" => ""
            ];
        }
        
        protected function PopulateResult($benchResult, $benchValue)
        {
            return $benchResult;
        }
    }
}