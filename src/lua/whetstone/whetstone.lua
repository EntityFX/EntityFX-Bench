Whetstone = class(function(w, printToConsole)
    w.output = Writer(nil)
    w.output.useConsole = printToConsole
    w.loop_time = {}
    w.loop_mops = {}
    w.loop_mflops = {}
    w.timeUsed = 0.0
    w.mwips = 0.0
    w.headings = {}
    w.check = 0.0
    w.results = {}
end)

function Whetstone:bench(getinput)
    local count = 10
    local calibrate = 1
    local xtra = 1
    local endit = 0
    local section = 0
    local x100 = 100
    local duration = 100
    local general = {}
    self:writeLine("%s Precision Lua Whetstone Benchmark\n", "Double")
    if not getinput then
        self:writeLine("No run time input data\n")
    else
        self:writeLine("With run time input data\n")
    end
    self:writeLine("Calibrate")
    repeat
        self.timeUsed = 0.0
        self:whetstones(xtra, x100, calibrate)
        self:writeLine("%11.2f Seconds %10d   Passes (x 100)", self.timeUsed, xtra)
        calibrate = calibrate + 1
        count = count - 1
        if self.timeUsed > 2.0 then
            count = 0
        else
            xtra = xtra * 5
        end
    until count == 0
    if self.timeUsed > 0 then
        xtra =  duration * xtra / self.timeUsed
    end
    if xtra < 1 then
        xtra = 1
    end
    calibrate = 0
    self:writeLine("\nUse %.4f  passes (x 100)", xtra)
    self:writeLine("\n          %s Precision Lua Whetstone Benchmark", "Double")
    self:writeLine("\n                  %s", "")
    self:writeLine("\nLoop content                  Result              MFLOPS " .. "     MOPS   Seconds\n")
    self.timeUsed = 0.0
    self:whetstones(xtra, x100, calibrate)
    self.output:write("MWIPS            ")
    if self.timeUsed > 0 then
        self.mwips = (xtra * x100) / (10.0 * self.timeUsed)
    else
        self.mwips = 0.0
    end
    self.output:writeLine("%39.3f%19.3f", self.mwips, self.timeUsed)
    if self.check == 0 then
        self:writeLine("Wrong answer  ")
    end
    return {
        output = self.output.output,
        mwips = self.mwips,
        timeUsed = self.timeUsed
    }
end

