require "whetstone/whetstone"

WhetstoneBenchmark = class(BenchmarkBase, function(a, writer, printToConsole)
    BenchmarkBase.init(a, writer, printToConsole)
    a.name = "WhetstoneBenchmark"
    a.ratio = 1
    a.whetstone = Whetstone(printToConsole)
end)

function WhetstoneBenchmark:benchImplementation()
    return self.whetstone:bench()
end

function WhetstoneBenchmark:warmup(aspect)

end

function WhetstoneBenchmark:populateResult(benchResult, result)
    -- dumpTable(result, 200)
    benchResult.points = result.mwips * self.ratio
    benchResult.result = result.mwips
    benchResult.units = "MWIPS"
    benchResult.output = result.output
    return benchResult
end