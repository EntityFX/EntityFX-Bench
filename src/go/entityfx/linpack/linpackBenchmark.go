package linpack

import (
	"../utils"
	g "../generic"
	"reflect"
)

type LinpackBenchmark struct {
	*g.BenchmarkBaseBase
}

func NewLinpackBenchmark(writer utils.WriterType, printToConsole bool) *LinpackBenchmark {
	var benchBase = g.NewBenchmarkBase(writer, printToConsole)
	benchBase.Ratio = 10

	linpackBenchmark := &LinpackBenchmark{benchBase}

	benchBase.Child = linpackBenchmark

	return linpackBenchmark
}


func (b *LinpackBenchmark) BenchImplementation() interface{} {
	return RunBenchmark(2000, b.BenchmarkBaseBase.Output)
}

func  (b *LinpackBenchmark) PopulateResult(benchResult *g.BenchResult, linpackResult interface{}) *g.BenchResult {
	benchResult.Points = linpackResult.(*LinpackResult).MFLOPS * b.Ratio
	benchResult.Result = linpackResult.(*LinpackResult).MFLOPS
	benchResult.Units = "MFLOPS"
	benchResult.Output = ""
	return benchResult
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