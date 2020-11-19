IfElseBenchmark = class(BenchmarkBase, function(a, writer, printToConsole)
    BenchmarkBase.init(a, writer, printToConsole)
    a.name = "IfElseBenchmark"
    a.iterrations = 2000000000
    a.ratio = 0.01
end)

function IfElseBenchmark:benchImplementation()
    local d = 0
    local c = -1
    for i=1,self.iterrations do
        c = (c == -4 and -1 or c)
        if i == -1 then
            d = 3
        elseif i == -2 then
            d = 2
        elseif i == -3 then
            d = 1
        end
        d = d + 1
        c = c - 1
    end
    return d
end