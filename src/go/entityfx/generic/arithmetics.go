package generic

import (
	"../utils"
)

type ArithmeticsBenchmark struct {
	*BenchmarkBaseBase
	R float32
}

type ParallelArithemticsBenchmark struct {
	*BenchmarkBaseBase
}

func newArithmeticsBase(writer utils.WriterType, printToConsole bool) *BenchmarkBaseBase {
	var benchBase = NewBenchmarkBase(writer, printToConsole)
	benchBase.Iterrations = 300000000
	benchBase.Ratio = 0.03
	return benchBase
}

func NewArithmetics(writer utils.WriterType, printToConsole bool) *ArithmeticsBenchmark {
	var benchBase = newArithmeticsBase(writer, printToConsole)

	arithmeticsBenchmark := &ArithmeticsBenchmark{benchBase, 0.0}

	benchBase.Child = arithmeticsBenchmark

	return arithmeticsBenchmark
}

func NewParallelArithmetics(writer utils.WriterType, printToConsole bool) *ParallelArithemticsBenchmark {
	var benchBase = newArithmeticsBase(writer, printToConsole)

	arithmeticsBenchmark := &ParallelArithemticsBenchmark{benchBase}
	benchBase.IsParallel = true
	benchBase.Child = arithmeticsBenchmark

	return arithmeticsBenchmark
}

func doArithmetics(i int64) float32 {
	var if32 = float32(i)
	r := (if32/10.0)*(if32/100.0)*(if32/100.0)*(if32/100.0)*1.11 +
		(if32/100)*(if32/1000.0)*(if32/1000.0)*2.22 -
		if32*(if32/10000.0)*3.33 +
		if32*5.33
	return r
}

func (b *ArithmeticsBenchmark) BenchImplementation() interface{} {
	b.R = 0
	var i int64
	for ; i < b.GetIterrations(); i++ {
		b.R += doArithmetics(i)
	}

	return b.R
}

func (b *ParallelArithemticsBenchmark) BenchImplementation() interface{} {
	return b.BenchmarkBaseBase.BenchInParallel(func () interface{}  {
		return 0
	}, func (interface{}) interface{}  {
		R := float32(0.0)
		var i int64
		for ; i < b.GetIterrations(); i++ {
			R += doArithmetics(i)
		}
	
		return R
	}, func (result interface{}, benchResult *BenchResult)  {

	})
}