function Whetstone:whetstones(xtra, x100, calibrate)
    local n1 = 0
    local n2 = 0
    local n3 = 0
    local n4 = 0
    local n5 = 0
    local n6 = 0
    local n7 = 0
    local n8 = 0
    local i = 0
    local ix = 0
    local n1mult = 0
    local x = 0.0
    local y = 0.0
    local z = 0.0
    local j = 0
    local k = 0
    local l = 0
    local e1 = {}
    local timea = 0.0
    local timeb = 0.0
    local t = 0.49999975
    local t0 = t
    local t1 = 0.50000025
    local t2 = 2.0
    self.check = 0.0
    n1 = 12 * x100
    n2 = 14 * x100
    n3 = 345 * x100
    n4 = 210 * x100
    n5 = 32 * x100
    n6 = 899 * x100
    n7 = 616 * x100
    n8 = 93 * x100
    n1mult = 10
    e1[0] = 1.0
    e1[1] = -1.0
    e1[2] = -1.0
    e1[3] = -1.0
    local start = os.clock()
    xtra1 = xtra - 1

    timea = start
    for ix = 0, xtra1 do 
        for i = 0, (n1 * n1mult) - 1  do
            e1[0] = (((e1[0] + e1[1] + e1[2]) - e1[3])) * t
            e1[1] = ((((e1[0] + e1[1]) - e1[2]) + e1[3])) * t
            e1[2] = (((e1[0] - e1[1]) + e1[2] + e1[3])) * t
            e1[3] = ((((-e1[0]) + e1[1] + e1[2]) + e1[3])) * t
        end
        t = 1.0 - t
    end
    t = t0
    timeb = (os.clock() - timea) / n1mult
    self:pout("N1 floating point", n1 * 16 * xtra, 1, e1[3], timeb, calibrate, 1)
    
    timea = start
    for ix = 0, xtra1 do
        for i = 0, n2 - 1 do
           e1 = self:pa(e1, t, t2)
        end
        t = 1.0 - t
    end
    t = t0
    timeb = os.clock() - timea
    self:pout("N2 floating point", n2 * 96 * xtra, 1, e1[3], timeb, calibrate, 2)

    j = 1
    timea = start
    for ix = 0, xtra1 do
        for i = 0, n3 - 1 do
            if j == 1 then
                j = 2
            else
                j = 3
            end
            if j > 2 then
                j = 0
            else
                j = 1
            end
            if j < 1 then
                j = 1
            else
                j = 0
            end
        end
    end
    timeb = os.clock() - timea
    self:pout("N3 if then else  ", n3 * 3 * xtra, 2, j, timeb, calibrate,3)

    j = 1
    k = 2
    l = 3
    timea = start
    for ix = 0, xtra1 do
        for i = 0,  n4 - 1 do
            j = j * ((k - j)) * ((l - k))
            k = (l * k) - (((l - j)) * k)
            l = ((l - k)) * ((k + j))
            e1[l - 2] =  j + k + l
            e1[k - 2] =  j * k * l
        end
    end
    timeb = os.clock() - timea
    x = e1[0] + e1[1]
    self:pout("N4 fixed point   ", n4 * 15 * xtra, 2, x, timeb, calibrate, 4)
   
    x = .5
    y = .5
    timea = start
    for ix = 0, xtra1 do
        for i = 1, n5 - 1 do
            x =  (((t) * math.atan(((t2) * math.sin(x) * math.cos(x))
                    / (((math.cos((x + y)) + math.cos((x - y))) - 1.0)))))
            y =  (((t) * math.atan(((t2) * math.sin(y) * math.cos(y))
                    / (((math.cos((x + y)) + math.cos((x - y))) - 1.0)))))
        end
        t = 1.0 - t
    end
    t = t0
    timeb = os.clock() - timea
    self:pout("N5 sin,cos etc.  ", n5 * 26 * xtra, 2, y, timeb, calibrate, 5)

    x = 1.0
    y = 1.0
    z = 1.0
    timea = start
    for ix = 0, xtra1 do
        for i = 0, n6 - 1 do
            wrapx2 = x
            wrapy3 = y
            wrapz4 = z
            r = self:p3(wrapx2, wrapy3, wrapz4, t, t1, t2)
            wrapx2 = r.x
            wrapy3 = r.y
            wrapz4 = r.z
            if wrapx2 ~= nil then
                x = wrapx2
            else
                x = 0
            end
            if wrapy3 ~= nil then
                y = wrapy3
            else
                y = 0
            end
            if wrapz4 ~= nil then
                z = wrapz4
            else
                z = 0
            end
        end
    end
    timeb = os.clock() - timea
    self:pout("N6 floating point", n6 * 6 * xtra, 1, z, timeb, calibrate, 6)
    
    j = 0
    k = 1
    l = 2
    e1[0] = 1.0
    e1[1] = 2.0
    e1[2] = 3.0
    timea = start
    for ix = 0, xtra1 do
        for i = 0, n7 - 1 do
            el = self:po(e1, j, k, l)
        end
    end
    timeb = os.clock() - timea
    self:pout("N7 assignments   ", n7 * 3 * xtra, 2, e1[2], timeb, calibrate, 7)
    
    x = 0.75
    timea = start
    for ix = 0, xtra1 do
        for i = 0, n8 - 1 do
            x =  math.sqrt(math.exp(math.log(x) / (t1)))
        end
    end
    timeb = os.clock() - timea
    self:pout("N8 exp,sqrt etc. ", n8 * 4 * xtra, 2, x, timeb, calibrate, 8)
end

function Whetstone:pa(e, t, t2)
    local j = 0
    for j = 0, 5 do
        e[0] = (((e[0] + e[1] + e[2]) - e[3])) * t
        e[1] = ((((e[0] + e[1]) - e[2]) + e[3])) * t
        e[2] = (((e[0] - e[1]) + e[2] + e[3])) * t
        e[3] = ((((-e[0]) + e[1] + e[2]) + e[3])) / t2
    end
    return e
end

function Whetstone:po(e1, j, k, l)
    e1[j] = e1[ k]
    e1[k] = e1[ l]
    e1[l] = e1[ j]
    return
end

function Whetstone:p3(x, y, z, t, t1, t2)
    x = y
    y = z
    x = t * ((x + y))
    y = t1 * ((x + y))
    z = ((x + y)) / t2
    return { x = x, y = y, z = z }
end

function Whetstone:pout(title, ops, type, checknum, time, calibrate, section)
    local mops = 0.0
    local mflops = 0.0
    self.check = self.check + checknum
    self.loop_time[section] = time
    self.headings[section] = title
    self.timeUsed = self.timeUsed + time
    if calibrate == 1 then
        self.results[section] = checknum
    end
    if calibrate == 0 then
        self:write("%-18s %24.17f    ", self.headings[section], self.results[section])
        if type == 1 then
            if time > 0 then
                mflops = ops / (1000000.0 * time)
            else
                mflops = 0.0
            end
            self.loop_mops[section] = 99999.0
            self.loop_mflops[section] = mflops
            self:writeLine("%9.3f           %9.3f", self.loop_mflops[section], self.loop_time[section])
        else
            if time > 0 then
                mops = ops / (1000000.0 *  time)
            else
                mops = 0.0
            end
            self.loop_mops[section] = mops
            self.loop_mflops[section] = 0.0
            self:writeLine("           %9.3f%9.3f", self.loop_mops[section], self.loop_time[section])
        end
    end
    return
end

function Whetstone:writeLine(text, ...)
    self.output:writeLine(text, ...)
end

function Whetstone:write(text, ...)
    self.output:write(text, ...)
end
