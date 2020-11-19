StringManipulationBase = class(BenchmarkBase, function(a, writer, printToConsole)
    BenchmarkBase.init(a, writer, printToConsole)
    a.iterrations = 5000000
    a.ratio = 10
end)

function StringManipulationBase:doStringManipilation(str)
    return string.lower(string.upper(table.concat(split(str, ' '), "/"):gsub('/', ' ')) .. "AAA"):gsub('aaa', '.')
end