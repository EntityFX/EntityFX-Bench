MemoryBenchmark = class(MemoryBenchmarkBase, function(a, writer, printToConsole)
    MemoryBenchmarkBase.init(a, writer, printToConsole)
    a.name = "MemoryBenchmark"
end)

function MemoryBenchmark:benchImplementation()
    return self:benchMemory()
end

function MemoryBenchmark:populateResult(benchResult, memoryBenchmarkResult)
    benchResult.points = memoryBenchmarkResult.average * self.ratio
    benchResult.result = memoryBenchmarkResult.average
    benchResult.units = "MB/s"
    benchResult.output = memoryBenchmarkResult.output
    return benchResult;
end