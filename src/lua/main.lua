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
require "generic/randomMemoryBenchmarkBase"
require "generic/randomMemoryBenchmark"
require "generic/hashBase"
require "generic/hashBenchmark"
require "dhrystone/dhrystoneBenchmark"
require "whetstone/whetstoneBenchmark"
require "linpack/linpackBenchmark"
require "scimark2/scimark2Benchmark"

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

BenchmarkBase.IterrationsRatio = 0.1

benchmarks = {
    ArithemticsBenchmark(writer, true),
    MathBenchmark(writer, true),
    CallBenchmark(writer, true),
    IfElseBenchmark(writer, true),
    StringManipulation(writer, true),
    MemoryBenchmark(writer, true),
    RandomMemoryBenchmark(writer, true),
    Scimark2Benchmark(writer, true),
    DhrystoneBenchmark(writer, true),
    WhetstoneBenchmark(writer, true),
    LinpackBenchmark(writer, true),
    HashBenchmark(writer, true)
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
local results = {}
local units = {}

for i,benchmark in ipairs(benchmarks) do
    writer:writeHeader("[%d] %s", i, benchmark.name)
    r = benchmark:bench()
    total = total + r.elapsed
    totalPoints = totalPoints + r.points
    points[i] = r.points
    results[i] = r.result
    units[i] = r.units
    writeResult(writer, r)
end

writer:writeLine()
writer:writeTitle("%-30s", "Total:")
writer:writeValue("%15d ms", total)
writer:writeValue("%13.2f pts", totalPoints)
writer:writeLine()

local os, arch = getOS()
local osVersion = os .. ' ' .. arch

local headerCommon = "Operating System,Runtime,Threads Count,Memory Used"
local headerTotals = ",Total Points,Total Time (ms)"

writer:writeLine()
writer:writeHeader("Single-thread results")
writer:writeTitle(headerCommon)
for i,benchmark in ipairs(benchmarks) do
    writer:writeTitle(",%s", benchmark.name)
end
writer:writeTitle(headerTotals)
writer:writeLine()
writer:writeTitle("%s,%s,%d,%d", osVersion, _VERSION, 1, 0)
for i,point in ipairs(points) do
    writer:writeTitle(",%.2f", point)
end
writer:writeTitle(",%.2f,%d", totalPoints, total)
writer:writeLine()

writer:writeLine()
writer:writeHeader("Single-thread Units results")
writer:writeTitle(headerCommon)
for i,benchmark in ipairs(benchmarks) do
    writer:writeTitle(",%s (%s)", benchmark.name, units[i])
end
writer:writeTitle(headerTotals)
writer:writeLine()
writer:writeTitle("%s,%s,%d,%d", osVersion, _VERSION, 1, 0)
for i,result in ipairs(results) do
    writer:writeTitle(",%.2f", result)
end
writer:writeTitle(",%.2f,%d", totalPoints, total)
writer:writeLine()