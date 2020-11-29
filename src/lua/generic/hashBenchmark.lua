HashBenchmark = class(HashBase, function(a, writer, printToConsole)
    HashBase.init(a, writer, printToConsole)
    a.name = "HashBenchmark"
end)

function HashBenchmark:benchImplementation()
    local result = {}
    for i=1,self.iterrations do
        result = self:doHash(i, self.strs)
    end
    return result
end