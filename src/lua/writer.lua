Writer = class(function(w,filePath)
    w.filePath = filePath or nil
    w.useConsole = true
    w.useFile = false
    w.output = ""
    w.isColor = true


    if filePath then
        w.stream = io.open(filePath, "a")
        w.useFile = true
    end

end)

function Writer:writeLine(format, ...)
    if format then
        self:writeColor("\27[1;30m", format, ...)
    end

    if self.useConsole then
        io.write("\n")
        self.output = self.output .. "\n"
        if self.useFile then
            self.stream:write("\n")
        end
    end
end

function Writer:writeColor(color, format, ...)
    local formatted = string.format(format, ...)
    self.output = self.output .. formatted

    if self.useConsole then
        if self.isColor then 
            toPrint = color .. formatted .. "\27[0m" 
        else 
            toPrint = formatted 
        end
        io.write(toPrint)
        if self.useFile then
            self.stream:write(formatted)
        end
    end
end

function Writer:write(format, ...)
    self:writeColor("\27[1;30m", format, ...)
end

function Writer:writeHeader(format, ...)
    self:writeColor("\27[1;36m", format, ...)  
    self:writeLine()
end

function Writer:writeValue(format, ...)
    self:writeColor("\27[1;32m", format, ...)  
end

function Writer:writeTitle(format, ...)
    self:writeColor("\27[1;37m", format, ...)  
end