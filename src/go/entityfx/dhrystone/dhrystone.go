package dhrystone

import (
	"strings"
	"../utils"
)

const (
    Ident_1 = 0
    Ident_2 = 1
    Ident_3 = 2
    Ident_4 = 3
    Ident_5 = 4
)

const LOOPS = 20000000

type Record struct {
	PtrComp *Record
	Discr int
	EnumComp int 
	IntComp int
	StringComp string
}

type CheckRecord struct {
	EnumLoc int
	IntLoc1 int
	IntLoc2 int
	IntLoc3 int
	String1Loc string
	String2Loc string
}

type DhrystoneResult struct {
	Output string
	TimeUsed int64
	Dhrystones int64
	VaxMips float64
}

var IntGlob = 0
var BoolGlob = false
var Char1Glob byte = '\\'
var Char2Glob byte = '\\'
var Array1Glob = [50]int{}
var Array2Glob = [50][50]int{}
var PtrGlbNext *Record
var PtrGlb *Record
var Check *CheckRecord

func Bench(loops int, output utils.WriterType) *DhrystoneResult {
	if (loops <= 0) {
		loops = LOOPS
	}
	output.WriteLine("##########################################")
	output.WriteLine("")
	output.WriteLine("Dhrystone Benchmark, Version 2.1 (Language: Go)")
	output.WriteLine("")
	output.WriteLine("Optimization %s", "Optimised")

	result := Proc0(loops)
	output.WriteLine("")
	output.WriteLine("Final values (* implementation-dependent):\n")
	output.WriteLine("")
	output.Write("Int_Glob:      ")
	if ((IntGlob == 5)) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d  ", IntGlob)
	output.Write("Bool_Glob:     ")
	if (BoolGlob) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%t", BoolGlob)
	output.Write("Ch_1_Glob:     ")
	if ((Char1Glob == 'A')) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%c  ", Char1Glob)
	output.Write("Ch_2_Glob:     ")
	if ((Char2Glob == 'B')) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%c", Char2Glob)
	output.Write("Arr_1_Glob[8]: ")
	if ((Array1Glob[8] == 7)) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d  ", Array1Glob[8])
	output.Write("Arr_2_Glob8/7: ")
	if ((Array2Glob[8][7] == (LOOPS + 10))) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%d", Array2Glob[8][7])
	output.Write("Ptr_Glob->            ")
	output.WriteLine("  Ptr_Comp:       *    %p", PtrGlb.PtrComp)
	output.Write("  Discr:       ")
	if ((PtrGlb.Discr == 0)) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d  ", PtrGlb.Discr)
	output.Write("Enum_Comp:     ")
	if ((PtrGlb.EnumComp == 2)) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%d", PtrGlb.EnumComp)
	output.Write("  Int_Comp:    ")
	if ((PtrGlb.IntComp == 17)) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d ", PtrGlb.IntComp)
	output.Write("Str_Comp:      ")
	if ((PtrGlb.StringComp == "DHRYSTONE PROGRAM, SOME STRING")) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%s", PtrGlb.StringComp)
	output.Write("Next_Ptr_Glob->       ")
	output.Write("  Ptr_Comp:       *    %p", PtrGlbNext.PtrComp)
	output.WriteLine(" same as above")
	output.Write("  Discr:       ")
	if ((PtrGlbNext.Discr == 0)) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d  ", PtrGlbNext.Discr)
	output.Write("Enum_Comp:     ")
	if ((PtrGlbNext.EnumComp == 1)) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%d", PtrGlbNext.EnumComp)
	output.Write("  Int_Comp:    ")
	if ((PtrGlbNext.IntComp == 18)) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d ", PtrGlbNext.IntComp)
	output.Write("Str_Comp:      ")
	if ((PtrGlbNext.StringComp == "DHRYSTONE PROGRAM, SOME STRING")) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%s", PtrGlbNext.StringComp)
	output.Write("Int_1_Loc:     ")
	if ((Check.IntLoc1 == 5)) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d  ", Check.IntLoc1)
	output.Write("Int_2_Loc:     ")
	if ((Check.IntLoc2 == 13)) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%d", Check.IntLoc2)
	output.Write("Int_3_Loc:     ")
	if ((Check.IntLoc3 == 7)) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d  ", Check.IntLoc3)
	output.Write("Enum_Loc:      ")
	if ((Check.EnumLoc == 1)) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%d  ", Check.EnumLoc)
	output.Write("Str_1_Loc:                             ")
	if Check.String1Loc == "DHRYSTONE PROGRAM, 1'ST STRING" {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%s", Check.String1Loc)
	output.Write("Str_2_Loc:                             ")
	if Check.String2Loc == "DHRYSTONE PROGRAM, 2'ND STRING" {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%s", Check.String2Loc)
	output.WriteNewLine()
	output.WriteLine("Nanoseconds one Dhrystone run: %d", (1000000000 / result.Dhrystones))
	output.WriteLine("Dhrystones per Second:         %d", result.Dhrystones)
	output.WriteLine("VAX  MIPS rating =             %.2f", result.VaxMips)
	output.WriteNewLine()
	result.Output = ""
	return result
}

func Proc0(loops int) *DhrystoneResult {
	PtrGlbNext = &Record{}
	PtrGlb = &Record{}
	PtrGlb.PtrComp = PtrGlbNext
	PtrGlb.Discr = Ident_1
	PtrGlb.EnumComp = Ident_3
	PtrGlb.IntComp = 40
	PtrGlb.StringComp = "DHRYSTONE PROGRAM, SOME STRING"
	var String1Loc = "DHRYSTONE PROGRAM, 1'ST STRING"
	var String2Loc = "DHRYSTONE PROGRAM, 2'ND STRING"

	IntGlob = 0
	BoolGlob = false
	Char1Glob = 0
	Char2Glob = 0

	Array2Glob[8][7] = 10

	EnumLoc := 0
	IntLoc1 := 0
	IntLoc2 := 0
	IntLoc3 := 0

	start := utils.MakeTimestamp()

	for i := 0; i < loops; i++ {
		Proc5()
		Proc4()
		IntLoc1 = 2
		IntLoc2 = 3
		IntLoc3 = 0
		String2Loc = "DHRYSTONE PROGRAM, 2'ND STRING"
		EnumLoc = Ident_2
		BoolGlob = !Func2(String1Loc, String2Loc)
		for IntLoc1 < IntLoc2 {
			IntLoc3 = 5 * IntLoc1 - IntLoc2
			IntLoc3 = Proc7(IntLoc1, IntLoc2)
			IntLoc1 += 1
		}
		Proc8(&Array1Glob, &Array2Glob, IntLoc1, IntLoc3)
		PtrGlb = Proc1(PtrGlb)
		var CharIndex byte = 'A'

		for CharIndex = 'A'; CharIndex <= Char2Glob; CharIndex++ {
			if EnumLoc == Func1(CharIndex, 'C') {
				EnumLoc = Proc6(Ident_1)
			}
		}

		IntLoc2 = IntLoc2 * IntLoc1
		IntLoc1 = IntLoc2 / IntLoc3
		IntLoc2 = 7 * (IntLoc2 - IntLoc3) - IntLoc1
		IntLoc1 = Proc2(IntLoc1)
	}

	var benchtime = utils.MakeTimestamp() - start
	var loopsPerBenchtime = int64(0)
	if benchtime == 0 {
		loopsPerBenchtime = 0
	} else {
		loopsPerBenchtime = int64(loops) / benchtime
	}

	var dhrystones = int64(1000) * loopsPerBenchtime

	Check = &CheckRecord{
		EnumLoc,
		IntLoc1,
		IntLoc2,
		IntLoc3,
		String1Loc,
		String2Loc,
	}

	return &DhrystoneResult{
		"",
		benchtime,
		dhrystones,
		float64(dhrystones) / float64(1757),
	}
}

func Proc1(PtrValPar *Record) *Record {
	NextRecord := PtrGlb.PtrComp
	StructAssign(PtrGlb.PtrComp, PtrGlb)
	PtrValPar.IntComp = 5
	NextRecord.IntComp = PtrValPar.IntComp
	NextRecord.PtrComp = PtrValPar.PtrComp
	NextRecord.PtrComp = Proc3(NextRecord.PtrComp)

	if NextRecord.Discr == Ident_1 {
		NextRecord.IntComp = 6
		NextRecord.EnumComp = Proc6(PtrValPar.EnumComp)
		NextRecord.PtrComp = PtrGlb.PtrComp
		NextRecord.IntComp = Proc7(NextRecord.IntComp, 10)
	} else {
		StructAssign(PtrValPar, PtrValPar.PtrComp)
	}

	return PtrValPar
}

func Proc2(IntParIO int) int {
	var IntLoc = IntParIO + 10
	EnumLoc := Ident_2
	for true {
		if Char1Glob == 'A' {
			IntLoc = IntLoc - 1
			IntParIO = IntLoc - IntGlob
			EnumLoc = Ident_1
		}
		if EnumLoc == Ident_1 { break }
	}
	return IntParIO
}

func Proc3(PtrParOut *Record) *Record {
	if PtrGlb != nil {
		PtrParOut = PtrGlb.PtrComp
	} else {
		IntGlob = 100
	}
	PtrGlb.IntComp = Proc7(10, IntGlob)
	return PtrParOut
}

func StructAssign(destination *Record, source *Record) {
	destination.IntComp = source.IntComp
	destination.PtrComp = source.PtrComp
	destination.StringComp = source.StringComp
	destination.EnumComp = source.EnumComp
	destination.Discr = source.Discr
}

func Proc4() {
	BoolLoc := Char1Glob == 'A'
	BoolGlob = BoolLoc || BoolGlob
	Char2Glob = 'B'
}

func Proc5() {
	Char1Glob = 'A'
	BoolGlob = false
}

func Proc6(EnumParIn int) int {
	EnumParOut := EnumParIn
	if !Func3(EnumParIn) {
		EnumParOut = Ident_4
	}

	switch (EnumParIn) {
		case Ident_1:
			EnumParOut = Ident_1
			break
		case Ident_2:
			if IntGlob > 100 {
				EnumParOut = Ident_1
			} else {
				EnumParOut = Ident_4
			}
			break
		case Ident_3:
			EnumParOut = Ident_2
			break
		case Ident_4:
			break
		case Ident_5:
			EnumParOut = Ident_3
			break
	}

	return EnumParOut
}

func Proc7(IntParI1 int, IntParI2 int) int {
	IntLoc := IntParI1 + 2
	IntParOut := IntParI2 + IntLoc
	return IntParOut
}

func Proc8(Array1Par *[50]int, Array2Par *[50][50]int, IntParI1 int, IntParI2 int) {
	IntLoc := IntParI1 + 5
	Array1Par[IntLoc] = IntParI2
	Array1Par[(IntLoc + 1)] = Array1Par[IntLoc]
	Array1Par[(IntLoc + 30)] = IntLoc
	// for IntIndex in range(IntLoc, IntLoc+2):
	for IntIndex := IntLoc; IntIndex < (IntLoc + 2); IntIndex++ {
		Array2Par[IntLoc][IntIndex] = IntLoc
	}

	Array2Par[IntLoc][(IntLoc - 1)] = (Array2Par[IntLoc][(IntLoc - 1)] + 1)
	Array2Par[(IntLoc + 20)][IntLoc] = Array1Par[IntLoc]
	IntGlob = 5
}

func Func1(CharPar1 byte, CharPar2 byte) int {
	CharLoc1 := CharPar1
	CharLoc2 := CharLoc1
	if CharLoc2 != CharPar2 { return Ident_1 } else { return Ident_2 }
}

func Func2(StrParI1 string, StrParI2 string) bool {
	IntLoc := 1
	var CharLoc byte = '\\'

	for IntLoc <= 1 {
		if Func1(StrParI1[IntLoc], StrParI2[IntLoc + 1]) == Ident_1 {
			CharLoc = 'A'
			IntLoc = IntLoc + 1
		}

	}

	if (CharLoc >= 'W') && (CharLoc <= 'Z') {
		IntLoc = 7
	}

	if CharLoc == 'X' {
		return true
	} else if strings.Compare(StrParI1, StrParI2) > 0 {
		IntLoc = (IntLoc + 7)
		return true
	} else {
		return false
	}

}

func Func3(EnumParIn int) bool {
	EnumLoc := EnumParIn
	return EnumLoc == Ident_3
}