RandomMemoryBenchmark = class(RandomMemoryBenchmarkBase, function(a, writer, printToConsole)
    RandomMemoryBenchmarkBase.init(a, writer, printToConsole)
    a.name = "RandomMemoryBenchmark"
end)

function RandomMemoryBenchmark:benchImplementation()
    return self:benchRandomMemory()
end

function RandomMemoryBenchmark:populateResult(benchResult, memoryBenchmarkResult)
    benchResult.points = memoryBenchmarkResult.average * self.ratio
    benchResult.result = memoryBenchmarkResult.average
    benchResult.units = "MB/s"
    benchResult.output = memoryBenchmarkResult.output
    return benchResult;
end