BenchmarkBase = class(function(b, writer, printToConsole)
    b.iterrations = 0
    b.printToConsole = true
    b.isParallel = false
    b.ratio = 1.0
    b.name = "BenchmarkBase"
    b.output = writer
    b.clockBefore = 0
    b.clockAfter = 0
    b.clockDiff = 0
    b.elapsed = 0
end)

BenchmarkBase.IterrationsRatio = 1;

function BenchmarkBase:bench()
    self.clockBefore = 0
    self.clockAfter = 0
    self.clockDiff = 0
    self:beforeBench()
    self.clockBefore = clock()
    local res = self:benchImplementation()
    self.clockAfter = clock()
    self.elapsed = math.floor(self.clockAfter - self.clockBefore)

    local result = self:populateResult(self:buildResult(start), res)
    self:doOutput(result)
    self:afterBench(result)
    return result
end

function BenchmarkBase:warmup(aspect)
    self.iterrations = self.iterrations * BenchmarkBase.IterrationsRatio
    tmp = self.iterrations
    self.iterrations = math.floor(self.iterrations * aspect)
    self:useConsole(false)
    self:bench()
    self:useConsole(true)
    self.iterrations = tmp
end

function BenchmarkBase:beforeBench()
end

function BenchmarkBase:afterBench(result)
end

function BenchmarkBase:useConsole(printToConsole)
    self.printToConsole = printToConsole
    self.output.useConsole = printToConsole
end

function BenchmarkBase:doOutput(result)
    if not result.output then
        return
    end
    stream = io.open(self.name .. ".log", "a")
    stream:write(result.output)
    stream:close()
end

function BenchmarkBase:benchImplementation()
end

function BenchmarkBase:buildResult(start)
    local tElapsed = 0

    if self.elapsed == 0 then
        tElapsed = 1000
    else
        tElapsed = self.elapsed
    end

    local elapsedSeconds = tElapsed / 1000

    return {
        benchmarkName = self.name,
        elapsed = tElapsed,
        points = self.iterrations / tElapsed * self.ratio,
        result = self.iterrations / elapsedSeconds,
        units = "Iter/s",
        iterrations = self.iterrations,
        ratio = self.ratio
    }
end

function BenchmarkBase:populateResult(benchResult, result)
    if isArray(result) then
    end
    return benchResult
end
