MathBenchmark = class(MathBase, function(a, writer, printToConsole)
    MathBase.init(a, writer, printToConsole)
    a.name = "MathBenchmark"
end)

function MathBenchmark:benchImplementation()
    local R = 0
    local li = 0.0
    for i=1,self.iterrations do
        R = R + self:doMath(i, li)
    end
    return R
end