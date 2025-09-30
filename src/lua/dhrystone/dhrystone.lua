Dhrystone2 = class(function(d, printToConsole)
    d.output = Writer(nil)
    d.output.useConsole = printToConsole

    d.PtrGlbNext = {}
    d.PtrGlb = {}
    d.IntGlob = 0
    d.BoolGlob = false
    d.Char1Glob = "\\"
    d.Char2Glob = "\\"
    d.Array1Glob = {}
    d.Array2Glob = {}

    d.elapsed = 0
    d.startClock = 0
    d.endClock = 0
end)

Dhrystone2.LOOPS = 20000000

Dhrystone2.Ident_1 = 0
Dhrystone2.Ident_2 = 1
Dhrystone2.Ident_3 = 2
Dhrystone2.Ident_4 = 3
Dhrystone2.Ident_5 = 4

function Dhrystone2:structAssign(destination, source)
    destination.IntComp = source.IntComp
    destination.PtrComp = source.PtrComp
    destination.StringComp = source.StringComp
    destination.EnumComp = source.EnumComp
    destination.Discr = source.Discr
end

function Dhrystone2:bench(loops)
    if loops == nil then
        loops = Dhrystone2.LOOPS 
    end

    self.startClock = 0
    self.endClock = 0
    self.elapsed = 0

    self.output:writeLine("##########################################")
    self.output:writeLine("")
    self.output:writeLine("Dhrystone Benchmark, Version 2.1 (Language: Lua)")
    self.output:writeLine("")

    local optimization = getVersion() .. ((jit and jit.status() == true) and " Optimised" or " Not optimised")

    self.output:writeLine("Optimization: %s", optimization)

    local result = self:Proc0(loops)

    self.output:writeLine("")
    self.output:writeLine("Final values (* implementation-dependent):\n")
    self.output:writeLine("")
    self.output:write("Int_Glob:      ")
    if ((self.IntGlob == 5)) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:write("%d  ", self.IntGlob)
    self.output:write("Bool_Glob:     ")
    if (self.BoolGlob) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:writeLine("%s", tostring(self.BoolGlob))
    self.output:write("Ch_1_Glob:     ")
    if ((self.Char1Glob == 'A')) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:write("%s  ", self.Char1Glob)
    self.output:write("Ch_2_Glob:     ")
    if ((self.Char2Glob == 'B')) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:writeLine("%s", self.Char2Glob)
    self.output:write("Arr_1_Glob[8]: ")
    if ((self.Array1Glob[8] == 7)) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:write("%d  ", self.Array1Glob[8])
    self.output:write("Arr_2_Glob8/7: ")
    if self.Array2Glob[8][7] == (loops + 10) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:writeLine("%d", self.Array2Glob[8][7])
    self.output:write("Ptr_Glob->            ")
    self.output:writeLine("  Ptr_Comp:       *    %s", tostring(self.PtrGlb.PtrComp))
    self.output:write("  Discr:       ")
    if ((self.PtrGlb.Discr == 0)) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:write("%d  ", self.PtrGlb.Discr)
    self.output:write("Enum_Comp:     ")
    if ((self.PtrGlb.EnumComp == 2)) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:writeLine("%d", self.PtrGlb.EnumComp)
    self.output:write("  Int_Comp:    ")
    if ((self.PtrGlb.IntComp == 17)) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:write("%d ", self.PtrGlb.IntComp)
    self.output:write("Str_Comp:      ")
    if ((self.PtrGlb.StringComp == "DHRYSTONE PROGRAM, SOME STRING")) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:writeLine("%s", self.PtrGlb.StringComp)
    self.output:write("Next_Ptr_Glob->       ")
    self.output:write("  Ptr_Comp:       *    %s", tostring(self.PtrGlbNext.PtrComp))
    self.output:writeLine(" same as above")
    self.output:write("  Discr:       ")
    if ((self.PtrGlbNext.Discr == 0)) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:write("%d  ", self.PtrGlbNext.Discr)
    self.output:write("Enum_Comp:     ")
    if ((self.PtrGlbNext.EnumComp == 1)) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:writeLine("%d", self.PtrGlbNext.EnumComp)
    self.output:write("  Int_Comp:    ")
    if ((self.PtrGlbNext.IntComp == 18)) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:write("%d ", self.PtrGlbNext.IntComp)
    self.output:write("Str_Comp:      ")
    if ((self.PtrGlbNext.StringComp == "DHRYSTONE PROGRAM, SOME STRING")) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:writeLine("%s", self.PtrGlbNext.StringComp)
    self.output:write("Int_1_Loc:     ")
    if ((self.Check.IntLoc1 == 5)) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:write("%d  ", self.Check.IntLoc1)
    self.output:write("Int_2_Loc:     ")
    if ((self.Check.IntLoc2 == 13)) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:writeLine("%d", self.Check.IntLoc2)
    self.output:write("Int_3_Loc:     ")
    if ((self.Check.IntLoc3 == 7)) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:write("%d  ", self.Check.IntLoc3)
    self.output:write("Enum_Loc:      ")
    if ((self.Check.EnumLoc == 1)) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:writeLine("%d  ", self.Check.EnumLoc)
    self.output:write("Str_1_Loc:                             ")
    if ((self.Check.String1Loc == "DHRYSTONE PROGRAM, 1\'ST STRING")) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:writeLine("%s", self.Check.String1Loc)
    self.output:write("Str_2_Loc:                             ")
    if ((self.Check.String2Loc == "DHRYSTONE PROGRAM, 2\'ND STRING")) then
        self.output:write("O.K.  ")
    else
        self.output:write("WRONG ")
    end

    self.output:writeLine("%s", self.Check.String2Loc)
    self.output:writeLine()
    self.output:writeLine("Nanoseconds one Dhrystone run: %.2f", (1000000000 / result.Dhrystones))
    self.output:writeLine("Dhrystones per Second:         %.2f", result.Dhrystones)
    self.output:writeLine("VAX  MIPS rating =             %.2f", result.VaxMips)
    self.output:writeLine("")
    result.output = self.output.output
    return result
