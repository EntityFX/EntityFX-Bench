------------------------------------------------------------------------------
-- Lua SciMark (2010-12-20).
--
-- A literal translation of SciMark 2.0a, written in Java and C.
-- Credits go to the original authors Roldan Pozo and Bruce Miller.
-- See: http://math.nist.gov/scimark2/
------------------------------------------------------------------------------
-- Copyright (C) 2006-2010 Mike Pall. All rights reserved.
--
-- Permission is hereby granted, free of charge, to any person obtaining
-- a copy of this software and associated documentation files (the
-- "Software"), to deal in the Software without restriction, including
-- without limitation the rights to use, copy, modify, merge, publish,
-- distribute, sublicense, and/or sell copies of the Software, and to
-- permit persons to whom the Software is furnished to do so, subject to
-- the following conditions:
--
-- The above copyright notice and this permission notice shall be
-- included in all copies or substantial portions of the Software.
--
-- THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
-- EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
-- MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
-- IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
-- CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
-- TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
-- SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
--
-- [ MIT license: http://www.opensource.org/licenses/mit-license.php ]
------------------------------------------------------------------------------

require "scimark2/FFT"
require "scimark2/SOR"
require "scimark2/MonteCarlo"
require "scimark2/SparseCompRow"
require "scimark2/LU"
require "scimark2/Kernel"
require "scimark2/Constants"


Scimark2 = class(function(w, printToConsole)
    w.output = Writer(nil)
    w.output.useConsole = printToConsole
end)

function Scimark2:bench(isLarge)
    self.output:writeLine("Lua SciMark %s based on SciMark 2.0a. %s.\n",
    SCIMARK_VERSION, SCIMARK_COPYRIGHT)

    local params = constants[SIZE_SELECT]
    local res = {}

    res[1] = self:measure(MIN_TIME, "FFT", unpack(params["FFT"]))
    res[2] = self:measure(MIN_TIME, "SOR", unpack(params["SOR"]))
    res[3] = self:measure(MIN_TIME, "MC", unpack(params["MC"]))
    res[4] = self:measure(MIN_TIME, "SPARSE", unpack(params["SPARSE"]))
    res[5] = self:measure(MIN_TIME, "LU", unpack(params["LU"]))

    local sum = res[1] + res[2] + res[3] + res[4] + res[5]
    local compositeScore = sum / #constants
    self.output:writeLine("Composite Score: %.2f", compositeScore) 

    self.output:write("FFT (%d): ", params["FFT"][1])

    if (res[1] == .0) then
        self.output:writeLine(" ERROR, INVALID NUMERICAL RESULT!")
    else
        self.output:writeLine("%.2f", res[1])
    end

    self.output:writeLine("SOR (%dx%d):   %.2f", params["SOR"][1], params["SOR"][1], res[2])
    self.output:writeLine("Monte Carlo : %.2f", res[3])
    self.output:writeLine("Sparse matmult (N=%d, nz=%d): %.2f", params["SPARSE"][1], params["SPARSE"][2], res[4])
    self.output:write("LU (%dx%d): ", params["LU"][1], params["LU"][1])

    if (res[5] == .0) then
        self.output:writeLine(" ERROR, INVALID NUMERICAL RESULT!")
    else
        self.output:writeLine("%.2f", res[5])
    end

    return {
        CompositeScore = compositeScore,
        FFT = res[1],
        SOR = res[2],
        MonteCarlo = res[3],
        SparseMathmult = res[4],
        LU = res[5],
        Output = self.output.output
    }
end

function Scimark2:measure(min_time, name, ...)
    local clockBefore = 0
    local clockAfter = 0
    local tm = 0
    math.randomseed( os.time() )
    local run = Kernel[name](...)
    local cycles = 1
    repeat
        clockBefore = clock() / 1000.0
        local flops = run(cycles, ...)
        clockAfter = clock() / 1000.0
        tm = clockAfter - clockBefore

        if (tm == 0) then
            tm = 1
        end

        if tm >= min_time then
            local res = flops / tm * 1.0e-6
            local p1, p2 = ...
            return res
        end
        cycles = cycles * 2
    until false
end
