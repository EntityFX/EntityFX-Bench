require "utils"
require "writer"
require "benchmarkBase"
require "generic/arithmeticsBase"
require "generic/arithmeticsBenchmark"
require "generic/mathBase"
require "generic/mathBenchmark"
require "generic/callBenchmarkBase"
require "generic/callBenchmark"
require "generic/ifElseBenchmark"
require "generic/stringManipulationBase"
require "generic/stringManipulation"
require "generic/memoryBenchmarkBase"
require "generic/memoryBenchmark"
require "dhrystone/dhrystone"
require "whetstone/whetstone"

function writeResult(writer, benchResult)
    writer:writeTitle("%-30s", benchResult.benchmarkName)
    writer:writeValue("%15d ms", benchResult.elapsed)
    writer:writeValue("%13.2f pts", benchResult.points)
    writer:writeValue("%15.2f %s", benchResult.result, benchResult.units)
    writer:writeLine()
    writer:writeValue("Iterrations: %15d, Ratio: %15f", benchResult.iterrations, benchResult.ratio)
    writer:writeLine()
end

local writer = Writer("Output.log")

-- local w = Whetstone(writer)
-- w:bench(0)

BenchmarkBase.IterrationsRatio = 0.1;

benchmarks = { 
    ArithemticsBenchmark(writer, true),
    MathBenchmark(writer, true),
    CallBenchmark(writer, true),
    IfElseBenchmark(writer, true),
    StringManipulation(writer, true),
    MemoryBenchmark(writer, true)
}

writer:writeHeader("Warmup")

for i,benchmark in ipairs(benchmarks) do
    benchmark:warmup(0.05)
    writer:write(".");
end

writer:writeLine();
writer:writeHeader("Bench");

local total = 0
local totalPoints = 0
local points = {}

for i,benchmark in ipairs(benchmarks) do
    writer:writeHeader("[%d] %s", i, benchmark.name)
    r = benchmark:bench()
    total = total + r.elapsed
    totalPoints = totalPoints + r.points
    points[i] = string.format("%.2f", r.points)
    writeResult(writer, r)
end