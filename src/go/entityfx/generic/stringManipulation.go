package generic

import "github.com/EntityFX/EntityFX-Bench/src/go/entityfx/utils"

import "strings"

type StringManipulation struct {
	*BenchmarkBaseBase
}

type ParallelStringManipulation struct {
	*BenchmarkBaseBase
}

func newStringManipulationBase(writer utils.WriterType, printToConsole bool) *BenchmarkBaseBase {
	var benchBase = NewBenchmarkBase(writer, printToConsole)
	benchBase.Iterrations = 5000000
	benchBase.Ratio = 10

	return benchBase
}

func NewStringManipulation(writer utils.WriterType, printToConsole bool) *StringManipulation {
	var benchBase = newStringManipulationBase(writer, printToConsole)
	stringManipulation := &StringManipulation{benchBase}
	benchBase.Child = stringManipulation

	return stringManipulation
}

func NewParallelStringManipulation(writer utils.WriterType, printToConsole bool) *ParallelStringManipulation {
	var benchBase = newStringManipulationBase(writer, printToConsole)
	stringManipulation := &ParallelStringManipulation{benchBase}
	benchBase.Child = stringManipulation
	benchBase.IsParallel = true

	return stringManipulation
}

func doStringManipilation(str string) string {
	return strings.Replace(
		strings.ToLower(
			strings.ToUpper(
				strings.Replace(
					strings.Join(
						strings.Split(str, " "), "/"), "/", "_", -1))+"AAA"), "aaa", ".", -1)
}

func (b *StringManipulation) BenchImplementation() interface{} {
	str := "the quick brown fox jumps over the lazy dog"
	str1 := ""
	var i int64
	for ; i < b.GetIterrations(); i++ {
		str1 = doStringManipilation(str)
	}
	return str1
}

func (b *ParallelStringManipulation) BenchImplementation() interface{} {
	return b.BenchmarkBaseBase.BenchInParallel(func () interface{}  {
		return 0
	}, func (interface{}) interface{}  {
		str := "the quick brown fox jumps over the lazy dog"
		str1 := ""
		var i int64
		for ; i < b.GetIterrations(); i++ {
			str1 = doStringManipilation(str)
		}
		return str1
	}, func (result interface{}, benchResult *BenchResult)  {

	})
}
