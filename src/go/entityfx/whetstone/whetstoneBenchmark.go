package whetstone

import (
	g "../generic"
	"../utils"
	"reflect"
)

type WhetstoneBenchmark struct {
	*g.BenchmarkBaseBase
}

func NewWhetstoneBenchmark(writer utils.WriterType, printToConsole bool) *WhetstoneBenchmark {
	var benchBase = g.NewBenchmarkBase(writer, printToConsole)
	benchBase.Ratio = 1

	dhrystoneBenchmark := &WhetstoneBenchmark{benchBase}

	benchBase.Child = dhrystoneBenchmark

	return dhrystoneBenchmark
}

func (b *WhetstoneBenchmark) BenchImplementation() interface{} {
	return Bench(true, b.BenchmarkBaseBase.Output)
}

func (b *WhetstoneBenchmark) PopulateResult(benchResult *g.BenchResult, whetstoneResult interface{}) *g.BenchResult {
	benchResult.Points = whetstoneResult.(*WhetstoneResult).MWIPS * b.Ratio
	benchResult.Result = whetstoneResult.(*WhetstoneResult).MWIPS
	benchResult.Units = "MWIPS"
	benchResult.Output = ""
	return benchResult
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
