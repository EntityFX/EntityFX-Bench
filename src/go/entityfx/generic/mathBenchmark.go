package generic

import "../utils"

import "math"

type MathBenchmark struct {
	*BenchmarkBaseBase
}

func NewMathBenchmark(writer utils.WriterType, printToConsole bool) *MathBenchmark {
	var benchBase = NewBenchmarkBase(writer, printToConsole)
	benchBase.Iterrations = 200000000
	benchBase.Ratio = 0.5

	mathBenchmark := &MathBenchmark{benchBase}

	benchBase.child = mathBenchmark

	return mathBenchmark
}

func DoMath(if64 float64) float64 {
	var rev float64 = 1.0 / (if64 + 1.0)
	return math.Abs(if64) * math.Acos(rev) * math.Asin(rev) * math.Atan(rev) +
		math.Floor(if64) + math.Exp(rev) * math.Cos(if64) * math.Sin(if64) * math.Pi + math.Sqrt(if64)
}

func (b *MathBenchmark) BenchImplementation() interface{} {
	var R float64 = 0.0
	var i int64
	for ; i < b.GetIterrations(); i++ {
		R += DoMath(float64(i))
	}
	return R
}