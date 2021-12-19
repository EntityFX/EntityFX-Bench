package generic

import "github.com/EntityFX/EntityFX-Bench/src/go/entityfx/utils"

import "math"

type MathBenchmark struct {
	*BenchmarkBaseBase
}

type ParallelMathBenchmark struct {
	*BenchmarkBaseBase
}

func newMathBenchmarkBase(writer utils.WriterType, printToConsole bool) *BenchmarkBaseBase {
	var benchBase = NewBenchmarkBase(writer, printToConsole)
	benchBase.Iterrations = 200000000
	benchBase.Ratio = 0.5

	return benchBase
}

func NewMathBenchmark(writer utils.WriterType, printToConsole bool) *MathBenchmark {
	var benchBase = newMathBenchmarkBase(writer, printToConsole)
	mathBenchmark := &MathBenchmark{benchBase}
	benchBase.Child = mathBenchmark

	return mathBenchmark
}

func NewParallelMathBenchmark(writer utils.WriterType, printToConsole bool) *ParallelMathBenchmark {
	var benchBase = newMathBenchmarkBase(writer, printToConsole)
	mathBenchmark := &ParallelMathBenchmark{benchBase}
	benchBase.IsParallel = true
	benchBase.Child = mathBenchmark

	return mathBenchmark
}

func doMath(if64 float64) float64 {
	var rev float64 = 1.0 / (if64 + 1.0)
	return math.Abs(if64)*math.Acos(rev)*math.Asin(rev)*math.Atan(rev) +
		math.Floor(if64) + math.Exp(rev)*math.Cos(if64)*math.Sin(if64)*math.Pi + math.Sqrt(if64)
}

func (b *MathBenchmark) BenchImplementation() interface{} {
	var R float64 = 0.0
	var i int64
	for ; i < b.GetIterrations(); i++ {
		R += doMath(float64(i))
	}
	return R
}

func (b *ParallelMathBenchmark) BenchImplementation() interface{} {
	return b.BenchmarkBaseBase.BenchInParallel(func () interface{}  {
		return 0
	}, func (interface{}) interface{}  {
		var R float64 = 0.0
		var i int64
		for ; i < b.GetIterrations(); i++ {
			R += doMath(float64(i))
		}
		return R
	}, func (result interface{}, benchResult *BenchResult)  {

	})
}

