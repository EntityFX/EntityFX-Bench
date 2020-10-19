<?php

define("DEBUG", true);

require_once("Writer.php");
require_once("BenchmarkBase.php");
require_once("ArithmeticsBase.php");
require_once("ArithmeticsBenchmark.php");
require_once("MathBase.php");
require_once("MathBenchmark.php");
require_once("CallBenchmark.php");
require_once("IfElseBenchmark.php");
require_once("StringManipulationBase.php");
require_once("StringManipulation.php");
require_once("MemoryBenchmarkBase.php");
require_once("MemoryBenchmark.php");
require_once("RandomMemoryBenchmarkBase.php");
require_once("RandomMemoryBenchmark.php");
require_once("Dhrystone2.php");
require_once("DhrystoneBenchmark.php");
require_once("Whetstone.php");
require_once("WhetstoneBenchmark.php");
require_once("HashBase.php");
require_once("HashBenchmark.php");
require_once("Scimark2/Scimark2.php");

use EntityFX\NetBenchmark\Core\Writer;
use EntityFX\NetBenchmark\Core\Generic\ArithmeticsBenchmark;
use EntityFX\NetBenchmark\Core\Generic\MathBenchmark;
use EntityFX\NetBenchmark\Core\Generic\CallBenchmark;
use EntityFX\NetBenchmark\Core\Generic\IfElseBenchmark;
use EntityFX\NetBenchmark\Core\Generic\StringManipulation;
use EntityFX\NetBenchmark\Core\Generic\MemoryBenchmark;
use EntityFX\NetBenchmark\Core\Generic\RandomMemoryBenchmark;
use EntityFX\NetBenchmark\Core\Generic\HashBenchmark;
use EntityFX\NetBenchmark\Core\Dhrystone\DhrystoneBenchmark;
use EntityFX\NetBenchmark\Core\Whetstone\WhetstoneBenchmark;
use EntityFX\NetBenchmark\Core\Scimark2\Scimark2;

function writeResult($writer, $benchResult)
{
    $writer->WriteTitle("%-30s", $benchResult["BenchmarkName"]);
    $writer->WriteValue("%13.2f ms", $benchResult["Elapsed"]);
    $writer->WriteValue("%13.2f pts", $benchResult["Points"]);
    if ($benchResult["Result"] != "") {
        $writer->WriteValue("%13.2f %s", $benchResult["Result"], $benchResult["Units"]);
    }
    $writer->WriteNewLine();
}


$writer = new Writer();

$s = new Scimark2(true);
$s->Bench();

$benchmarks = [
    new HashBenchmark($writer, true),
    new MemoryBenchmark($writer, true),
    new RandomMemoryBenchmark($writer, true),
    new DhrystoneBenchmark($writer, true),
    new WhetstoneBenchmark($writer, true),
    new ArithmeticsBenchmark($writer, true),
    new MathBenchmark($writer, true),
    new CallBenchmark($writer, true),
    new IfElseBenchmark($writer, true),
    new StringManipulation($writer, true)
];

$writer->WriteHeader("Warmup");
foreach ($benchmarks as $key => $bench) {
    $bench->Warmup();
    $writer->Write(".");
}

$writer->WriteNewLine();
$writer->WriteHeader("Bench");

$total = 0;
$totalPoints = 0;
$i = 1;
$result = [];

foreach ($benchmarks as $key => $bench) {
    $writer->WriteHeader("[$i] {$bench->Name}");
    $r = $bench->Bench();
    $total += $r["Elapsed"];
    $totalPoints += $r["Points"];
    writeResult($writer, $r);

    $result[] = $r;
    $i++;
}

