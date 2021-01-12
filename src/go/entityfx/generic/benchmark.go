package generic

import (
	"reflect"
	"runtime"
	"strings"
	"sync"

	"../utils"
)

var IterrationsRatio float64 = 1.0

type BenchResult struct {
	Elapsed       int64
	Result        float64
	Units         string
	Points        float64
	Ratio         float64
	Output        string
	BenchmarkName string
	Iterrations   int64
	IsParallel    bool
}

type BenchmarkInterface interface {
	BenchImplementation() interface{}

	GetIterrations() int64

	PopulateResult(benchResult *BenchResult, dhrystoneResult interface{}) *BenchResult

	Bench() *BenchResult

	Warmup(aspect float64)

	BeforeBench()

	AfterBench(result *BenchResult)

	BuildResult(start int64) *BenchResult

	DoOutput(result *BenchResult)

	GetName() string
}

type BenchmarkBaseBase struct {
	Iterrations      int64
	printToConsole   bool
	IterrationsRatio float64
	Ratio            float64
	Name             string
	IsParallel       bool
	Output           utils.WriterType
	Child            BenchmarkInterface
}

func NewBenchmarkBase(writer utils.WriterType, printToConsole bool) *BenchmarkBaseBase {
	var benchBase = &BenchmarkBaseBase{}
	benchBase.printToConsole = printToConsole
	benchBase.Output = writer
	if writer == nil {
		benchBase.Output = utils.NewWriter("")
	}

	return benchBase
}

func (b *BenchmarkBaseBase) BeforeBench() {
}

func (b *BenchmarkBaseBase) AfterBench(result *BenchResult) {
}

func (b *BenchmarkBaseBase) PopulateResult(benchResult *BenchResult, dhrystoneResult interface{}) *BenchResult {
	br, ok := dhrystoneResult.([]*BenchResult)
	if ok {
		return b.BuildParallelResult(benchResult, br)
	}

	return benchResult
}

func (b *BenchmarkBaseBase) BuildResult(start int64) *BenchResult {
	elapsed := utils.MakeTimestamp() - start
	var tElapsed int64 = 1
	if elapsed == 0 {
		tElapsed = 1
	} else {
		tElapsed = elapsed
	}

	var elapsedSeconds float64 = float64(tElapsed) / 1000.0
	iterrations := b.Iterrations
	ratio := b.Ratio
	return &BenchResult{
		tElapsed,
		float64(iterrations) / elapsedSeconds,
		"Iter/s",
		float64(iterrations) / float64(tElapsed) * ratio,
		ratio,
		"",
		b.Name,
		iterrations,
		false,
	}
}

func (b *BenchmarkBaseBase) BuildParallelResult(rootResult *BenchResult, results []*BenchResult) *BenchResult {
	rootResult.Points = 0.01
	rootResult.Iterrations = b.GetIterrations()
	rootResult.Ratio = b.Ratio

	for _, result := range results {
		rootResult.Points += result.Points
		rootResult.Result += result.Result
	}

	rootResult.IsParallel = b.IsParallel

	return rootResult
}

func (b *BenchmarkBaseBase) DoOutput(result *BenchResult) {
	if len(strings.TrimSpace(result.Output)) == 0 {
		return
	}
	//final FileWriter fileWriter = new FileWriter(Name + ".log");
	//final PrintWriter printWriter = new PrintWriter(fileWriter);
	//printWriter.print(result.Output);
	//printWriter.close();
}

func Warmup(b BenchmarkInterface, aspect float64) {
	b.Warmup(aspect)
}

func (b *BenchmarkBaseBase) Warmup(aspect float64) {
	var name string
	if t := reflect.TypeOf(b.Child); t.Kind() == reflect.Ptr {
		name = t.Elem().Name()
	} else {
		name = t.Name()
	}
	b.Name = name

	b.Iterrations = int64(float64(b.Iterrations) * IterrationsRatio)
	tmp := b.Iterrations
	b.Iterrations = int64(float64(b.Iterrations) * aspect)
	b.useConsole(false)
	b.Bench()
	b.useConsole(true)
	b.Iterrations = tmp
}

func (b *BenchmarkBaseBase) BenchImplementation() interface{} {
	return nil
}

func Bench(b BenchmarkInterface) *BenchResult {
	return b.Bench()
}

func (b *BenchmarkBaseBase) Bench() *BenchResult {
	b.BeforeBench()
	start := utils.MakeTimestamp()
	res := b.Child.BenchImplementation()
	result := b.Child.PopulateResult(b.BuildResult(start), res)
	b.DoOutput(result)
	b.AfterBench(result)
	return result
}

func (b *BenchmarkBaseBase) GetIterrations() int64 {
	return b.Iterrations
}

func (b *BenchmarkBaseBase) GetName() string {
	return b.Name
}

func (b *BenchmarkBaseBase) useConsole(value bool) {
	b.printToConsole = value
	b.Output.UseConsole(value)
}

func (b *BenchmarkBaseBase) BenchInParallel(buildFunc func() interface{}, benchFunc func(interface{}) interface{}, setBenchResultFunc func(interface{}, *BenchResult)) []*BenchResult {
	b.useConsole(false)
	count := runtime.NumCPU()
	count = 8
	benchs := make([]interface{}, count)

	for i := 0; i < count; i++ {
		benchs[i] = buildFunc()
	}

	parallelResults := runParallel(func(i int) interface{} {
		start := utils.MakeTimestamp()
		result := benchFunc(benchs[i])
		benchResult := b.BuildResult(start)
		setBenchResultFunc(result, benchResult)
		return benchResult
	}, count)

	results := make([]*BenchResult, count)

	for i := 0; i < count; i++ {
		results[i] = parallelResults[i].(*BenchResult)
	}
	b.useConsole(true)
	return results
}

func runParallel(function func(int) interface{}, t int) []interface{} {
	//runtime.GOMAXPROCS(t)
	var out []chan interface{} = make([]chan interface{}, t)
	var res []interface{} = make([]interface{}, t)
	for i := range out {
		out[i] = make(chan interface{})
	}

	var waitGroup sync.WaitGroup

	for i := 0; i < t; i++ {
		waitGroup.Add(1)
		go func(wg *sync.WaitGroup, function func(int) interface{}, c chan interface{}, i int) {
			defer wg.Done()
			c <- function(i)
		}(&waitGroup, function, out[i], i)
	}

	defer waitGroup.Wait()

	for i := range out {
		res[i] = <-out[i]
	}

	return res
}
