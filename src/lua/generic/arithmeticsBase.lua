ArithmeticsBase = class(BenchmarkBase, function(a, writer, printToConsole)
    BenchmarkBase.init(a, writer, printToConsole)
    a.R = 0.0
    a.iterrations = 300000000
    a.ratio = 0.03
end)

function ArithmeticsBase:doArithmetics(i)
    return (i / 10) * (i / 100) * (i / 100) * (i / 100) * 1.11 + (i / 100) * (i / 1000) * (i / 1000) * 2.22
        - i * (i / 10000) * 3.33 + i * 5.33
end