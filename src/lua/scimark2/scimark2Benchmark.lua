require "scimark2/scimark2"

Scimark2Benchmark = class(BenchmarkBase, function(a, writer, printToConsole)
    BenchmarkBase.init(a, writer, printToConsole)
    a.name = "Scimark2Benchmark"
    a.ratio = 10
    a.scimark2 = Scimark2(printToConsole)
end)

function Scimark2Benchmark:benchImplementation()
    return self.scimark2:bench(false)
end

function Scimark2Benchmark:warmup(aspect)

end

function Scimark2Benchmark:populateResult(benchResult, result)
    benchResult.points = result.CompositeScore * self.ratio
    benchResult.result = result.CompositeScore
    benchResult.units = "CompositeScore"
    benchResult.output = result.Output
    return benchResult
end