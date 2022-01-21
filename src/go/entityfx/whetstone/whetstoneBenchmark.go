package whetstone

import (
	"reflect"

	g "github.com/EntityFX/EntityFX-Bench/src/go/entityfx/generic"
	"github.com/EntityFX/EntityFX-Bench/src/go/entityfx/utils"
)

type WhetstoneBenchmark struct {
	*g.BenchmarkBaseBase
}

type ParallelWhetstoneBenchmark struct {
	*WhetstoneBenchmark
}

func newWhetstoneBenchmark(writer utils.WriterType, printToConsole bool) *g.BenchmarkBaseBase {
	var benchBase = g.NewBenchmarkBase(writer, printToConsole)
	benchBase.Ratio = 1
	return benchBase
}

func NewWhetstoneBenchmark(writer utils.WriterType, printToConsole bool) *WhetstoneBenchmark {
	var benchBase = newWhetstoneBenchmark(writer, printToConsole)
	whetstoneBenchmark := &WhetstoneBenchmark{benchBase}
	benchBase.Child = whetstoneBenchmark

	return whetstoneBenchmark
}

func NewParallelWhetstoneBenchmark(writer utils.WriterType, printToConsole bool) *ParallelWhetstoneBenchmark {
	var benchBase = NewWhetstoneBenchmark(writer, printToConsole)
	whetstoneBenchmark := &ParallelWhetstoneBenchmark{benchBase}
	benchBase.Child = whetstoneBenchmark
	benchBase.IsParallel = true
	whetstoneBenchmark.WhetstoneBenchmark = benchBase

	return whetstoneBenchmark
}

func (b *WhetstoneBenchmark) BenchImplementation() interface{} {
	return Bench(true, utils.NewWriter(""))
}

func (b *ParallelWhetstoneBenchmark) BenchImplementation() interface{} {
	return b.BenchmarkBaseBase.BenchInParallel(func() interface{} {
		return nil
	}, func(interface{}) interface{} {
		w := utils.NewWriter("")
		w.UseConsole(false)
		return Bench(true, w)
	}, func(result interface{}, benchResult *g.BenchResult) {
		benchResult.Points = result.(*WhetstoneResult).MWIPS * b.Ratio
		benchResult.Result = result.(*WhetstoneResult).MWIPS
		benchResult.Output = result.(*WhetstoneResult).Output
	})
}

func (b *WhetstoneBenchmark) PopulateResult(benchResult *g.BenchResult, whetstoneResult interface{}) *g.BenchResult {
	benchResult.Points = whetstoneResult.(*WhetstoneResult).MWIPS * b.Ratio
	benchResult.Result = whetstoneResult.(*WhetstoneResult).MWIPS
	benchResult.Units = "MWIPS"
	benchResult.Output = whetstoneResult.(*WhetstoneResult).Output
	return benchResult
}

func (b *ParallelWhetstoneBenchmark) PopulateResult(benchResult *g.BenchResult, results interface{}) *g.BenchResult {
	result := b.BuildParallelResult(benchResult, results.([]*g.BenchResult))
	resultSum := 0.0
	output := ""
	for _, r := range results.([]*g.BenchResult) {
		resultSum += r.Result
		output += r.Output + "\n\n"
	}
	result.Points = resultSum * benchResult.Ratio
	result.Result = resultSum
	result.Units = "MWIPS"
	result.Output = output
	return result
}

func (b *WhetstoneBenchmark) Warmup(aspect float64) {
	var name string
	if t := reflect.TypeOf(b.Child); t.Kind() == reflect.Ptr {
		name = t.Elem().Name()
	} else {
		name = t.Name()
	}
	b.Name = name
}
