RandomMemoryBenchmarkBase = class(BenchmarkBase, function(a, writer, printToConsole)
    BenchmarkBase.init(a, writer, printToConsole)
    a.iterrations = 500000
    a.ratio = 2
    a.beforeClockMemory = 0
    a.afterClockMemory = 0
    a.elapsedClockMemory = 0
end)

function RandomMemoryBenchmarkBase:benchRandomMemory()
    self.beforeClockMemory = 0
    self.afterClockMemory = 0
    self.elapsedClockMemory = 0

    local int4k = self:measureArrayRandomRead(1024)
    self.output:writeLine("Random int 4k: %.2f MB/s", int4k.mbPerSec)
    local int512k = self:measureArrayRandomRead(131072)
    self.output:writeLine("Random int 512k: %.2f MB/s", int512k.mbPerSec)
    local int8m = self:measureArrayRandomRead(2097152)
    self.output:writeLine("Random int 8M: %.2f MB/s", int8m.mbPerSec)

    local long4k = self:measureArrayRandomRead(1024)
    self.output:writeLine("Random long 4k: %.2f MB/s", long4k.mbPerSec)
    local long512k = self:measureArrayRandomRead(131072)
    self.output:writeLine("Random long 512k: %.2f MB/s", long512k.mbPerSec)
    local long8m = self:measureArrayRandomRead(2097152)
    self.output:writeLine("Random long 8M: %.2f MB/s", long8m.mbPerSec)
    
    local avg = arithmetic_mean({
        int4k.mbPerSec, int512k.mbPerSec, int8m.mbPerSec, 
        long4k.mbPerSec, long512k.mbPerSec, long8m.mbPerSec})

    self.output:writeLine("Average: %.2f MB/s", avg)

    return {
        average = avg,
        output = self.output.output
    }
end

function RandomMemoryBenchmarkBase:measureArrayRandomRead(size)
    local I = 0

    local array = randomIntArray(size, 2^31 - 2)

    local endA = #array - 1
    local indexes = randomIntArray(endA, endA)
    local k0 = (size / 1024)

    local k1 = 1
    if k0 == 0 then k1 = 1 else k1 = k0 end
    local iterInternal = self.iterrations / k1
    if iterInternal == 0 then iterInternal = 1 end
    for idx=0,endA do
        I = array[idx]
    end

    self.beforeClockMemory = clock()
    for i=0,iterInternal-1 do
        for key,idx in ipairs(indexes) 
        do
            I = array[idx]
        end
    end

    self.afterClockMemory = clock()
    self.elapsedClockMemory = self.afterClockMemory - self.beforeClockMemory
    if (self.elapsedClockMemory == 0) then
        self.elapsedClockMemory = 1
    end

    local elapsed = math.floor(self.elapsedClockMemory)
    return { mbPerSec = (iterInternal * #array * 8.0 / (elapsed / 1000.0) / 1024 / 1024), res = I }
end
