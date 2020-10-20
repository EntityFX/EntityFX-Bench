<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;

if (!defined("DEBUG_ASPECT_RATIO")) {
    define("DEBUG_ASPECT_RATIO", 0.1);
}
    
    abstract class BenchmarkBase {
        protected $Iterrations = 0;

        protected $printToConsole = true;

        public static $DebugAspectRatio = DEBUG_ASPECT_RATIO;
    
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

            if (DEBUG) {
                $this->Iterrations *= self::$DebugAspectRatio;
            }

            $tmp = $this->Iterrations;
            $this->Iterrations = $this->Iterrations * $aspect;
            $this->Bench();
            $this->Iterrations = $tmp;
        }
        
        public function UseConsole($printToConsole) {
            $this->printToConsole = $printToConsole != null ? $printToConsole : true;
            $this->output->UseConsole = $printToConsole;
        }
        
        protected function BenchImplementation() {
        }

        private static function getCores() {
            return (int) ((PHP_OS_FAMILY == 'Windows')?(getenv("NUMBER_OF_PROCESSORS")+0):substr_count(file_get_contents("/proc/cpuinfo"),"processor"));
        }
        
        
        protected function BuildResult($elapsed)
        {
            return [
                "BenchmarkName" => $this->Name, 
                "Elapsed" => $elapsed * 1000,
                "Points" => $this->Iterrations / ($elapsed * 1000) * $this->Ratio,
                "Result" => "",
                "Units" => ""
            ];
        }
        
        protected function PopulateResult($benchResult, $benchValue)
        {
            return $benchResult;
        }
    }
}