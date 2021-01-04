package generic

import (
	"../utils"
)

type RandomMemoryBenchmark struct {
	*BenchmarkBaseBase
}

func NewRandomMemoryBenchmark(writer utils.WriterType, printToConsole bool) *RandomMemoryBenchmark {
	var benchBase = NewBenchmarkBase(writer, printToConsole)
	benchBase.Iterrations = 500000
	benchBase.Ratio = 2

	arithmeticsBenchmark := &RandomMemoryBenchmark{benchBase}

	benchBase.Child = arithmeticsBenchmark

	return arithmeticsBenchmark
}

func (m *RandomMemoryBenchmark) BenchRandomMemory() float64 {
	int4k, _ := m.MeasureArrayRandomRead(1024)
	m.BenchmarkBaseBase.Output.WriteLine("int 4k: %.2f MB/s", int4k)
	int512k, _ := m.MeasureArrayRandomRead(131072)
	m.BenchmarkBaseBase.Output.WriteLine("int 512k: %.2f MB/s", int512k)
	int8m, _ := m.MeasureArrayRandomRead(2097152)
	m.BenchmarkBaseBase.Output.WriteLine("int 8M: %.2f MB/s", int8m)

	long4k, _ := m.MeasureArrayRandomLongRead(1024)
	m.BenchmarkBaseBase.Output.WriteLine("long 4k: %.2f MB/s", long4k)
	long512k, _ := m.MeasureArrayRandomLongRead(131072)
	m.BenchmarkBaseBase.Output.WriteLine("long 512k: %.2f MB/s", long512k)
	long8m, _ := m.MeasureArrayRandomLongRead(2097152)
	m.BenchmarkBaseBase.Output.WriteLine("long 8M: %.2f MB/s", long8m)

	avg := utils.Average([]float64{int4k, int512k, int8m, long4k, long512k, long8m})

	m.BenchmarkBaseBase.Output.WriteLine("Average: %.2f MB/s", avg)

	return avg
}

func (m *RandomMemoryBenchmark) MeasureArrayRandomRead(size int32) (float64, int32) {
	I := int32(0)
	const MaxInt = int32(^uint32(0) >> 1)
	array := utils.RandomIntArray(size, MaxInt)

	end := int32(len(array) - 1)
	indexes := utils.RandomIntArray(end, end)
	k0 := size / 1024
	var k1 int64 = 0
	if (k0 == 0) { k1 = 1 } else { k1 = int64(k0) }

	iterInternal := m.Iterrations / k1
	if (iterInternal == 0) { iterInternal = 1 }
	var idx int64 = 0
	var i int64 = 0
	for ; idx < int64(end); idx++ {
		I = array[idx]
	}
	start := utils.MakeTimestamp()
	for ; i < iterInternal; i++ {
		for _, idx := range indexes {
			I = array[idx]
		}
	}
	elapsed := utils.MakeTimestamp() - start
	return float64(iterInternal) * float64(len(array)) * 4.0 / (float64(elapsed) / 1000.0) / 1024.0 / 1024.0, I
}

func (m *RandomMemoryBenchmark) MeasureArrayRandomLongRead(size int32) (float64, int64) {
	L := int64(0)
	const MaxLong = int64(^uint64(0) >> 1)
	array := utils.RandomLongArray(size, MaxLong)

	end := int32(len(array) - 1)
	indexes := utils.RandomIntArray(end, end)
	k0 := size / 1024
	var k1 int64 = 0
	if (k0 == 0) { k1 = 1 } else { k1 = int64(k0) }

	iterInternal := m.Iterrations / k1
	if (iterInternal == 0) { iterInternal = 1 }
	var idx int64 = 0
	var i int64 = 0
	for ; idx < int64(end); idx++ {
		L = array[idx]
	}
	start := utils.MakeTimestamp()
	for ; i < iterInternal; i++ {
		for _, idx := range indexes {
			L = array[idx]
		}
	}
	elapsed := utils.MakeTimestamp() - start
	return float64(iterInternal) * float64(len(array)) * 8.0 / (float64(elapsed) / 1000.0) / 1024.0 / 1024.0, L
}

func (b *RandomMemoryBenchmark) BenchImplementation() interface{} {
	return b.BenchRandomMemory()
}

func  (b *RandomMemoryBenchmark) PopulateResult(benchResult *BenchResult, memoryBenchmarkResult interface{}) *BenchResult {
	benchResult.Points = memoryBenchmarkResult.(float64) * b.Ratio
	benchResult.Result = memoryBenchmarkResult.(float64) 
	benchResult.Units = "MB/s"
	benchResult.Output = ""
	return benchResult
}