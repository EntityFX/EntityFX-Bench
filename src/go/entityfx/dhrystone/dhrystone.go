package dhrystone

import (
	"strings"

	"github.com/EntityFX/EntityFX-Bench/src/go/entityfx/utils"
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
	PtrComp    *Record
	Discr      int
	EnumComp   int
	IntComp    int
	StringComp string
}

type CheckRecord struct {
	EnumLoc    int
	IntLoc1    int
	IntLoc2    int
	IntLoc3    int
	String1Loc string
	String2Loc string
}

type DhrystoneResult struct {
	Output     string
	TimeUsed   int64
	Dhrystones int64
	VaxMips    float64
}

type Dhrystone struct {
	IntGlob    int
	BoolGlob   bool
	Char1Glob  byte
	Char2Glob  byte
	Array1Glob [50]int
	Array2Glob [50][50]int
	PtrGlbNext *Record
	PtrGlb     *Record
	Check      *CheckRecord
}

//'\\'

func Bench(loops int, output utils.WriterType) *DhrystoneResult {
	if loops <= 0 {
		loops = LOOPS
	}
	output.WriteLine("##########################################")
	output.WriteLine("")
	output.WriteLine("Dhrystone Benchmark, Version 2.1 (Language: Go)")
	output.WriteLine("")
	output.WriteLine("Optimization %s", "Optimised")

	dhrystone := &Dhrystone{0, false, '\\', '\\', [50]int{}, [50][50]int{}, nil, nil, nil}

	result := Proc0(loops, dhrystone)
	output.WriteLine("")
	output.WriteLine("Final values (* implementation-dependent):\n")
	output.WriteLine("")
	output.Write("Int_Glob:      ")

	if dhrystone.IntGlob == 5 {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d  ", dhrystone.IntGlob)
	output.Write("Bool_Glob:     ")
	if dhrystone.BoolGlob {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%t", dhrystone.BoolGlob)
	output.Write("Ch_1_Glob:     ")
	if dhrystone.Char1Glob == 'A' {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%c  ", dhrystone.Char1Glob)
	output.Write("Ch_2_Glob:     ")
	if dhrystone.Char2Glob == 'B' {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%c", dhrystone.Char2Glob)
	output.Write("Arr_1_Glob[8]: ")
	if dhrystone.Array1Glob[8] == 7 {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d  ", dhrystone.Array1Glob[8])
	output.Write("Arr_2_Glob8/7: ")
	if dhrystone.Array2Glob[8][7] == (LOOPS + 10) {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%d", dhrystone.Array2Glob[8][7])
	output.Write("Ptr_Glob->            ")
	output.WriteLine("  Ptr_Comp:       *    %p", dhrystone.PtrGlb.PtrComp)
	output.Write("  Discr:       ")
	if dhrystone.PtrGlb.Discr == 0 {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d  ", dhrystone.PtrGlb.Discr)
	output.Write("Enum_Comp:     ")
	if dhrystone.PtrGlb.EnumComp == 2 {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%d", dhrystone.PtrGlb.EnumComp)
	output.Write("  Int_Comp:    ")
	if dhrystone.PtrGlb.IntComp == 17 {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d ", dhrystone.PtrGlb.IntComp)
	output.Write("Str_Comp:      ")
	if dhrystone.PtrGlb.StringComp == "DHRYSTONE PROGRAM, SOME STRING" {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%s", dhrystone.PtrGlb.StringComp)
	output.Write("Next_Ptr_Glob->       ")
	output.Write("  Ptr_Comp:       *    %p", dhrystone.PtrGlbNext.PtrComp)
	output.WriteLine(" same as above")
	output.Write("  Discr:       ")
	if dhrystone.PtrGlbNext.Discr == 0 {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d  ", dhrystone.PtrGlbNext.Discr)
	output.Write("Enum_Comp:     ")
	if dhrystone.PtrGlbNext.EnumComp == 1 {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%d", dhrystone.PtrGlbNext.EnumComp)
	output.Write("  Int_Comp:    ")
	if dhrystone.PtrGlbNext.IntComp == 18 {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d ", dhrystone.PtrGlbNext.IntComp)
	output.Write("Str_Comp:      ")
	if dhrystone.PtrGlbNext.StringComp == "DHRYSTONE PROGRAM, SOME STRING" {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%s", dhrystone.PtrGlbNext.StringComp)
	output.Write("Int_1_Loc:     ")
	if dhrystone.Check.IntLoc1 == 5 {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d  ", dhrystone.Check.IntLoc1)
	output.Write("Int_2_Loc:     ")
	if dhrystone.Check.IntLoc2 == 13 {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%d", dhrystone.Check.IntLoc2)
	output.Write("Int_3_Loc:     ")
	if dhrystone.Check.IntLoc3 == 7 {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.Write("%d  ", dhrystone.Check.IntLoc3)
	output.Write("Enum_Loc:      ")
	if dhrystone.Check.EnumLoc == 1 {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%d  ", dhrystone.Check.EnumLoc)
	output.Write("Str_1_Loc:                             ")
	if dhrystone.Check.String1Loc == "DHRYSTONE PROGRAM, 1'ST STRING" {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%s", dhrystone.Check.String1Loc)
	output.Write("Str_2_Loc:                             ")
	if dhrystone.Check.String2Loc == "DHRYSTONE PROGRAM, 2'ND STRING" {
		output.Write("O.K.  ")
	} else {
		output.Write("WRONG ")
	}

	output.WriteLine("%s", dhrystone.Check.String2Loc)
	output.WriteNewLine()
	output.WriteLine("Nanoseconds one Dhrystone run: %d", (1000000000 / result.Dhrystones))
	output.WriteLine("Dhrystones per Second:         %d", result.Dhrystones)
	output.WriteLine("VAX  MIPS rating =             %.2f", result.VaxMips)
	output.WriteNewLine()
	result.Output = output.GetOutput()
	return result
}

func Proc0(loops int, dhrystone *Dhrystone) *DhrystoneResult {
	dhrystone.PtrGlbNext = &Record{}
	dhrystone.PtrGlb = &Record{}
	dhrystone.PtrGlb.PtrComp = dhrystone.PtrGlbNext
	dhrystone.PtrGlb.Discr = Ident_1
	dhrystone.PtrGlb.EnumComp = Ident_3
	dhrystone.PtrGlb.IntComp = 40
	dhrystone.PtrGlb.StringComp = "DHRYSTONE PROGRAM, SOME STRING"
	var String1Loc = "DHRYSTONE PROGRAM, 1'ST STRING"
	var String2Loc = "DHRYSTONE PROGRAM, 2'ND STRING"

	dhrystone.IntGlob = 0
	dhrystone.BoolGlob = false
	dhrystone.Char1Glob = 0
	dhrystone.Char2Glob = 0

	dhrystone.Array2Glob[8][7] = 10

	EnumLoc := 0
	IntLoc1 := 0
	IntLoc2 := 0
	IntLoc3 := 0

	start := utils.MakeTimestamp()

	for i := 0; i < loops; i++ {
		Proc5(dhrystone)
		Proc4(dhrystone)
		IntLoc1 = 2
		IntLoc2 = 3
		IntLoc3 = 0
		String2Loc = "DHRYSTONE PROGRAM, 2'ND STRING"
		EnumLoc = Ident_2
		dhrystone.BoolGlob = !Func2(String1Loc, String2Loc)
		for IntLoc1 < IntLoc2 {
			IntLoc3 = 5*IntLoc1 - IntLoc2
			IntLoc3 = Proc7(IntLoc1, IntLoc2)
			IntLoc1 += 1
		}
		Proc8(&dhrystone.Array1Glob, &dhrystone.Array2Glob, IntLoc1, IntLoc3, dhrystone)
		dhrystone.PtrGlb = Proc1(dhrystone.PtrGlb, dhrystone)
		var CharIndex byte = 'A'

		for CharIndex = 'A'; CharIndex <= dhrystone.Char2Glob; CharIndex++ {
			if EnumLoc == Func1(CharIndex, 'C') {
				EnumLoc = Proc6(Ident_1, dhrystone)
			}
		}

		IntLoc2 = IntLoc2 * IntLoc1
		IntLoc1 = IntLoc2 / IntLoc3
		IntLoc2 = 7*(IntLoc2-IntLoc3) - IntLoc1
		IntLoc1 = Proc2(IntLoc1, dhrystone)
	}

	var benchtime = utils.MakeTimestamp() - start
	var loopsPerBenchtime = int64(0)
	if benchtime == 0 {
		loopsPerBenchtime = 0
	} else {
		loopsPerBenchtime = int64(loops) / benchtime
	}

	var dhrystones = int64(1000) * loopsPerBenchtime

	dhrystone.Check = &CheckRecord{
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

func Proc1(PtrValPar *Record, dhrystone *Dhrystone) *Record {
	NextRecord := dhrystone.PtrGlb.PtrComp
	StructAssign(dhrystone.PtrGlb.PtrComp, dhrystone.PtrGlb)
	PtrValPar.IntComp = 5
	NextRecord.IntComp = PtrValPar.IntComp
	NextRecord.PtrComp = PtrValPar.PtrComp
	NextRecord.PtrComp = Proc3(NextRecord.PtrComp, dhrystone)

	if NextRecord.Discr == Ident_1 {
		NextRecord.IntComp = 6
		NextRecord.EnumComp = Proc6(PtrValPar.EnumComp, dhrystone)
		NextRecord.PtrComp = dhrystone.PtrGlb.PtrComp
		NextRecord.IntComp = Proc7(NextRecord.IntComp, 10)
	} else {
		StructAssign(PtrValPar, PtrValPar.PtrComp)
	}

	return PtrValPar
}

func Proc2(IntParIO int, dhrystone *Dhrystone) int {
	var IntLoc = IntParIO + 10
	EnumLoc := Ident_2
	for true {
		if dhrystone.Char1Glob == 'A' {
			IntLoc = IntLoc - 1
			IntParIO = IntLoc - dhrystone.IntGlob
			EnumLoc = Ident_1
		}
		if EnumLoc == Ident_1 {
			break
		}
	}
	return IntParIO
}

func Proc3(PtrParOut *Record, dhrystone *Dhrystone) *Record {
	if dhrystone.PtrGlb != nil {
		PtrParOut = dhrystone.PtrGlb.PtrComp
	} else {
		dhrystone.IntGlob = 100
	}
	dhrystone.PtrGlb.IntComp = Proc7(10, dhrystone.IntGlob)
	return PtrParOut
}

func StructAssign(destination *Record, source *Record) {
	destination.IntComp = source.IntComp
	destination.PtrComp = source.PtrComp
	destination.StringComp = source.StringComp
	destination.EnumComp = source.EnumComp
	destination.Discr = source.Discr
}

func Proc4(dhrystone *Dhrystone) {
	BoolLoc := dhrystone.Char1Glob == 'A'
	dhrystone.BoolGlob = BoolLoc || dhrystone.BoolGlob
	dhrystone.Char2Glob = 'B'
}

func Proc5(dhrystone *Dhrystone) {
	dhrystone.Char1Glob = 'A'
	dhrystone.BoolGlob = false
}

func Proc6(EnumParIn int, dhrystone *Dhrystone) int {
	EnumParOut := EnumParIn
	if !Func3(EnumParIn) {
		EnumParOut = Ident_4
	}

	switch EnumParIn {
	case Ident_1:
		EnumParOut = Ident_1
		break
	case Ident_2:
		if dhrystone.IntGlob > 100 {
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

func Proc8(Array1Par *[50]int, Array2Par *[50][50]int, IntParI1 int, IntParI2 int, dhrystone *Dhrystone) {
	IntLoc := IntParI1 + 5
	Array1Par[IntLoc] = IntParI2
	Array1Par[(IntLoc + 1)] = Array1Par[IntLoc]
	Array1Par[(IntLoc + 30)] = IntLoc
	// for IntIndex in range(IntLoc, IntLoc+2):
	for IntIndex := IntLoc; IntIndex < (IntLoc + 2); IntIndex++ {
		Array2Par[IntLoc][IntIndex] = IntLoc
	}

	Array2Par[IntLoc][(IntLoc - 1)] = (Array2Par[IntLoc][(IntLoc-1)] + 1)
	Array2Par[(IntLoc + 20)][IntLoc] = Array1Par[IntLoc]
	dhrystone.IntGlob = 5
}

func Func1(CharPar1 byte, CharPar2 byte) int {
	CharLoc1 := CharPar1
	CharLoc2 := CharLoc1
	if CharLoc2 != CharPar2 {
		return Ident_1
	} else {
		return Ident_2
	}
}

func Func2(StrParI1 string, StrParI2 string) bool {
	IntLoc := 1
	var CharLoc byte = '\\'

	for IntLoc <= 1 {
		if Func1(StrParI1[IntLoc], StrParI2[IntLoc+1]) == Ident_1 {
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
