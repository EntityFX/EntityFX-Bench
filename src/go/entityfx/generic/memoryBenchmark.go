package generic

import (
	"../utils"
)

type MemoryBenchmark struct {
	*BenchmarkBaseBase
}

func NewMemoryBenchmark(writer utils.WriterType, printToConsole bool) *MemoryBenchmark {
	var benchBase = NewBenchmarkBase(writer, printToConsole)
	benchBase.Iterrations = 500000
	benchBase.Ratio = 1

	arithmeticsBenchmark := &MemoryBenchmark{benchBase}

	benchBase.child = arithmeticsBenchmark

	return arithmeticsBenchmark
}

func (m *MemoryBenchmark) BenchRandomMemory() float64 {
	int4k, _ := m.MeasureArrayRandomRead(1024)
	m.BenchmarkBaseBase.output.WriteLine("int 4k: %.2f MB/s", int4k)
	int512k, _ := m.MeasureArrayRandomRead(131072)
	m.BenchmarkBaseBase.output.WriteLine("int 512k: %.2f MB/s", int512k)
	int8m, _ := m.MeasureArrayRandomRead(2097152)
	m.BenchmarkBaseBase.output.WriteLine("int 8M: %.2f MB/s", int8m)
	int32m, _ := m.MeasureArrayRandomRead(32 * 1024 * 1024 / 4);
	m.BenchmarkBaseBase.output.WriteLine("int 32M: %.2f MB/s", int32m)

	long4k, _ := m.MeasureArrayRandomLongRead(1024)
	m.BenchmarkBaseBase.output.WriteLine("long 4k: %.2f MB/s", long4k)
	long512k, _ := m.MeasureArrayRandomLongRead(131072)
	m.BenchmarkBaseBase.output.WriteLine("long 512k: %.2f MB/s", long512k)
	long8m, _ := m.MeasureArrayRandomLongRead(2097152)
	m.BenchmarkBaseBase.output.WriteLine("long 8M: %.2f MB/s", long8m)
	long32m, _ := m.MeasureArrayRandomLongRead(32 * 1024 * 1024 / 8);
	m.BenchmarkBaseBase.output.WriteLine("long 32M: %.2f MB/s", long32m)

	avg := utils.Average([]float64{int4k, int512k, int8m, int32m, long4k, long512k, long8m, long32m})

	m.BenchmarkBaseBase.output.WriteLine("Average: %.2f MB/s", avg)

	return avg
}

func (m *MemoryBenchmark) MeasureArrayRandomRead(size int32) (float64, [16]int32) {
	const blockSize = 16
	I := [blockSize]int32{}

	const MaxInt = int32(^uint32(0) >> 1)

	var array []int32 = utils.RandomIntArray(size, MaxInt)
	end := int64(len(array) - 1)
	k0 := size / 1024
	var k1 int64 = 0
	if (k0 == 0) { k1 = 1 } else { k1 = int64(k0) }

	iterInternal := m.Iterrations / k1
	if (iterInternal == 0) { iterInternal = 1 }
	var idx int64 = 0
	var i int64 = 0
	for ; idx < end; idx+=blockSize {
		I[0] = array[idx]
		I[1] = array[idx + 1]
		I[2] = array[idx + 2]
		I[3] = array[idx + 3]
		I[4] = array[idx + 4]
		I[5] = array[idx + 5]
		I[6] = array[idx + 6]
		I[7] = array[idx + 7]
		I[8] = array[idx + 8]
		I[9] = array[idx + 9]
		I[0xA] = array[idx + 0xA]
		I[0xB] = array[idx + 0xB]
		I[0xC] = array[idx + 0xC]
		I[0xD] = array[idx + 0xD]
		I[0xE] = array[idx + 0xE]
		I[0xF] = array[idx + 0xF]
	}
	start := utils.MakeTimestamp()
	for ; i < iterInternal; i++ {
		idx = 0
		for ; idx < end; idx+=blockSize {
			I[0] = array[idx]
			I[1] = array[idx + 1]
			I[2] = array[idx + 2]
			I[3] = array[idx + 3]
			I[4] = array[idx + 4]
			I[5] = array[idx + 5]
			I[6] = array[idx + 6]
			I[7] = array[idx + 7]
			I[8] = array[idx + 8]
			I[9] = array[idx + 9]
			I[0xA] = array[idx + 0xA]
			I[0xB] = array[idx + 0xB]
			I[0xC] = array[idx + 0xC]
			I[0xD] = array[idx + 0xD]
			I[0xE] = array[idx + 0xE]
			I[0xF] = array[idx + 0xF]
		}
	}
	elapsed := utils.MakeTimestamp() - start
	return float64(iterInternal) * float64(len(array)) * 4.0 / (float64(elapsed) / 1000.0) / 1024.0 / 1024.0, I
}

func (m *MemoryBenchmark) MeasureArrayRandomLongRead(size int32) (float64, [8]int64) {
	const blockSize = 8
	L := [blockSize]int64{}

	const MaxLong = int64(^uint64(0) >> 1)

	var array []int64 = utils.RandomLongArray(size, MaxLong)
	end := int64(len(array) - 1)
	k0 := size / 1024
	var k1 int64 = 0
	if (k0 == 0) { k1 = 1 } else { k1 = int64(k0) }

	iterInternal := m.Iterrations / k1
	if (iterInternal == 0) { iterInternal = 1 }
	var idx int64 = 0
	var i int64 = 0
	for ; idx < end; idx+=blockSize {
		L[0] = array[idx]
		L[1] = array[idx + 1]
		L[2] = array[idx + 2]
		L[3] = array[idx + 3]
		L[4] = array[idx + 4]
		L[5] = array[idx + 5]
		L[6] = array[idx + 6]
		L[7] = array[idx + 7]
	}
	start := utils.MakeTimestamp()
	for ; i < iterInternal; i++ {
		idx = 0
		for ; idx < end; idx+=blockSize {
			L[0] = array[idx]
			L[1] = array[idx + 1]
			L[2] = array[idx + 2]
			L[3] = array[idx + 3]
			L[4] = array[idx + 4]
			L[5] = array[idx + 5]
			L[6] = array[idx + 6]
			L[7] = array[idx + 7]
		}
	}
	elapsed := utils.MakeTimestamp() - start
	return float64(iterInternal) * float64(len(array)) * 8.0 / (float64(elapsed) / 1000.0) / 1024.0 / 1024.0, L
}

func (b *MemoryBenchmark) BenchImplementation() interface{} {
	return b.BenchRandomMemory()
}

func  (b *MemoryBenchmark) PopulateResult(benchResult *BenchResult, memoryBenchmarkResult interface{}) *BenchResult {
	benchResult.Points = memoryBenchmarkResult.(float64) * b.Ratio
	benchResult.Result = memoryBenchmarkResult.(float64) 
	benchResult.Units = "MB/s"
	benchResult.Output = ""
	return benchResult
}