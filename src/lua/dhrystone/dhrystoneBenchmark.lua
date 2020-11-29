require "dhrystone/dhrystone"

DhrystoneBenchmark = class(BenchmarkBase, function(a, writer, printToConsole)
    BenchmarkBase.init(a, writer, printToConsole)
    a.name = "DhrystoneBenchmark"
    a.ratio = 4
    a.dhrystone = Dhrystone2(false)
end)

function DhrystoneBenchmark:benchImplementation()
    return self.dhrystone:bench()
end

function DhrystoneBenchmark:warmup(aspect)

end

function DhrystoneBenchmark:populateResult(benchResult, result)
    -- dumpTable(result, 200)
    benchResult.points = result.VaxMips * self.ratio
    benchResult.result = result.VaxMips
    benchResult.units = "DMIPS"
    benchResult.output = result.Output
    return benchResult
end