end

function Dhrystone2:Proc0(loops)
    self.PtrGlbNext = {}
    self.PtrGlb = {}
    self.PtrGlb.PtrComp = self.PtrGlbNext
    self.PtrGlb.Discr = Dhrystone2.Ident_1
    self.PtrGlb.EnumComp = Dhrystone2.Ident_3
    self.PtrGlb.IntComp = 40
    self.PtrGlb.StringComp = "DHRYSTONE PROGRAM, SOME STRING"
    local String1Loc = "DHRYSTONE PROGRAM, 1'ST STRING"
    local String2Loc = "DHRYSTONE PROGRAM, 2'ND STRING"
    self.IntGlob = 0
    self.BoolGlob = false
    self.Char1Glob = "\\"
    self.Char2Glob = "\\"
    self.Array1Glob = {}
    self.Array2Glob = {}

    for idx=0,50 do
        self.Array1Glob[idx] = 0
        self.Array2Glob[idx] = {}
        for idy=0,50 do
            self.Array2Glob[idx][idy] = 0
        end
    end

    self.Array2Glob[8][7] = 10
    local EnumLoc = 0
    local IntLoc1 = 0
    local IntLoc2 = 0
    local IntLoc3 = 0

    self.startClock = clock()

    for i=0,loops - 1 do
        self:Proc5()
        self:Proc4()
        IntLoc1 = 2
        IntLoc2 = 3
        IntLoc3 = 0
        String2Loc = "DHRYSTONE PROGRAM, 2\'ND STRING"
        EnumLoc = Dhrystone2.Ident_2
        self.BoolGlob = not self:Func2(String1Loc, String2Loc)
        while IntLoc1 < IntLoc2 do
            IntLoc3 = 5 * IntLoc1 - IntLoc2
            IntLoc3 = self:Proc7(IntLoc1, IntLoc2)
            IntLoc1 = IntLoc1 + 1
        end
        self:Proc8(self.Array1Glob, self.Array2Glob, IntLoc1, IntLoc3)
        self.PtrGlb = self:Proc1(self.PtrGlb)
        local CharIndex = 'A';
        local charIndexByte = string.byte(CharIndex)
        local char2GlobByte = string.byte(self.Char2Glob)

        while charIndexByte <= char2GlobByte do
            if EnumLoc == self:Func1(string.char(charIndexByte), 'C') then
                EnumLoc = self:Proc6(Dhrystone2.Ident_1)
            end
            charIndexByte = charIndexByte + 1
        end

        IntLoc2 = (IntLoc2 * IntLoc1)
        IntLoc1 = math.floor(IntLoc2 / IntLoc3)
        IntLoc2 = ((7 * (IntLoc2 - IntLoc3)) - IntLoc1)
        IntLoc1 = self:Proc2(IntLoc1)
    end

    self.endClock = clock()
    self.elapsed =  (self.endClock - self.startClock)

    local benchtime = self.elapsed
    local loopsPerBenchtime = 0
    if ((benchtime == 0)) then
        loopsPerBenchtime = 0;
    else
        loopsPerBenchtime = (loops / benchtime)
    end

    local dhrystones = 1000 * loopsPerBenchtime
    self.Check = {
        EnumLoc = EnumLoc,
        IntLoc1 = IntLoc1,
        IntLoc2 = IntLoc2,
        IntLoc3 = IntLoc3,
        String1Loc = String1Loc,
        String2Loc = String2Loc
    }

    return {
        Dhrystones = dhrystones,
        Output = self.output.output,
        TimeUsed = benchtime,
        VaxMips = dhrystones / 1757
    }
end

