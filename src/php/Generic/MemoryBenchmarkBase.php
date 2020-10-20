<?php

namespace EntityFX\NetBenchmark\Core\Generic {

    use EntityFX\NetBenchmark\Core\Writer;
    
    abstract class MemoryBenchmarkBase  extends BenchmarkBase {

        public function __construct($writer, $printToConsole)
        {
            parent::__construct($writer, $printToConsole);
            $this->Iterrations = 500000;
            $this->Ratio = 50;
        }

        public function Warmup($aspect = 0.05) {
            $this->UseConsole(DEBUG);
            parent::Warmup($aspect);
            $this->UseConsole(true);
        }

        protected function BenchRandomMemory($aspect = 0.05) {
            $int4k = $this->MeasureArraySpeed(1024);
            $this->output->Write("int 4k: %.2f MB/s", $int4k["MbPerSec"]);
            $this->output->WriteNewLine();
            $int512k = $this->MeasureArraySpeed(131072);
            $this->output->Write("int 512k: %.2f MB/s", $int512k["MbPerSec"]);
            $this->output->WriteNewLine();
            $int8m = $this->MeasureArraySpeed(2097152);
            $this->output->Write("int 8M: %.2f MB/s", $int8m["MbPerSec"]);
            $this->output->WriteNewLine();
            $int32m = $this->MeasureArraySpeed(32 * 1024 * 1024 / 8);
            $this->output->Write("int 32M: %.2f MB/s", $int32m["MbPerSec"]);
            $this->output->WriteNewLine();

            $long4k = $this->MeasureArraySpeedLong(512);
            $this->output->Write("long 4k: %.2f MB/s", $long4k["MbPerSec"]);
            $this->output->WriteNewLine();
            $long512k = $this->MeasureArraySpeedLong(65536);
            $this->output->Write("long 512k: %.2f MB/s", $long512k["MbPerSec"]);
            $this->output->WriteNewLine();
            $long8m = $this->MeasureArraySpeedLong(1048576);
            $this->output->Write("long 8M: %.2f MB/s", $long8m["MbPerSec"]);
            $this->output->WriteNewLine();
            $long32m = $this->MeasureArraySpeedLong(32 * 1024 * 1024 / 8);
            $this->output->Write("long 32M: %.2f MB/s", $long32m["MbPerSec"]);
            $this->output->WriteNewLine();

            $results = [
                $int4k["MbPerSec"],  $int512k["MbPerSec"],   $int8m["MbPerSec"], $int32m["MbPerSec"] ,
                $long4k["MbPerSec"], $long512k["MbPerSec"],  $long8m["MbPerSec"], $long32m["MbPerSec"] ];
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
            $blockSize = 8;
            $L = [];

            $array = [];
            
            for ($i = 0; $i < $size; $i++) {
                $array[$i] = mt_rand(-2147483648, 2147483647);
            }

            $end = count($array) - 1;
            $k0 = intdiv($size, 1024); 
            $k1 = $k0 == 0 ? 1 : $k0 ;
            $iterInternal = intdiv($this->Iterrations, $k1);
            $iterInternal = $iterInternal == 0 ? 1: $iterInternal;
            for ($idx = 0; $idx < $end; $idx+=$blockSize)
            {
                $L[0] = $array[$idx];
                $L[1] = $array[$idx+1];
                $L[2] = $array[$idx+2];
                $L[3] = $array[$idx+3];
                $L[4] = $array[$idx+4];
                $L[5] = $array[$idx+5];
                $L[6] = $array[$idx+6];
                $L[7] = $array[$idx+7];
            }
            $start = microtime(true);
            for ($i = 0; $i < $iterInternal; $i++)
            {
                for ($idx = 0; $idx < $end; $idx+=$blockSize)
                {
                    $L[0] = $array[$idx];
                    $L[1] = $array[$idx+1];
                    $L[2] = $array[$idx+2];
                    $L[3] = $array[$idx+3];
                    $L[4] = $array[$idx+4];
                    $L[5] = $array[$idx+5];
                    $L[6] = $array[$idx+6];
                    $L[7] = $array[$idx+7];
                }
            }
            $elapsed = microtime(true) - $start;
            return [
                "MbPerSec" => $iterInternal * count($array) * PHP_INT_SIZE / $elapsed / 1024 / 1024,
                "Data" => $L
            ];
        }

        protected function MeasureArraySpeedLong($size)
        {
            $blockSize = 16;
            $L = [];

            $array = [];
            
            for ($i = 0; $i < $size; $i++) {
                $array[$i] = mt_rand(-2147483648, 2147483647);
            }

            $end = count($array) - 1;
            $k0 = intdiv($size, 1024); 
            $k1 = $k0 == 0 ? 1 : $k0 ;
            $iterInternal = intdiv($this->Iterrations, $k1);
            $iterInternal = $iterInternal == 0 ? 1: $iterInternal;
            for ($idx = 0; $idx < $end; $idx+=$blockSize)
            {
                $L[0] = $array[$idx];
                $L[1] = $array[$idx+1];
                $L[2] = $array[$idx+2];
                $L[3] = $array[$idx+3];
                $L[4] = $array[$idx+4];
                $L[5] = $array[$idx+5];
                $L[6] = $array[$idx+6];
                $L[7] = $array[$idx+7];
                $L[8] = $array[$idx+8];
                $L[9] = $array[$idx+9];
                $L[0xA] = $array[$idx+0xA];
                $L[0xB] = $array[$idx+0xB];
                $L[0xC] = $array[$idx+0xC];
                $L[0xD] = $array[$idx+0xD];
                $L[0xE] = $array[$idx+0xE];
                $L[0xF] = $array[$idx+0xF];
            }
            $start = microtime(true);
            for ($i = 0; $i < $iterInternal; $i++)
            {
                for ($idx = 0; $idx < $end; $idx+=$blockSize)
                {
                    $L[0] = $array[$idx];
                    $L[1] = $array[$idx+1];
                    $L[2] = $array[$idx+2];
                    $L[3] = $array[$idx+3];
                    $L[4] = $array[$idx+4];
                    $L[5] = $array[$idx+5];
                    $L[6] = $array[$idx+6];
                    $L[7] = $array[$idx+7];
                    $L[8] = $array[$idx+8];
                    $L[9] = $array[$idx+9];
                    $L[0xA] = $array[$idx+0xA];
                    $L[0xB] = $array[$idx+0xB];
                    $L[0xC] = $array[$idx+0xC];
                    $L[0xD] = $array[$idx+0xD];
                    $L[0xE] = $array[$idx+0xE];
                    $L[0xF] = $array[$idx+0xF];
                }
            }
            $elapsed = microtime(true) - $start;
            return [
                "MbPerSec" => $iterInternal * count($array) * PHP_INT_SIZE / $elapsed / 1024 / 1024,
                "Data" => $L
            ];
        }
    }
}