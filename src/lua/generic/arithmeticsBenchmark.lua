ArithemticsBenchmark = class(ArithmeticsBase, function(a, writer, printToConsole)
    ArithmeticsBase.init(a, writer, printToConsole)
    a.name = "ArithemticsBenchmark"
end)

function ArithemticsBenchmark:benchImplementation()
    local R = 0
    for i=1,self.iterrations do
        R = R + self:doArithmetics(i)
    end
    return R
end