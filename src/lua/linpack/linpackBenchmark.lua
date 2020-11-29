require "linpack/linpack"

LinpackBenchmark = class(BenchmarkBase, function(a, writer, printToConsole)
    BenchmarkBase.init(a, writer, printToConsole)
    a.name = "LinpackBenchmark"
    a.ratio = 10
    a.linpack = Linpack(printToConsole)
end)

function LinpackBenchmark:benchImplementation()
    return self.linpack:run_benchmark(1000)
end

function LinpackBenchmark:warmup(aspect)

end

function LinpackBenchmark:populateResult(benchResult, result)
    benchResult.points = result.mflops * self.ratio
    benchResult.result = result.mflops
    benchResult.units = "MFLOPS"
    benchResult.output = result.Output
    return benchResult
end