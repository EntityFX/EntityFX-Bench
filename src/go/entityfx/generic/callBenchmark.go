package generic

import (
	"runtime"

	"github.com/EntityFX/EntityFX-Bench/src/go/entityfx/utils"
)

type CallBenchmark struct {
	*BenchmarkBaseBase
}

type ParallelCallBenchmark struct {
	*CallBenchmark
}

func newCallBenchmarkBase(writer utils.WriterType, printToConsole bool) *BenchmarkBaseBase {
	var benchBase = NewBenchmarkBase(writer, printToConsole)
	benchBase.Iterrations = 2000000000
	benchBase.Ratio = 0.01

	return benchBase
}

func NewCallBenchmark(writer utils.WriterType, printToConsole bool) *CallBenchmark {
	var benchBase = newCallBenchmarkBase(writer, printToConsole)
	callBenchmark := &CallBenchmark{benchBase}
	benchBase.Child = callBenchmark

	return callBenchmark
}

func NewParallelCallBenchmark(writer utils.WriterType, printToConsole bool) *ParallelCallBenchmark {
	var benchBase = NewCallBenchmark(writer, printToConsole)
	callBenchmark := &ParallelCallBenchmark{benchBase}
	benchBase.IsParallel = true
	benchBase.Child = callBenchmark

	callBenchmark.CallBenchmark = benchBase

	return callBenchmark
}

func doCall(i float64, b float64) float64 {
	var z float64 = i * 0.7
	var z1 float64 = i * b
	return z + z1 + 0.5
}

func (c *CallBenchmark) doCallBench() (int64, float64) {
	start := utils.MakeTimestamp()
	var elapsed1 int64 = 0
	var elapsed2 int64 = 0
	var a float64 = 0.0

	var i int64 = 0
	for i = 0; i < c.GetIterrations(); i++ {
		var z float64 = a * 0.7
		var z1 float64 = a * 0.01
		a = z + z1 + 0.5
	}

	elapsed1 = utils.MakeTimestamp() - start
	a = 0.0
	i = 0
	start = utils.MakeTimestamp()
	for i = 0; i < c.GetIterrations(); i++ {
		a = doCall(a, 0.01)
	}
	elapsed2 = utils.MakeTimestamp() - start

	writer := c.BenchmarkBaseBase.Output
	writer.Write("Elapsed No Call: %d", elapsed1)
	writer.WriteNewLine()
	writer.Write("Elapsed Call: %d", elapsed2)
	writer.WriteNewLine()

	var callTime int64 = 0

	if elapsed2 <= elapsed1 {
		callTime = elapsed1 - elapsed2
	} else {
		callTime = elapsed2 - elapsed1
	}

	return callTime, a
}

func (b *CallBenchmark) Bench() *BenchResult {
	b.BeforeBench()
	start := utils.MakeTimestamp()
	elapsed, value := b.doCallBench()
	result := b.PopulateResult(b.BuildResult(start), value)
	result.Elapsed = elapsed
	b.doOutput(result)
	b.AfterBench(result)
	return result
}

func (b *ParallelCallBenchmark) Bench() *BenchResult {
	return b.BenchmarkBaseBase.Bench()
}

func (b *ParallelCallBenchmark) BenchInParallel(buildFunc func() interface{}) []*BenchResult {
	b.useConsole(false)
	count := runtime.NumCPU()

	parallelResults := runParallel(func(i int) *BenchResult {
		start := utils.MakeTimestamp()
		elapsed, _ := b.CallBenchmark.doCallBench()
		benchResult := b.BuildResult(start)
		benchResult.Elapsed = elapsed
		return benchResult
	}, count)

	b.useConsole(true)
	return parallelResults
}

func (b *ParallelCallBenchmark) BenchImplementation() interface{} {
	return b.BenchInParallel(func() interface{} {
		return 0
	})
}
