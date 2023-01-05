package linpack

import (
	"reflect"

	g "github.com/EntityFX/EntityFX-Bench/generic"
	"github.com/EntityFX/EntityFX-Bench/utils"
)

type LinpackBenchmark struct {
	*g.BenchmarkBaseBase
}

type ParallelLinpackBenchmark struct {
	*LinpackBenchmark
}

func newLinpackBenchmark(writer utils.WriterType, printToConsole bool) *g.BenchmarkBaseBase {
	var benchBase = g.NewBenchmarkBase(writer, printToConsole)
	benchBase.Ratio = 10
	return benchBase
}

func NewLinpackBenchmark(writer utils.WriterType, printToConsole bool) *LinpackBenchmark {
	var benchBase = newLinpackBenchmark(writer, printToConsole)
	linpackBenchmark := &LinpackBenchmark{benchBase}
	benchBase.Child = linpackBenchmark

	return linpackBenchmark
}

func NewParallelLinpackBenchmark(writer utils.WriterType, printToConsole bool) *ParallelLinpackBenchmark {
	var benchBase = NewLinpackBenchmark(writer, printToConsole)
	linpackBenchmark := &ParallelLinpackBenchmark{benchBase}
	benchBase.Child = linpackBenchmark
	benchBase.IsParallel = true
	linpackBenchmark.LinpackBenchmark = benchBase
	return linpackBenchmark
}

func (b *LinpackBenchmark) BenchImplementation() interface{} {
	return RunBenchmark(2000, utils.NewWriter(""))
}

func (b *ParallelLinpackBenchmark) BenchImplementation() interface{} {
	return b.BenchmarkBaseBase.BenchInParallel(func() interface{} {
		return nil
	}, func(interface{}) interface{} {
		w := utils.NewWriter("")
		w.UseConsole(false)
		return RunBenchmark(2000, w)
	}, func(result interface{}, benchResult *g.BenchResult) {
		benchResult.Points = result.(*LinpackResult).MFLOPS * b.Ratio
		benchResult.Result = result.(*LinpackResult).MFLOPS
		benchResult.Output = result.(*LinpackResult).Output
	})
}

func (b *LinpackBenchmark) PopulateResult(benchResult *g.BenchResult, linpackResult interface{}) *g.BenchResult {
	benchResult.Points = linpackResult.(*LinpackResult).MFLOPS * b.Ratio
	benchResult.Result = linpackResult.(*LinpackResult).MFLOPS
	benchResult.Units = "MFLOPS"
	benchResult.Output = linpackResult.(*LinpackResult).Output
	return benchResult
}

func (b *ParallelLinpackBenchmark) PopulateResult(benchResult *g.BenchResult, results interface{}) *g.BenchResult {
	result := b.BuildParallelResult(benchResult, results.([]*g.BenchResult))
	resultSum := 0.0
	output := ""
	for _, r := range results.([]*g.BenchResult) {
		resultSum += r.Result
		output += (r.Output + "\n\n")
	}
	result.Points = resultSum * benchResult.Ratio
	result.Result = resultSum
	result.Units = "MFLOPS"
	result.Output = output
	return result
}

func (b *LinpackBenchmark) Warmup(aspect float64) {
	var name string
	if t := reflect.TypeOf(b.Child); t.Kind() == reflect.Ptr {
		name = t.Elem().Name()
	} else {
		name = t.Name()
	}
	b.Name = name
}
