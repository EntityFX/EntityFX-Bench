importScripts("utils.js");
importScripts("flops.js");
importScripts("benchmarkBase.js");
importScripts("flopsBenchmark.js");

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
	output.writeTitle("%s,%s,%d,%d,".$(navigator.platform, navigator.os, 0, 0));
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
	output.writeTitle("%s,%s,%d,%d,".$(navigator.platform, navigator.os, 0, 0));
	var pointsCsv = results.map(function(value) { return "%.2f".$(value.Result); }).join(',');
	output.writeValue(pointsCsv);
	output.writeTitle(",%.2f,%d".$(totalPoints, total));
	output.writeLine();
}

var output = new WorkerWriter();

var benchMarks = [
	new FlopsBenchmark(output),
];

Warmup(benchMarks, output);
Bench(benchMarks, output);