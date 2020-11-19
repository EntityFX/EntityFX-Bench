StringManipulation = class(StringManipulationBase, function(a, writer, printToConsole)
    StringManipulationBase.init(a, writer, printToConsole)
    a.name = "StringManipulation"
end)

function StringManipulation:benchImplementation()
    local str = "the quick brown fox jumps over the lazy dog"
    local str1 = ""
    for i=1,self.iterrations do
        str1 = self:doStringManipilation(str)
    end
    return str1
end