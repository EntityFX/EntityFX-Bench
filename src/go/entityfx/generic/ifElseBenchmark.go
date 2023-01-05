package generic

import (
	"github.com/EntityFX/EntityFX-Bench/utils"
)

type IfElseBenchmark struct {
	*BenchmarkBaseBase
}

type ParallelIfElseBenchmark struct {
	*BenchmarkBaseBase
}

func newIfElseBenchmarkBase(writer utils.WriterType, printToConsole bool) *BenchmarkBaseBase {
	var benchBase = NewBenchmarkBase(writer, printToConsole)
	benchBase.Iterrations = 2000000000
	benchBase.Ratio = 0.01

	return benchBase
}

func NewIfElseBenchmark(writer utils.WriterType, printToConsole bool) *IfElseBenchmark {
	var benchBase = newIfElseBenchmarkBase(writer, printToConsole)
	ifElseBenchmark := &IfElseBenchmark{benchBase}
	benchBase.Child = ifElseBenchmark

	return ifElseBenchmark
}

func NewParallelIfElseBenchmark(writer utils.WriterType, printToConsole bool) *ParallelIfElseBenchmark {
	var benchBase = newIfElseBenchmarkBase(writer, printToConsole)
	ifElseBenchmark := &ParallelIfElseBenchmark{benchBase}
	benchBase.Child = ifElseBenchmark
	benchBase.IsParallel = true

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
		} else if i == -3 {
			d = 1
		}
		d += 1
		c--
	}
	return d
}

func (b *ParallelIfElseBenchmark) BenchImplementation() interface{} {
	return b.BenchmarkBaseBase.BenchInParallel(func() interface{} {
		return 0
	}, func(interface{}) interface{} {
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
			} else if i == -3 {
				d = 1
			}
			d += 1
			c--
		}
		return d
	}, func(result interface{}, benchResult *BenchResult) {

	})
}
