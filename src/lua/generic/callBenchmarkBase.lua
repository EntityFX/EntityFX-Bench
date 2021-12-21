CallBenchmarBase = class(BenchmarkBase, function(a, writer, printToConsole)
    BenchmarkBase.init(a, writer, printToConsole)
    a.iterrations = 2000000000
    a.ratio = 0.01
end)

function CallBenchmarBase:doCall(i, b)
    local z = i * 0.7
    local z1 = i * b
    return  z + z1 + 0.5
end

function CallBenchmarBase:doCallBench()
    local start = os.clock()
    local elapsed1 = 0
    local elapsed2 = 0
    local i = 0
    local a = 0.0

    for i=1,self.iterrations do
        local z = a * 0.7
        local z1 = a * 0.01
        a = z + z1 + 0.5;
    end

    elapsed1 = (os.clock() - start) * 1000
    a = 0.0;
    i = 0;
    start = os.clock()
    for i=1,self.iterrations do
        a = self:doCall(a, 0.01)
    end
    elapsed2 = (os.clock() - start) * 1000

    self.output:write("Elapsed No Call: %d", math.floor(elapsed1))
    self.output:writeLine()
    self.output:write("Elapsed Call: %d", math.floor(elapsed2))
    self.output:writeLine()
    local callTime = 0

    if (elapsed2 <= elapsed1) then
        callTime = elapsed1 - elapsed2
    else
        callTime = elapsed2 - elapsed1
    end
    self.output:writeLine("Call time: %d", math.floor(callTime))
    return { callTime = callTime / 1000, a = a }
end