function Dhrystone2:Proc1(PtrValPar)
    local NextRecord = self.PtrGlb.PtrComp
    self:structAssign(self.PtrGlb.PtrComp, self.PtrGlb)
    PtrValPar.IntComp = 5
    NextRecord.IntComp = PtrValPar.IntComp
    NextRecord.PtrComp = PtrValPar.PtrComp
    NextRecord.PtrComp = self:Proc3(NextRecord.PtrComp)
    if NextRecord.Discr == Dhrystone2.Ident_1 then
        NextRecord.IntComp = 6
        NextRecord.EnumComp = self:Proc6(PtrValPar.EnumComp)
        NextRecord.PtrComp = self.PtrGlb.PtrComp
        NextRecord.IntComp = Dhrystone2:Proc7(NextRecord.IntComp, 10)
    else
        Dhrystone2:structAssign(PtrValPar, PtrValPar.PtrComp)
    end

    return PtrValPar
end

function Dhrystone2:Proc2(IntParIO)
    local IntLoc = IntParIO + 10
    local EnumLoc = Dhrystone2.Ident_2
    while true do
        if self.Char1Glob == 'A' then
            IntLoc = IntLoc - 1
            IntParIO = IntLoc - self.IntGlob
            EnumLoc = Dhrystone2.Ident_1
        end

        if EnumLoc == Dhrystone2.Ident_1 then
            break
        end

    end

    return IntParIO
end

function Dhrystone2:Proc3(PtrParOut)
    if self.PtrGlb ~= nil then
        PtrParOut = self.PtrGlb.PtrComp
    else
        self.IntGlob = 100
    end

    self.PtrGlb.IntComp = self:Proc7(10, self.IntGlob)
    return PtrParOut
end

function Dhrystone2:Proc4()
    local BoolLoc = self.Char1Glob == 'A'
    self.BoolGlob = BoolLoc or self.BoolGlob
    self.Char2Glob = 'B'
end

function Dhrystone2:Proc5()
    self.Char1Glob = 'A'
    self.BoolGlob = false
end

function Dhrystone2:Proc6(EnumParIn)
    local EnumParOut = EnumParIn
    if not self:Func3(EnumParIn) then
        EnumParOut = Dhrystone2.Ident_4
    end


    if EnumParIn == Dhrystone2.Ident_1 then
        EnumParOut = Dhrystone2.Ident_1
    elseif EnumParIn == Dhrystone2.Ident_2 then
        if ((self.IntGlob > 100)) then
            EnumParOut = Dhrystone2.Ident_1
        else
            EnumParOut = Dhrystone2.Ident_4
        end
    elseif EnumParIn == Dhrystone2.Ident_3 then
        EnumParOut = Dhrystone2.Ident_2
    elseif EnumParIn == Dhrystone2.Ident_4 then
    elseif EnumParIn == Dhrystone2.Ident_5 then
        EnumParOut = Dhrystone2.Ident_3
    end
    return EnumParOut
end

function Dhrystone2:Proc7(IntParI1, IntParI2)
    local IntLoc = (IntParI1 + 2)
    local IntParOut = (IntParI2 + IntLoc)
    return IntParOut
end

function Dhrystone2:Proc8(Array1Par, Array2Par, IntParI1, IntParI2)
    local IntLoc = (IntParI1 + 5)
    Array1Par[IntLoc] = IntParI2
    Array1Par[(IntLoc + 1)] = Array1Par[IntLoc]
    Array1Par[(IntLoc + 30)] = IntLoc

    for IntIndex = IntLoc, IntLoc + 2 do
        Array2Par[IntLoc][IntIndex] = IntLoc
    end

    Array2Par[IntLoc][(IntLoc - 1)] = (Array2Par[IntLoc][(IntLoc - 1)] + 1)
    Array2Par[(IntLoc + 20)][IntLoc] = Array1Par[IntLoc]
    self.IntGlob = 5
end


function Dhrystone2:Func1(CharPar1, CharPar2)
    local CharLoc1 = CharPar1
    local CharLoc2 = CharLoc1
    if CharLoc2 ~= CharPar2 then 
        return Dhrystone2.Ident_1 
    else 
        return Dhrystone2.Ident_2
    end
end

function Dhrystone2:Func2(StrParI1, StrParI2)
    local IntLoc = 1
    local CharLoc = '\\'
    while IntLoc <= 1 do
        if self:Func1(string.sub(StrParI1, IntLoc + 1, IntLoc + 1), string.sub(StrParI1, IntLoc + 2, IntLoc + 2)) == Dhrystone2.Ident_1 then
            CharLoc = 'A'
            IntLoc = (IntLoc + 1)
        end
    end

    if CharLoc >= 'W' and CharLoc <= 'Z' then
        IntLoc = 7
    end
    if ((CharLoc == 'X')) then
        return true;
    elseif compareTo(StrParI1, StrParI2) > 0 then
        IntLoc = (IntLoc + 7)
        return true
    else
        return false
    end
end

function Dhrystone2:Func3(EnumParIn)
    local EnumLoc = EnumParIn
    return (EnumLoc == Dhrystone2.Ident_3)
end
