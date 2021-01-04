package generic

import "../utils"

type CallBenchmark struct {
	*BenchmarkBaseBase
}

func NewCallBenchmark(writer utils.WriterType, printToConsole bool) *CallBenchmark {
	var benchBase = NewBenchmarkBase(writer, printToConsole)
	benchBase.Iterrations = 2000000000
	benchBase.Ratio = 0.01

	callBenchmark := &CallBenchmark{benchBase}

	benchBase.Child = callBenchmark

	return callBenchmark
}

func DoCall(i float64, b float64) float64 {
	var z float64 = i * 0.7
	var z1 float64 = i * b
	return  z + z1 + 0.5
}

func (c *CallBenchmark) DoCallBench() (int64, float64) {
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
		a = DoCall(a, 0.01)
	}
	elapsed2 = utils.MakeTimestamp() - start

	writer := c.BenchmarkBaseBase.Output
	writer.Write("Elapsed No Call: %d", elapsed1)
	writer.WriteNewLine()
	writer.Write("Elapsed Call: %d", elapsed2)
	writer.WriteNewLine()

	var callTime int64 = 0

	if (elapsed2 <= elapsed1) {
		callTime = elapsed1 - elapsed2
	} else {
		callTime = elapsed2 - elapsed1
	}

	return callTime, a
}

func (b *CallBenchmark) Bench() *BenchResult {
    b.BeforeBench()
    start := utils.MakeTimestamp()
    elapsed, value := b.DoCallBench()
	result := b.PopulateResult(b.BuildResult(start), value);
	result.Elapsed = elapsed
    b.DoOutput(result)
    b.AfterBench(result)
    return result
}