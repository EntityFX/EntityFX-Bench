package generic

import (
	"../utils"
)

type IfElseBenchmark struct {
	*BenchmarkBaseBase
}

func NewIfElseBenchmark(writer utils.WriterType, printToConsole bool) *IfElseBenchmark {
	var benchBase = NewBenchmarkBase(writer, printToConsole)
	benchBase.Iterrations = 2000000000
	benchBase.Ratio = 0.01

	ifElseBenchmark := &IfElseBenchmark{benchBase}

	benchBase.child = ifElseBenchmark

	return ifElseBenchmark
}

func (b *IfElseBenchmark) BenchImplementation() interface{} {
	d := 0
	c := -1
	var i int64 = 0
	for ; i < b.Iterrations; i++ {
		if c == -4 {
			c = -1
		}

		if i == -1 {
			d = 3
		} else if i == -2 {
			d = 2
		} else if i == -3	{
			d = 1
		}
		d += 1
		c--
	}
	return d
}