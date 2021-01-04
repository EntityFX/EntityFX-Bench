package main

import "./generic"
import "./utils"
import "./linpack"
import "./dhrystone"
import "fmt"

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
	var writer utils.WriterType = utils.NewWriter("")

	var benchmarks = [...]generic.BenchmarkInterface{
		generic.NewArithmetics(writer, true),
		generic.NewMathBenchmark(writer, true),
		generic.NewCallBenchmark(writer, true),
		generic.NewIfElseBenchmark(writer, true),
		generic.NewStringManipulation(writer, true),
		generic.NewMemoryBenchmark(writer, true),
		generic.NewRandomMemoryBenchmark(writer, true),
		dhrystone.NewDhrystoneBenchmark(writer, true),
		linpack.NewLinpackBenchmark(writer, true),
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
}
