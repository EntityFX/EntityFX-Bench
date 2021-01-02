package generic

import "../utils"

import ( 
    "strings"
    "time"
    "reflect"
) 

var IterrationsRatio float64 = 1.0;

func makeTimestamp() int64 {
    t := time.Now()
    return int64(time.Nanosecond) * t.UnixNano() / int64(time.Millisecond)
}

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

    SetIterrations(iterrations int64)
    GetIterrations() int64

    SetPrintToConsole(printToConsole bool)
    GetPrintToConsole() bool

    SetWriter(writer utils.WriterType)
    GetWriter() utils.WriterType

    PopulateResult(benchResult *BenchResult, dhrystoneResult interface{}) *BenchResult

    BeforeBench()
    
    AfterBench(result *BenchResult)

    BuildResult(start int64) *BenchResult

    DoOutput(result *BenchResult)

    SetRatio(ratio float64)

    SetName(name string)
    GetName() string
}

type BenchmarkBaseBase struct {
    Iterrations int64
    printToConsole bool
    IterrationsRatio float64
    Ratio float64
    Name string
    output utils.WriterType
}

func NewBenchmarkBase(writer utils.WriterType, printToConsole bool) BenchmarkInterface {
    var benchBase BenchmarkInterface = &BenchmarkBaseBase{}
    benchBase.SetPrintToConsole(printToConsole)
    benchBase.SetWriter(writer)
    if (writer == nil) {
        benchBase.SetWriter(utils.NewWriter(""))
    }

    return benchBase
}

func (b *BenchmarkBaseBase) BeforeBench() {
}

func (b *BenchmarkBaseBase) AfterBench(result *BenchResult) {
}

func (b *BenchmarkBaseBase) BenchImplementation() interface{} {
    return nil
}

func  (b *BenchmarkBaseBase) PopulateResult(benchResult *BenchResult, dhrystoneResult interface{}) *BenchResult {
    // switch v := dhrystoneResult.(type) { 
    // default:

    // case []BenchResult:
    // } 

    return benchResult
}


func (b *BenchmarkBaseBase) BuildResult(start int64) *BenchResult {
    elapsed := makeTimestamp() - start
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

func Bench(b BenchmarkInterface) *BenchResult {

    var name string
    if t := reflect.TypeOf(b); t.Kind() == reflect.Ptr {
        name = t.Elem().Name()
    } else {
        name = t.Name()
    }
    b.SetName(name)

    b.BeforeBench()
    start := makeTimestamp()
    res := b.BenchImplementation()
    result := b.PopulateResult(b.BuildResult(start), res);
    b.DoOutput(result)
    b.AfterBench(result)
    return result
}

func Warmup(b BenchmarkInterface, aspect float64) {
    b.SetIterrations(int64(float64(b.GetIterrations()) * IterrationsRatio))
    tmp := b.GetIterrations()
    b.SetIterrations(int64(float64(b.GetIterrations()) * aspect))
    Bench(b)
    b.SetIterrations(tmp)
}

func (b *BenchmarkBaseBase) SetIterrations(iterrations int64) {
    b.Iterrations = iterrations
}

func (b *BenchmarkBaseBase) GetIterrations() int64 {
    return b.Iterrations
}

func (b *BenchmarkBaseBase) SetPrintToConsole(printToConsole bool) {
    b.printToConsole = printToConsole
}

func (b *BenchmarkBaseBase) GetPrintToConsole() bool {
    return b.printToConsole
}

func (b *BenchmarkBaseBase) SetWriter(writer utils.WriterType) {
    b.output = writer
}

func (b *BenchmarkBaseBase) GetWriter() utils.WriterType {
    return b.output
}

func (b *BenchmarkBaseBase) SetRatio(ratio float64) {
    b.Ratio = ratio
}

func (b *BenchmarkBaseBase) SetName(name string) {
    b.Name = name
}

func (b *BenchmarkBaseBase) GetName() string {
    return b.Name
}