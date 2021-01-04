package generic

import (
	"../utils"
)

type ArithmeticsBenchmark struct {
	*BenchmarkBaseBase
	R float32
}

func NewArithmetics(writer utils.WriterType, printToConsole bool) *ArithmeticsBenchmark {
	var benchBase = NewBenchmarkBase(writer, printToConsole)
	benchBase.Iterrations = 300000000
	benchBase.Ratio = 0.03

	arithmeticsBenchmark := &ArithmeticsBenchmark{benchBase, 0.0}

	benchBase.Child = arithmeticsBenchmark

	return arithmeticsBenchmark
}

func DoArithmetics(i int64) float32 {
	var if32 = float32(i)
	r := (if32 / 10.0) * (if32 / 100.0) * (if32 / 100.0) * (if32 / 100.0) * 1.11 + 
		(if32 / 100) * (if32 / 1000.0) * (if32 / 1000.0) * 2.22 -
		if32 * (if32 / 10000.0) * 3.33 +
		if32 * 5.33
	return r
}

func (b *ArithmeticsBenchmark) BenchImplementation() interface{} {
	b.R = 0
	var i int64
	for ; i < b.GetIterrations(); i++ {
		b.R += DoArithmetics(i)
	}
	return b.R
}