<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;
    
    abstract class RandomMemoryBenchmarkBase  extends BenchmarkBase {

        public function __construct($writer, $printToConsole)
        {
            parent::__construct($writer, $printToConsole);
            $this->Iterrations = 500000;
            $this->Ratio = 2;
        }

        public function Warmup($aspect = 0.05) {
            $this->UseConsole(DEBUG);
            parent::Warmup($aspect);
            $this->UseConsole(true);
        }

        protected function BenchRandomMemory($aspect = 0.05) {
            $int4k = $this->MeasureArraySpeed(1024);
            $this->output->Write("Random int 4k: %.2f MB/s", $int4k["MbPerSec"]);
            $this->output->WriteNewLine();
            $int512k = $this->MeasureArraySpeed(131072);
            $this->output->Write("Random int 512k: %.2f MB/s", $int512k["MbPerSec"]);
            $this->output->WriteNewLine();
            $int8m = $this->MeasureArraySpeed(2097152);
            $this->output->Write("Random int 8M: %.2f MB/s", $int8m["MbPerSec"]);
            $this->output->WriteNewLine();

            $long4k = $this->MeasureArraySpeed(512);
            $this->output->Write("Random long 4k: %.2f MB/s", $long4k["MbPerSec"]);
            $this->output->WriteNewLine();
            $long512k = $this->MeasureArraySpeed(65536);
            $this->output->Write("Random long 512k: %.2f MB/s", $long512k["MbPerSec"]);
            $this->output->WriteNewLine();
            $long8m = $this->MeasureArraySpeed(1048576);
            $this->output->Write("Random long 8M: %.2f MB/s", $long8m["MbPerSec"]);
            $this->output->WriteNewLine();

            $results = [
                $int4k["MbPerSec"],  $int512k["MbPerSec"],   $int8m["MbPerSec"], 
                $long4k["MbPerSec"], $long512k["MbPerSec"],  $long8m["MbPerSec"]
            ];
            $avg = array_sum($results) / count($results);
            $this->output->Write("Average: %.2f MB/s", $avg);
            $this->output->WriteNewLine();

            return [
                "Average" => $avg,
                "Output" => ""
            ];
        }

        protected function MeasureArraySpeed($size)
        {
            $I = 0;

            $array = [];
            for ($i = 0; $i < $size; $i++) {
                $array[$i] = mt_rand(-2147483648, 2147483647);
            }

            $indexes = [];
            for ($i = 0; $i < $size; $i++) {
                $indexes[$i] = mt_rand(0, $size - 1);
            }

            $end = count($array) - 1;
            $k0 = intdiv($size, 1024); 
            $k1 = $k0 == 0 ? 1 : $k0 ;
            $iterInternal = intdiv($this->Iterrations, $k1);
            $iterInternal = $iterInternal == 0 ? 1: $iterInternal;
            for ($idx = 0; $idx < $end; $idx++)
            {
                $I = $array[$idx];
            }
            $start = microtime(true);
            for ($i = 0; $i < $iterInternal; $i++)
            {
                for ($idx = 0; $idx < $end; $idx++)
                {
                    $I = $array[$indexes[$idx]];
                }
            }
            $elapsed = microtime(true) - $start;
            return [
                "MbPerSec" => $iterInternal * count($array) * PHP_INT_SIZE / $elapsed / 1024 / 1024,
                "Data" => $I
            ];
        }
    }
}