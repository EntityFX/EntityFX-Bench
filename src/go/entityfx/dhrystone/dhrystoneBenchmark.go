package dhrystone

import (
	"../utils"
	g "../generic"
	"reflect"
)

type DhrystoneBenchmark struct {
	*g.BenchmarkBaseBase
}

func NewDhrystoneBenchmark(writer utils.WriterType, printToConsole bool) *DhrystoneBenchmark {
	var benchBase = g.NewBenchmarkBase(writer, printToConsole)
	benchBase.Ratio = 4

	dhrystoneBenchmark := &DhrystoneBenchmark{benchBase}

	benchBase.Child = dhrystoneBenchmark

	return dhrystoneBenchmark
}


func (b *DhrystoneBenchmark) BenchImplementation() interface{} {
	return Bench(LOOPS, b.BenchmarkBaseBase.Output)
}

func  (b *DhrystoneBenchmark) PopulateResult(benchResult *g.BenchResult, dhrystoneResult interface{}) *g.BenchResult {
	benchResult.Points = dhrystoneResult.(*DhrystoneResult).VaxMips * b.Ratio
	benchResult.Result = dhrystoneResult.(*DhrystoneResult).VaxMips
	benchResult.Units = "DMIPS"
	benchResult.Output = ""
	return benchResult
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