package generic

import "../utils"

import "strings"

type StringManipulation struct {
	*BenchmarkBaseBase
}

func NewStringManipulation(writer utils.WriterType, printToConsole bool) *StringManipulation {
	var benchBase = NewBenchmarkBase(writer, printToConsole)
	benchBase.Iterrations = 5000000
	benchBase.Ratio = 10

	mathBenchmark := &StringManipulation{benchBase}

	benchBase.child = mathBenchmark

	return mathBenchmark
}

func DoStringManipilation(str string) string {
	return strings.Replace(
			strings.ToLower(
				strings.ToUpper(
					strings.Replace(
						strings.Join(
							strings.Split(str, " "), "/"), "/", "_", -1))  + "AAA"), "aaa", ".", -1)
}

func (b *StringManipulation) BenchImplementation() interface{} {
	str := "the quick brown fox jumps over the lazy dog"
	str1 := ""
	var i int64
	for ; i < b.GetIterrations(); i++ {
		str1 = DoStringManipilation(str)
	}
	return str1
}