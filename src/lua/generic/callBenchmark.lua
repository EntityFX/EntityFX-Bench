CallBenchmark = class(CallBenchmarBase, function(a, writer, printToConsole)
    CallBenchmarBase.init(a, writer, printToConsole)
    a.name = "CallBenchmark"
end)

function CallBenchmark:benchImplementation()
    return .0
end

function CallBenchmark:bench()
    self:beforeBench()
    local start = os.clock() * 1000
    local call_time = self:doCallBench()
    local result = self:populateResult(self:buildResult(start), call_time)
    result.elapsed = os.clock() * 1000 - start
    self:doOutput(result)
    self:afterBench(result)
    return result
end