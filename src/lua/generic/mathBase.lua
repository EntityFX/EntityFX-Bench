MathBase = class(BenchmarkBase, function(a, writer, printToConsole)
    BenchmarkBase.init(a, writer, printToConsole)
    a.iterrations = 200000000
    a.ratio = 0.5
end)

function MathBase:doMath(i, li)
    local rev = 1.0 / (i + 1.0)
    return math.abs(i) * math.acos(rev) * math.asin(rev) * math.atan(rev) +
        math.floor(li) + math.exp(rev) * math.cos(i) * math.sin(i) * math.pi + math.sqrt(i)

end