CallBenchmark = class(CallBenchmarBase, function(a, writer, printToConsole)
    CallBenchmarBase.init(a, writer, printToConsole)
    a.name = "CallBenchmark"
end)

function CallBenchmark:benchImplementation()
    return .0
end

function CallBenchmark:bench()
    self:beforeBench()
    local call_time = self:doCallBench()
    local result = self:populateResult(self:buildResult(call_time.callTime), call_time)
    self:doOutput(result)
    self:afterBench(result)
    return result
end
