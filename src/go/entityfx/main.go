package main

import (
	"fmt"
	"runtime"

	"./dhrystone"
	"./generic"
	"./linpack"
	"./scimark2"
	"./utils"
	"./whetstone"
)

func writeResult(writer utils.WriterType, benchResult *generic.BenchResult) {
	writer.WriteTitle("%-30s", benchResult.BenchmarkName)
	writer.WriteValue("%15d ms", benchResult.Elapsed)
	writer.WriteValue("%13.2f pts", benchResult.Points)
	writer.WriteValue("%15.2f %s", benchResult.Result, benchResult.Units)
	writer.WriteNewLine()
	writer.WriteValue("Iterrations: %15d, Ratio: %15f", benchResult.Iterrations, benchResult.Ratio)
	writer.WriteNewLine()
}

func main() {
	var writer utils.WriterType = utils.NewWriter("Output.log")

	var benchmarks = [...]generic.BenchmarkInterface{
		generic.NewArithmetics(writer, true),
		generic.NewParallelArithmetics(writer, true),
		generic.NewMathBenchmark(writer, true),
		generic.NewParallelMathBenchmark(writer, true),
		generic.NewCallBenchmark(writer, true),
		generic.NewParallelCallBenchmark(writer, true),
		generic.NewIfElseBenchmark(writer, true),
		generic.NewParallelIfElseBenchmark(writer, true),
		generic.NewStringManipulation(writer, true),
		generic.NewParallelStringManipulation(writer, true),
		generic.NewMemoryBenchmark(writer, true),
		generic.NewParallelMemoryBenchmark(writer, true),
		generic.NewRandomMemoryBenchmark(writer, true),
		generic.NewParallelRandomMemoryBenchmark(writer, true),
		scimark2.NewScimark2Benchmark(writer, true),
		scimark2.NewParallelScimark2Benchmark(writer, true),
		dhrystone.NewDhrystoneBenchmark(writer, true),
		dhrystone.NewParallelDhrystoneBenchmark(writer, true),
		whetstone.NewWhetstoneBenchmark(writer, true),
		whetstone.NewParallelWhetstoneBenchmark(writer, true),
		linpack.NewLinpackBenchmark(writer, true),
		linpack.NewParallelLinpackBenchmark(writer, true),
		generic.NewHashBenchmark(writer, true),
		generic.NewParallelHashBenchmark(writer, true),
	}

	writer.WriteHeader("Warmup")

	for _, benchmark := range benchmarks {
		generic.Warmup(benchmark, 0.05)
		writer.Write(".")
	}

	writer.WriteNewLine()
	writer.WriteHeader("Bench")

	var total int64 = 0
	var totalPoints float64 = 0.0

	var points [len(benchmarks)]string
	var result [len(benchmarks)]*generic.BenchResult

	for i, benchmark := range benchmarks {
		writer.WriteHeader("[%d] %s", i+1, benchmark.GetName())
		result[i] = generic.Bench(benchmark)
		total += result[i].Elapsed
		totalPoints += result[i].Points
		points[i] = fmt.Sprintf("%.2f", result[i].Points)

		writeResult(writer, result[i])
	}

	writer.WriteNewLine()
	writer.WriteTitle("%-30s", "Total:")
	writer.WriteValue("%15d ms", total)
	writer.WriteValue("%13.2f pts", totalPoints)
	writer.WriteNewLine()

	var m runtime.MemStats
	runtime.ReadMemStats(&m)

	osVersion := runtime.GOOS + " " + runtime.GOARCH
	environmentVersion := runtime.Version()
	threadsCount := runtime.NumCPU()
	memory := m.TotalAlloc

	headerCommon := "Operating System,Runtime,Threads Count,Memory Used"
	headerTotals := ",Total Points,Total Time (ms)"

	writer.WriteNewLine()
	writer.WriteHeader("Single-thread results")
	writer.WriteTitle(headerCommon)

	for _, r := range result {
		if r.IsParallel {
			continue
		}
		writer.WriteTitle(",%s", r.BenchmarkName)
	}
	writer.WriteTitle(headerTotals)
	writer.WriteNewLine()
	writer.WriteTitle("%s,%s,%d,%d", osVersion, environmentVersion, threadsCount, memory)
	for _, r := range result {
		if r.IsParallel {
			continue
		}
		writer.WriteValue(",%.2f", r.Points)
	}
	writer.WriteTitle(",%.2f,%d", totalPoints, total)
	writer.WriteNewLine()

	writer.WriteNewLine()
	writer.WriteHeader("All results")
	writer.WriteTitle(headerCommon)
	for _, r := range result {
		writer.WriteTitle(",%s", r.BenchmarkName)
	}
	writer.WriteTitle(headerTotals)
	writer.WriteNewLine()
	writer.WriteTitle("%s,%s,%d,%d", osVersion, environmentVersion, threadsCount, memory)
	for _, r := range result {
		writer.WriteValue(";%.2f", r.Points)
	}
	writer.WriteTitle(";%.2f;%d", totalPoints, total)
	writer.WriteNewLine()

	writer.WriteNewLine()
	writer.WriteHeader("Single-thread  Units results")
	writer.WriteTitle(headerCommon)
	for _, r := range result {
		writer.WriteTitle(",%s (%s)", r.BenchmarkName, r.Units)
	}
	writer.WriteTitle(headerTotals)
	writer.WriteNewLine()
	writer.WriteTitle("%s,%s,%d,%d", osVersion, environmentVersion, threadsCount, memory)
	for _, r := range result {
		writer.WriteValue(",%.2f", r.Result)
	}
	writer.WriteTitle(";%.2f;%d", totalPoints, total)
	writer.WriteNewLine()

	writer.WriteNewLine()
	writer.WriteHeader("All  Units results")
	writer.WriteTitle(headerCommon)
	for _, r := range result {
		writer.WriteTitle(",%s (%s)", r.BenchmarkName, r.Units)
	}
	writer.WriteTitle(headerTotals)
	writer.WriteNewLine()
	writer.WriteTitle("%s,%s,%d,%d", osVersion, environmentVersion, threadsCount, memory)
	for _, r := range result {
		writer.WriteValue(",%.2f", r.Result)
	}
	writer.WriteTitle(",%.2f,%d", totalPoints, total)
	writer.WriteNewLine()
}
