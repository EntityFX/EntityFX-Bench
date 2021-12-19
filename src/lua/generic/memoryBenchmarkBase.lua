MemoryBenchmarkBase = class(BenchmarkBase, function(a, writer, printToConsole)
    BenchmarkBase.init(a, writer, printToConsole)
    a.iterrations = 500000
    a.ratio = 1
end)

function MemoryBenchmarkBase:benchRandomMemory()
    local int4k = self:measureArrayRandomRead(1024)
    self.output:writeLine("int 4k: %.2f MB/s", int4k.mbPerSec)
    local int512k = self:measureArrayRandomRead(131072)
    self.output:writeLine("int 512k: %.2f MB/s", int512k.mbPerSec)
    local int8m = self:measureArrayRandomRead(2097152)
    self.output:writeLine("int 8M: %.2f MB/s", int8m.mbPerSec)
    local int32m = self:measureArrayRandomRead(32 * 1024 * 1024 / 4)
    self.output:writeLine("int 32M: %.2f MB/s", int32m.mbPerSec)

    local long4k = self:measureArrayRandomLongRead(1024)
    self.output:writeLine("long 4k: %.2f MB/s", long4k.mbPerSec)
    local long512k = self:measureArrayRandomLongRead(131072)
    self.output:writeLine("long 512k: %.2f MB/s", long512k.mbPerSec)
    local long8m = self:measureArrayRandomLongRead(2097152)
    self.output:writeLine("long 8M: %.2f MB/s", long8m.mbPerSec)
    local long32m = self:measureArrayRandomLongRead(32 * 1024 * 1024 / 8)
    self.output:writeLine("long 32M: %.2f MB/s", long32m.mbPerSec)
    
    local avg = arithmetic_mean({
        int4k.mbPerSec, int512k.mbPerSec, int8m.mbPerSec, 
        int32m.mbPerSec, long4k.mbPerSec, long512k.mbPerSec,
        long8m.mbPerSec, long32m.mbPerSec})

    self.output:writeLine("Average: %.2f MB/s", avg)

    return {
        average = avg,
        output = self.output.output
    }
end

function MemoryBenchmarkBase:measureArrayRandomRead(size)
    local blockSize = 16
    local I = {}

    local array = randomIntArray(size, 2^31 - 2)

    local endA = #array - 1
    local k0 = (size / 1024)

    local k1 = 1
    if k0 == 0 then k1 = 1 else k1 = k0 end
    local iterInternal = self.iterrations / k1
    if iterInternal == 0 then iterInternal = 1 end
    for idx=0,endA,blockSize do
        I[0] = array[idx]
        I[1] = array[idx + 1]
        I[2] = array[idx + 2]
        I[3] = array[idx + 3]
        I[4] = array[idx + 4]
        I[5] = array[idx + 5]
        I[6] = array[idx + 6]
        I[7] = array[idx + 7]
        I[8] = array[idx + 8]
        I[9] = array[idx + 9]
        I[10] = array[idx + 10]
        I[11] = array[idx + 11]
        I[12] = array[idx + 12]
        I[13] = array[idx + 13]
        I[14] = array[idx + 14]
        I[15] = array[idx + 15]
    end
    local start = os.clock() * 1000
    for i=0,iterInternal-1 do
        for idx=0,endA,blockSize do
            I[0] = array[idx]
            I[1] = array[idx + 1]
            I[2] = array[idx + 2]
            I[3] = array[idx + 3]
            I[4] = array[idx + 4]
            I[5] = array[idx + 5]
            I[6] = array[idx + 6]
            I[7] = array[idx + 7]
            I[8] = array[idx + 8]
            I[9] = array[idx + 9]
            I[10] = array[idx + 10]
            I[11] = array[idx + 11]
            I[12] = array[idx + 12]
            I[13] = array[idx + 13]
            I[14] = array[idx + 14]
            I[15] = array[idx + 15]
        end
    end
    local elapsed = math.floor(os.clock() * 1000 - start)
    return { mbPerSec = (iterInternal * #array * 8.0 / (elapsed / 1000.0) / 1024 / 1024), res = I }
end

function MemoryBenchmarkBase:measureArrayRandomLongRead(size)
    local blockSize = 8
    local I = {}

    local array = randomIntArray(size, 2^31 - 2)

    local endA = #array - 1
    local k0 = (size / 1024)

    local k1 = 1
    if k0 == 0 then k1 = 1 else k1 = k0 end
    local iterInternal = self.iterrations / k1
    if iterInternal == 0 then iterInternal = 1 end
    for idx=0,endA,blockSize do
        I[0] = array[idx]
        I[1] = array[idx + 1]
        I[2] = array[idx + 2]
        I[3] = array[idx + 3]
        I[4] = array[idx + 4]
        I[5] = array[idx + 5]
        I[6] = array[idx + 6]
        I[7] = array[idx + 7]
    end
    local start = os.clock() * 1000
    for i=0,iterInternal-1 do
        for idx=0,endA,blockSize do
            I[0] = array[idx]
            I[1] = array[idx + 1]
            I[2] = array[idx + 2]
            I[3] = array[idx + 3]
            I[4] = array[idx + 4]
            I[5] = array[idx + 5]
            I[6] = array[idx + 6]
            I[7] = array[idx + 7]
        end
    end
    local elapsed = math.floor(os.clock() * 1000 - start)
    return { mbPerSec = (iterInternal * #array * 8.0 / (elapsed / 1000.0) / 1024 / 1024), res = I }
end
