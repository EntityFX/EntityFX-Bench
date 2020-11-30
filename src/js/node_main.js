var utils = require('./utils.js');
global.Writer = utils.Writer;
global.getTime = utils.getTime;
global.__extends = utils.__extends;

var benchmarkBase = require('./benchmarkBase.js');
global.BenchmarkBase = benchmarkBase.BenchmarkBase;

var arithmeticsBase = require('./arithmeticsBase.js');
global.ArithmeticsBase = arithmeticsBase.ArithmeticsBase;
var arithmetics = require('./arithmetics.js');

var mathBase = require('./mathBase.js');
global.MathBase = mathBase.MathBase;
var mathBenchmark = require('./mathBenchmark.js');

var callBenchmark = require('./callBenchmark.js');

var ifElseBenchmark = require('./ifElseBenchmark.js');

var stringManipulationBase = require('./stringManipulationBase.js');
global.StringManipulationBase = stringManipulationBase.StringManipulationBase;
var stringManipulation = require('./stringManipulation.js');

var memoryBenchmarkBase = require('./memoryBenchmarkBase.js');
global.MemoryBenchmarkBase = memoryBenchmarkBase.MemoryBenchmarkBase;
var memoryBenchmark = require('./memoryBenchmark.js');

var randomMemoryBenchmarkBase = require('./randomMemoryBenchmarkBase.js');
global.RandomMemoryBenchmarkBase = randomMemoryBenchmarkBase.RandomMemoryBenchmarkBase;
var randomMemoryBenchmark = require('./randomMemoryBenchmark.js');

var scimark2 = require('./scimark2.js');
global.Scimark2 = scimark2.Scimark2;
var scimark2Benchmark = require('./scimark2Benchmark.js');

var dhrystone = require('./dhrystone.js');
global.Dhrystone = dhrystone.Dhrystone;
var dhrystoneBenchmark = require('./dhrystoneBenchmark.js');

var whetstone = require('./whetstone.js');
global.Whetstone = whetstone.Whetstone;
var whetstoneBenchmark = require('./whetstoneBenchmark.js');

var linpack = require('./linpack.js');
global.Linpack = linpack.Linpack;
var linpackBenchmark = require('./linpackBenchmark.js');


require("./crypto-js/core.js");
var SHA1 = require("./crypto-js/sha1.js");
var SHA256 = require("./crypto-js/sha256.js");
global.CryptoJS = {
    SHA1 : SHA1,
    SHA256 : SHA256
};

var hashBase = require('./hashBase.js');
global.HashBase = hashBase.HashBase;
var hashBenchmark = require('./hashBenchmark.js');
const { platform } = require('os');

function writeResult(benchResult, output) {
	output.writeTitle("%-30s".$(benchResult.BenchmarkName));
	output.writeValue("%15d ms".$(benchResult.Elapsed));
	output.writeValue("%13.2f pts".$(benchResult.Points));
	output.writeValue("%15.2f %s".$(benchResult.Result, benchResult.Units));
	output.writeLine();
	output.writeValue("Iterrations: %15d, Ratio: %15.3f".$(benchResult.Iterrations, benchResult.Ratio));
	output.writeLine();
}

function Warmup(benchMarks, output) {
	output.writeHeader("Warmup");
	
	for (var index = 0; index < benchMarks.length; index++) {
		benchMarks[index].Warmup();
		output.write(".");
	}
	output.writeLine();
}

function BenchOne(benchMark, output) {
	var r = benchMark.Bench();
	writeResult(r, output);
	return r;
}

function Bench(benchMarks, output) {
	var total = 0;
	var totalPoints = 0;
	var results = [];
	var i = 0;
	
	for (var index = 0; index < benchMarks.length; index++) {
		output.writeHeader("[%d] %s".$(index + 1, benchMarks[index].Name));
		var r = BenchOne(benchMarks[index], output);
		
		total += r.Elapsed;
		totalPoints += r.Points;

		results.push(r);
	}

	output.writeLine();
	output.writeTitle("%-30s".$("Total:"));
	output.writeValue("%15d ms".$(total));
	output.writeValue("%13.2f pts".$(totalPoints));
	
	var headerCommon = "Operating System,Runtime,Threads Count,Memory Used,";
	var headerTotals = ",Total Points,Total Time (ms)";

	var namesCsv = results.map(function(value) { return value.BenchmarkName; }).join(',');

	output.writeLine();
	output.writeHeader("Single-thread results");
	output.writeTitle(headerCommon);
	output.writeTitle(namesCsv);
	output.writeTitle(headerTotals);
	output.writeLine();
	output.writeTitle("%s,%s,%d,%d,".$(process.version, process.platform + " " + process.arch, 0, process.memoryUsage().rss));
	var pointsCsv = results.map(function(value) { return "%.2f".$(value.Points); }).join(',');
	output.writeValue(pointsCsv);
	output.writeTitle(",%.2f,%d".$(totalPoints, total));
	output.writeLine();

	output.writeLine();
	output.writeHeader("Single-thread Units results");
	output.writeTitle(headerCommon);
	output.writeTitle(namesCsv);
	output.writeTitle(headerTotals);
	output.writeLine();
	output.writeTitle("%s,%s,%d,%d,".$(process.version, process.platform + " " + process.arch, 0, process.memoryUsage().rss));
	var pointsCsv = results.map(function(value) { return "%.2f".$(value.Result); }).join(',');
	output.writeValue(pointsCsv);
	output.writeTitle(",%.2f,%d".$(totalPoints, total));
	output.writeLine();
}

var output = new utils.Writer();

var benchMarks = [
	new arithmetics.ArithemticsBenchmark(output),
	new mathBenchmark.MathBenchmark(output),
	new callBenchmark.CallBenchmark(output),
	new ifElseBenchmark.IfElseBenchmark(output),
	new stringManipulation.StringManipulation(output),
	new memoryBenchmark.MemoryBenchmark(output),
	new randomMemoryBenchmark.RandomMemoryBenchmark(output),
	new scimark2Benchmark.Scimark2Benchmark(output),
	new dhrystoneBenchmark.DhrystoneBenchmark(output),
	new whetstoneBenchmark.WhetstoneBenchmark(output),
	new linpackBenchmark.LinpackBenchmark(output),
	new hashBenchmark.HashBenchmark(output)
];

Warmup(benchMarks, output);
Bench(benchMarks, output);