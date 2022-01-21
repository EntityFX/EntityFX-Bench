package dhrystone

import (
	"reflect"

	g "github.com/EntityFX/EntityFX-Bench/src/go/entityfx/generic"
	"github.com/EntityFX/EntityFX-Bench/src/go/entityfx/utils"
)

type DhrystoneBenchmark struct {
	*g.BenchmarkBaseBase
}

type ParallelDhrystoneBenchmark struct {
	*DhrystoneBenchmark
}

func newDhrystoneBenchmarkBase(writer utils.WriterType, printToConsole bool) *g.BenchmarkBaseBase {
	var benchBase = g.NewBenchmarkBase(writer, printToConsole)
	benchBase.Ratio = 4

	return benchBase
}

func NewDhrystoneBenchmark(writer utils.WriterType, printToConsole bool) *DhrystoneBenchmark {
	var benchBase = newDhrystoneBenchmarkBase(writer, printToConsole)
	dhrystoneBenchmark := &DhrystoneBenchmark{benchBase}
	benchBase.Child = dhrystoneBenchmark

	return dhrystoneBenchmark
}

func NewParallelDhrystoneBenchmark(writer utils.WriterType, printToConsole bool) *ParallelDhrystoneBenchmark {
	var benchBase = NewDhrystoneBenchmark(writer, printToConsole)
	dhrystoneBenchmark := &ParallelDhrystoneBenchmark{benchBase}
	benchBase.Child = dhrystoneBenchmark
	benchBase.IsParallel = true
	dhrystoneBenchmark.DhrystoneBenchmark = benchBase

	return dhrystoneBenchmark
}

func (b *DhrystoneBenchmark) BenchImplementation() interface{} {
	return Bench(LOOPS, utils.NewWriter(""))
}

func (b *ParallelDhrystoneBenchmark) BenchImplementation() interface{} {
	return b.BenchmarkBaseBase.BenchInParallel(func() interface{} {
		return nil
	}, func(interface{}) interface{} {
		w := utils.NewWriter("")
		w.UseConsole(false)
		return Bench(LOOPS, w)
	}, func(result interface{}, benchResult *g.BenchResult) {
		benchResult.Points = result.(*DhrystoneResult).VaxMips * b.Ratio
		benchResult.Result = result.(*DhrystoneResult).VaxMips
		benchResult.Output = result.(*DhrystoneResult).Output
	})
}

func (b *DhrystoneBenchmark) PopulateResult(benchResult *g.BenchResult, dhrystoneResult interface{}) *g.BenchResult {
	benchResult.Points = dhrystoneResult.(*DhrystoneResult).VaxMips * b.Ratio
	benchResult.Result = dhrystoneResult.(*DhrystoneResult).VaxMips
	benchResult.Units = "DMIPS"
	benchResult.Output = dhrystoneResult.(*DhrystoneResult).Output
	return benchResult
}

func (b *ParallelDhrystoneBenchmark) PopulateResult(benchResult *g.BenchResult, results interface{}) *g.BenchResult {
	result := b.BuildParallelResult(benchResult, results.([]*g.BenchResult))
	resultSum := 0.0
	output := ""
	for _, r := range results.([]*g.BenchResult) {
		resultSum += r.Result
		output += (r.Output + "\n\n")
	}
	result.Points = resultSum * benchResult.Ratio
	result.Result = resultSum
	result.Units = "DMIPS"
	result.Output = output
	return result
}

func (b *DhrystoneBenchmark) Warmup(aspect float64) {
	var name string
	if t := reflect.TypeOf(b.Child); t.Kind() == reflect.Ptr {
		name = t.Elem().Name()
	} else {
		name = t.Name()
	}
	b.Name = name
}
