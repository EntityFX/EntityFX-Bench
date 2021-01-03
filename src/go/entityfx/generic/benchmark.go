package generic

import "../utils"

import ( 
    "strings"
    "reflect"
) 

var IterrationsRatio float64 = 1.0

type BenchResult struct	{
    Elapsed int64
    Result float64
    Units string
    Points float64
    Ratio float64
    Output string
    BenchmarkName string
    Iterrations int64
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
    Iterrations int64
    printToConsole bool
    IterrationsRatio float64
    Ratio float64
    Name string
    output utils.WriterType
    child BenchmarkInterface
}

func NewBenchmarkBase(writer utils.WriterType, printToConsole bool) *BenchmarkBaseBase {
    var benchBase = &BenchmarkBaseBase{}
    benchBase.printToConsole = printToConsole
    benchBase.output = writer
    if (writer == nil) {
        benchBase.output  = utils.NewWriter("")
    }

    return benchBase
}

func (b *BenchmarkBaseBase) BeforeBench() {
}

func (b *BenchmarkBaseBase) AfterBench(result *BenchResult) {
}

func  (b *BenchmarkBaseBase) PopulateResult(benchResult *BenchResult, dhrystoneResult interface{}) *BenchResult {
    // switch v := dhrystoneResult.(type) { 
    // default:

    // case []BenchResult:
    // } 

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
            iterrations}
}

func (b *BenchmarkBaseBase) DoOutput(result *BenchResult) {
    if (len(strings.TrimSpace(result.Output)) == 0) {
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
    if t := reflect.TypeOf(b.child); t.Kind() == reflect.Ptr {
        name = t.Elem().Name()
    } else {
        name = t.Name()
    }
    b.Name = name

    b.Iterrations = int64(float64(b.Iterrations) * IterrationsRatio)
    tmp := b.Iterrations
    b.Iterrations = int64(float64(b.Iterrations) * aspect)
    b.Bench()
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
    res := b.child.BenchImplementation()
    result := b.child.PopulateResult(b.BuildResult(start), res);
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