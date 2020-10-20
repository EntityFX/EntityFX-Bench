<?php

define("DEBUG", false);
define("ASPECT_RATIO", 0.2);
define("DEBUG_ASPECT_RATIO", 0.05);

require_once("Writer.php");
require_once("BenchmarkBase.php");
require_once("Generic/ArithmeticsBase.php");
require_once("Generic/ArithmeticsBenchmark.php");
require_once("Generic/MathBase.php");
require_once("Generic/MathBenchmark.php");
require_once("Generic/CallBenchmark.php");
require_once("Generic/IfElseBenchmark.php");
require_once("Generic/StringManipulationBase.php");
require_once("Generic/StringManipulation.php");
require_once("Generic/MemoryBenchmarkBase.php");
require_once("Generic/MemoryBenchmark.php");
require_once("Generic/RandomMemoryBenchmarkBase.php");
require_once("Generic/RandomMemoryBenchmark.php");
require_once("Generic/HashBase.php");
require_once("Generic/HashBenchmark.php");
require_once("Dhrystone/Dhrystone2.php");
require_once("Dhrystone/DhrystoneBenchmark.php");
require_once("Whetstone/Whetstone.php");
require_once("Whetstone/WhetstoneBenchmark.php");
require_once("Scimark2/Scimark2.php");
require_once("Scimark2/Scimark2Benchmark.php");

use EntityFX\NetBenchmark\Core\Writer;
use EntityFX\NetBenchmark\Core\Generic\BenchmarkBase;
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
use EntityFX\NetBenchmark\Core\Scimark2\Scimark2Benchmark;

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


$writer = new Writer("Output.log");

$benchmarks = [
    new MemoryBenchmark($writer, true),
    new RandomMemoryBenchmark($writer, true),
    new Scimark2Benchmark($writer, true),
    new DhrystoneBenchmark($writer, true),
    new WhetstoneBenchmark($writer, true),
    new ArithmeticsBenchmark($writer, true),
    new MathBenchmark($writer, true),
    new CallBenchmark($writer, true),
    new IfElseBenchmark($writer, true),
    new StringManipulation($writer, true),
    new HashBenchmark($writer, true)
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
$points = [];
$i = 1;
$result = [];

foreach ($benchmarks as $key => $bench) {
    $writer->WriteHeader("[$i] {$bench->Name}");
    $r = $bench->Bench();
    $total += $r["Elapsed"];
    $totalPoints += $r["Points"];
    $points[] = sprintf("%.2f", $r["Points"]);
    writeResult($writer, $r);

    $result[] = $r;
    $i++;
}

$writer->WriteNewLine();
$writer->WriteTitle("%-30s", "Total:");
$writer->WriteValue("%13.2f ms", $total);
$writer->WriteValue("%13.2f pts", $totalPoints);
$writer->WriteNewLine();

$os = php_uname('s') . " " . php_uname('r');
$cores = BenchmarkBase::getCores();
$version = "PHP " . phpversion();
$mem = memory_get_usage();

$pointsString = implode(";", $points);

$writer->WriteNewLine();
$writer->WriteTitle("$os;$version;$cores;$mem;");
$writer->WriteValue($pointsString);
$writer->WriteLine(";%.2f", $total);