package scimark2

import (
	"reflect"

	g "../generic"
	"../utils"
)

type Scimark2Benchmark struct {
	*g.BenchmarkBaseBase
}

type ParallelScimark2Benchmark struct {
	*Scimark2Benchmark
}

func newScimark2BenchmarkBase(writer utils.WriterType, printToConsole bool) *g.BenchmarkBaseBase {
	var benchBase = g.NewBenchmarkBase(writer, printToConsole)
	benchBase.Ratio = 10

	return benchBase
}

func NewScimark2Benchmark(writer utils.WriterType, printToConsole bool) *Scimark2Benchmark {
	var benchBase = newScimark2BenchmarkBase(writer, printToConsole)
	scimark2Benchmark := &Scimark2Benchmark{benchBase}
	benchBase.Child = scimark2Benchmark

	return scimark2Benchmark
}

func NewParallelScimark2Benchmark(writer utils.WriterType, printToConsole bool) *ParallelScimark2Benchmark {
	var benchBase = NewScimark2Benchmark(writer, printToConsole)
	scimark2Benchmark := &ParallelScimark2Benchmark{benchBase}
	benchBase.Child = scimark2Benchmark
	benchBase.IsParallel = true
	scimark2Benchmark.Scimark2Benchmark = benchBase
	return scimark2Benchmark
}

func (b *Scimark2Benchmark) BenchImplementation() interface{} {
	return Bench(RESOLUTION_DEFAULT, false, utils.NewWriter(""))
}

func (b *ParallelScimark2Benchmark) BenchImplementation() interface{} {
	return b.BenchmarkBaseBase.BenchInParallel(func() interface{} {
		return nil
	}, func(interface{}) interface{} {
		w := utils.NewWriter("")
		w.UseConsole(false)
		return Bench(RESOLUTION_DEFAULT, false, w)
	}, func(result interface{}, benchResult *g.BenchResult) {
		benchResult.Points = result.(*Scimark2Result).compositeScore * b.Ratio
		benchResult.Result = result.(*Scimark2Result).compositeScore
		benchResult.Output = result.(*Scimark2Result).Output
	})
}

func (b *Scimark2Benchmark) PopulateResult(benchResult *g.BenchResult, scimark2Result interface{}) *g.BenchResult {
	benchResult.Points = scimark2Result.(*Scimark2Result).compositeScore * b.Ratio
	benchResult.Result = scimark2Result.(*Scimark2Result).compositeScore
	benchResult.Units = "CompositeScore"
	benchResult.Output = scimark2Result.(*Scimark2Result).Output
	return benchResult
}

func (b *Scimark2Benchmark) Warmup(aspect float64) {
	var name string
	if t := reflect.TypeOf(b.Child); t.Kind() == reflect.Ptr {
		name = t.Elem().Name()
	} else {
		name = t.Name()
	}
	b.Name = name
}

func (b *ParallelScimark2Benchmark) PopulateResult(benchResult *g.BenchResult, results interface{}) *g.BenchResult {
	result := b.BuildParallelResult(benchResult, results.([]*g.BenchResult))
	resultSum := 0.0
	output := ""
	for _, r := range results.([]*g.BenchResult) {
		resultSum += r.Result
		output += (r.Output + "\n\n")
	}

	result.Result = resultSum
	result.Units = "CompositeScore"
	result.Output = output
	return result
}
