package generic

import (
	"crypto/sha1"
	"crypto/sha256"

	"../utils"
)

type HashBenchmark struct {
	*BenchmarkBaseBase
	strs         [3]string
	arrayOfBytes [][]byte
}

type ParallelHashBenchmark struct {
	*HashBenchmark
}

func NewHashBenchmark(writer utils.WriterType, printToConsole bool) *HashBenchmark {
	var benchBase = NewBenchmarkBase(writer, printToConsole)
	benchBase.Iterrations = 2000000
	benchBase.Ratio = 10

	strs := [3]string{
		"the quick brown fox jumps over the lazy dog", "Some red wine",
		"Candels & Ropes"}

	hashBenchmark := &HashBenchmark{benchBase, strs, make([][]byte, len(strs))}

	for i, v := range strs {
		hashBenchmark.arrayOfBytes[i] = []byte(v)
	}

	benchBase.Child = hashBenchmark

	return hashBenchmark
}

func NewParallelHashBenchmark(writer utils.WriterType, printToConsole bool) *ParallelHashBenchmark {
	var benchBase = NewHashBenchmark(writer, printToConsole)

	hashBenchmark := &ParallelHashBenchmark{benchBase}

	benchBase.Child = hashBenchmark
	benchBase.IsParallel = true
	hashBenchmark.HashBenchmark = benchBase

	return hashBenchmark
}

func doHash(i int64, preparedBytes [][]byte) []byte {
	sha1Crypt := sha1.New()
	sha256Crypt := sha256.New()
	sha1Crypt.Write(preparedBytes[i%3])
	sha256Crypt.Write(preparedBytes[(i+1)%3])
	sha1bytes := sha1Crypt.Sum(nil)
	sha256bytes := sha256Crypt.Sum(nil)
	return append(sha1bytes, sha256bytes...)
}

func (b *HashBenchmark) BenchImplementation() interface{} {
	var result []byte
	for i := int64(0); i < b.Iterrations; i++ {
		result = doHash(i, b.arrayOfBytes)
	}
	return result
}

func (b *ParallelHashBenchmark) BenchImplementation() interface{} {
	return b.BenchmarkBaseBase.BenchInParallel(func() interface{} {
		return 0
	}, func(interface{}) interface{} {
		var result []byte
		for i := int64(0); i < b.Iterrations; i++ {
			result = doHash(i, b.arrayOfBytes)
		}
		return result
	}, func(result interface{}, benchResult *BenchResult) {

	})
}
