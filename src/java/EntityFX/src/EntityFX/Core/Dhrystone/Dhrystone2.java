
package EntityFX.Core.Dhrystone;

import java.io.FileNotFoundException;
import java.io.IOException;

import EntityFX.Core.Writer;

public class Dhrystone2 {

    static final int Ident_1 = 0;

    static final int Ident_2 = 1;

    static final int Ident_3 = 2;

    static final int Ident_4 = 3;

    static final int Ident_5 = 4;

    public class DhrystoneResult {
        public String Output;

        public double VaxMips;

        public double Dhrystones;

        public double TimeUsed;

    }

    class Record {

        public Record PtrComp;

        public int Discr;

        public int EnumComp;

        public int IntComp;

        public String StringComp;
    }

    private static CheckRecord Check;

    public class CheckRecord {

        public int IntLoc1;

        public int IntLoc2;

        public int IntLoc3;

        public String String1Loc;

        public String String2Loc;

        public int EnumLoc;
    }

    private Writer output;

    public Dhrystone2(boolean printToConsole) throws FileNotFoundException {
        output = new Writer(null);
        this.output.UseConsole = printToConsole;
    }

    public final DhrystoneResult Bench(int loops) throws IOException {
        loops = loops <= 0 ? LOOPS : loops;
        this.output.writeLine("##########################################");
        this.output.writeLine("");
        this.output.writeLine("Dhrystone Benchmark, Version 2.1 (Language: C#)");
        this.output.writeLine("");
        this.output.writeLine("Optimization %s", "Optimised");

        var result = this.Proc0(loops);
        this.output.writeLine("");
        this.output.writeLine("Final values (* implementation-dependent):\n");
        this.output.writeLine("");
        this.output.write("Int_Glob:      ");
        if ((IntGlob == 5)) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.write("%d  ", IntGlob);
        this.output.write("Bool_Glob:     ");
        if (BoolGlob) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.writeLine("%b", BoolGlob);
        this.output.write("Ch_1_Glob:     ");
        if ((Char1Glob == 'A')) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.write("%s  ", Char1Glob);
        this.output.write("Ch_2_Glob:     ");
        if ((Char2Glob == 'B')) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.writeLine("%s", Char2Glob);
        this.output.write("Arr_1_Glob[8]: ");
        if ((Array1Glob[8] == 7)) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.write("%d  ", Array1Glob[8]);
        this.output.write("Arr_2_Glob8/7: ");
        if ((this.Array2Glob[8][7] == (LOOPS + 10))) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.writeLine("%d", Array2Glob[8][7]);
        this.output.write("Ptr_Glob->            ");
        this.output.writeLine("  Ptr_Comp:       *    %b", (PtrGlb.PtrComp != null));
        this.output.write("  Discr:       ");
        if ((PtrGlb.Discr == 0)) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.write("%d  ", PtrGlb.Discr);
        this.output.write("Enum_Comp:     ");
        if ((PtrGlb.EnumComp == 2)) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.writeLine("%d", PtrGlb.EnumComp);
        this.output.write("  Int_Comp:    ");
        if ((PtrGlb.IntComp == 17)) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.write("%d ", PtrGlb.IntComp);
        this.output.write("Str_Comp:      ");
        if ((PtrGlb.StringComp == "DHRYSTONE PROGRAM, SOME STRING")) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.writeLine("%s", PtrGlb.StringComp);
        this.output.write("Next_Ptr_Glob->       ");
        this.output.write("  Ptr_Comp:       *    %b", (PtrGlbNext.PtrComp != null));
        this.output.writeLine(" same as above");
        this.output.write("  Discr:       ");
        if ((PtrGlbNext.Discr == 0)) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.write("%d  ", PtrGlbNext.Discr);
        this.output.write("Enum_Comp:     ");
        if ((PtrGlbNext.EnumComp == 1)) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.writeLine("%d", PtrGlbNext.EnumComp);
        this.output.write("  Int_Comp:    ");
        if ((PtrGlbNext.IntComp == 18)) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.write("%d ", PtrGlbNext.IntComp);
        this.output.write("Str_Comp:      ");
        if ((PtrGlbNext.StringComp == "DHRYSTONE PROGRAM, SOME STRING")) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.writeLine("%s", PtrGlbNext.StringComp);
        this.output.write("Int_1_Loc:     ");
        if ((Check.IntLoc1 == 5)) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.write("%d  ", Check.IntLoc1);
        this.output.write("Int_2_Loc:     ");
        if ((Check.IntLoc2 == 13)) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.writeLine("%d", Check.IntLoc2);
        this.output.write("Int_3_Loc:     ");
        if ((Check.IntLoc3 == 7)) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.write("%d  ", Check.IntLoc3);
        this.output.write("Enum_Loc:      ");
        if ((Check.EnumLoc == 1)) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.writeLine("%d  ", Check.EnumLoc);
        this.output.write("Str_1_Loc:                             ");
        if ((Check.String1Loc == "DHRYSTONE PROGRAM, 1\'ST STRING")) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.writeLine("%s", Check.String1Loc);
        this.output.write("Str_2_Loc:                             ");
        if ((Check.String2Loc == "DHRYSTONE PROGRAM, 2\'ND STRING")) {
            this.output.write("O.K.  ");
        } else {
            this.output.write("WRONG ");
        }

        this.output.writeLine("%s", Check.String2Loc);
        this.output.writeLine();
        this.output.writeLine("Nanoseconds one Dhrystone run: %.2f", (1000000000 / result.Dhrystones));
        this.output.writeLine("Dhrystones per Second:         %.2f", result.Dhrystones);
        this.output.writeLine("VAX  MIPS rating =             %.2f", result.VaxMips);
        this.output.writeLine("");
        result.Output = this.output.Output;
        return result;
    }

    private static final int LOOPS = 20000000;

    private int IntGlob = 0;

    private boolean BoolGlob = false;

    private char Char1Glob = '\\';

    private char Char2Glob = '\\';

    private int[] Array1Glob = new int[50];

    private int[][] Array2Glob = new int[50][50];

    private Record PtrGlbNext;

    private Record PtrGlb;

    static void StructAssign(Record destination, Record source) {
        destination.IntComp = source.IntComp;
        destination.PtrComp = source.PtrComp;
        destination.StringComp = source.StringComp;
        destination.EnumComp = source.EnumComp;
        destination.Discr = source.Discr;
    }

    final DhrystoneResult Proc0(int loops) {
        this.PtrGlbNext = new Record();
        this.PtrGlb = new Record();
        this.PtrGlb.PtrComp = this.PtrGlbNext;
        this.PtrGlb.Discr = Ident_1;
        this.PtrGlb.EnumComp = Ident_3;
        this.PtrGlb.IntComp = 40;
        this.PtrGlb.StringComp = "DHRYSTONE PROGRAM, SOME STRING";
        var String1Loc = "DHRYSTONE PROGRAM, 1\'ST STRING";
        var String2Loc = "DHRYSTONE PROGRAM, 2\'ND STRING";
        this.IntGlob = 0;
        this.BoolGlob = false;
        this.Char1Glob = '\\';
        this.Char2Glob = '\\';
        this.Array1Glob = new int[50];
        this.Array2Glob = new int[50][50];

        this.Array2Glob[8][7] = 10;
        int EnumLoc = 0;
        int IntLoc1 = 0;
        int IntLoc2 = 0;
        int IntLoc3 = 0;
        final long start = System.currentTimeMillis();
        for (var i = 0; (i < loops); i++) {
            this.Proc5();
            this.Proc4();
            IntLoc1 = 2;
            IntLoc2 = 3;
            IntLoc3 = 0;
            String2Loc = "DHRYSTONE PROGRAM, 2\'ND STRING";
            EnumLoc = Ident_2;
            this.BoolGlob = !Dhrystone2.Func2(String1Loc, String2Loc);
            while ((IntLoc1 < IntLoc2)) {
                IntLoc3 = ((5 * IntLoc1) - IntLoc2);
                IntLoc3 = Dhrystone2.Proc7(IntLoc1, IntLoc2);
                IntLoc1++;
            }

            this.Proc8(this.Array1Glob, this.Array2Glob, IntLoc1, IntLoc3);
            this.PtrGlb = this.Proc1(this.PtrGlb);
            var CharIndex = 'A';
            for (CharIndex = 'A'; (CharIndex <= this.Char2Glob); CharIndex++) {
                if ((EnumLoc == Dhrystone2.Func1(CharIndex, 'C'))) {
                    EnumLoc = this.Proc6(Ident_1);
                }

            }

            IntLoc2 = (IntLoc2 * IntLoc1);
            IntLoc1 = (IntLoc2 / IntLoc3);
            IntLoc2 = ((7 * (IntLoc2 - IntLoc3)) - IntLoc1);
            IntLoc1 = this.Proc2(IntLoc1);
        }

        long benchtime = System.currentTimeMillis() - start;
        long loopsPerBenchtime = 0;
        if ((benchtime == 0)) {
            loopsPerBenchtime = 0;
        } else {
            loopsPerBenchtime = (loops / benchtime);
        }

        long dhrystones = (1000 * loopsPerBenchtime);
        Check = new CheckRecord()
        {{
            EnumLoc = EnumLoc;
            IntLoc1 = IntLoc1;
            IntLoc2 = IntLoc2;
            IntLoc3 = IntLoc3;
            String1Loc = String1Loc;
            String2Loc = String2Loc;
        }};
        return new DhrystoneResult()
        {{
            Dhrystones = dhrystones;
            Output = output.Output;
            TimeUsed = benchtime;
            VaxMips = dhrystones / 1757;
        }};
    }

    final Record Proc1(Record PtrValPar) {
        Record NextRecord = this.PtrGlb.PtrComp;
        Dhrystone2.StructAssign(this.PtrGlb.PtrComp, this.PtrGlb);
        PtrValPar.IntComp = 5;
        NextRecord.IntComp = PtrValPar.IntComp;
        NextRecord.PtrComp = PtrValPar.PtrComp;
        NextRecord.PtrComp = this.Proc3(NextRecord.PtrComp);
        if ((NextRecord.Discr == Ident_1)) {
            NextRecord.IntComp = 6;
            NextRecord.EnumComp = this.Proc6(PtrValPar.EnumComp);
            NextRecord.PtrComp = this.PtrGlb.PtrComp;
            NextRecord.IntComp = Dhrystone2.Proc7(NextRecord.IntComp, 10);
        } else {
            Dhrystone2.StructAssign(PtrValPar, PtrValPar.PtrComp);
        }

        return PtrValPar;
    }

    final int Proc2(int IntParIO) {
        var IntLoc = (IntParIO + 10);
        int EnumLoc = Ident_2;
        while (true) {
            if ((this.Char1Glob == 'A')) {
                IntLoc = (IntLoc - 1);
                IntParIO = (IntLoc - this.IntGlob);
                EnumLoc = Ident_1;
            }

            if ((EnumLoc == Ident_1)) {
                break;
            }

        }

        return IntParIO;
    }

    final Record Proc3(Record PtrParOut) {
        if ((this.PtrGlb != null)) {
            PtrParOut = this.PtrGlb.PtrComp;
        } else {
            this.IntGlob = 100;
        }

        this.PtrGlb.IntComp = Dhrystone2.Proc7(10, this.IntGlob);
        return PtrParOut;
    }

    final void Proc4() {
        var BoolLoc = (this.Char1Glob == 'A');
        this.BoolGlob = (BoolLoc || this.BoolGlob);
        this.Char2Glob = 'B';
    }

    final void Proc5() {
        this.Char1Glob = 'A';
        this.BoolGlob = false;
    }

    final int Proc6(int EnumParIn) {
        int EnumParOut = EnumParIn;
        if (!Dhrystone2.Func3(EnumParIn)) {
            EnumParOut = Ident_4;
        }

        switch (EnumParIn) {
            case Ident_1:
                EnumParOut = Ident_1;
                break;
            case Ident_2:
                if ((this.IntGlob > 100)) {
                    EnumParOut = Ident_1;
                } else {
                    EnumParOut = Ident_4;
                }

                break;
            case Ident_3:
                EnumParOut = Ident_2;
                break;
            case Ident_4:
                break;
            case Ident_5:
                EnumParOut = Ident_3;
                break;
        }
        return EnumParOut;
    }

    static int Proc7(int IntParI1, int IntParI2) {
        var IntLoc = (IntParI1 + 2);
        var IntParOut = (IntParI2 + IntLoc);
        return IntParOut;
    }

    final void Proc8(int[] Array1Par, int[][] Array2Par, int IntParI1, int IntParI2) {
        var IntLoc = (IntParI1 + 5);
        Array1Par[IntLoc] = IntParI2;
        Array1Par[(IntLoc + 1)] = Array1Par[IntLoc];
        Array1Par[(IntLoc + 30)] = IntLoc;
        // for IntIndex in range(IntLoc, IntLoc+2):
        for (var IntIndex = IntLoc; (IntIndex < (IntLoc + 2)); IntIndex++) {
            Array2Par[IntLoc][IntIndex] = IntLoc;
        }

        Array2Par[IntLoc][(IntLoc - 1)] = (Array2Par[IntLoc][(IntLoc - 1)] + 1);
        Array2Par[(IntLoc + 20)][IntLoc] = Array1Par[IntLoc];
        this.IntGlob = 5;
    }

    static int Func1(char CharPar1, char CharPar2) {
        char CharLoc1 = CharPar1;
        char CharLoc2 = CharLoc1;
        return CharLoc2 != CharPar2 ? Ident_1 : Ident_2;
    }

    static boolean Func2(String StrParI1, String StrParI2) {
        int IntLoc = 1;
        char CharLoc = '\\';
        while ((IntLoc <= 1)) {
            if ((Dhrystone2.Func1(StrParI1.charAt(IntLoc), StrParI2.charAt((IntLoc + 1))) == Ident_1)) {
                CharLoc = 'A';
                IntLoc = (IntLoc + 1);
            }

        }

        if (((CharLoc >= 'W') && (CharLoc <= 'Z'))) {
            IntLoc = 7;
        }

        if ((CharLoc == 'X')) {
            return true;
        } else if ((StrParI1.compareTo(StrParI2) > 0)) {
            IntLoc = (IntLoc + 7);
            return true;
        } else {
            return false;
        }

    }

    static boolean Func3(int EnumParIn) {
        var EnumLoc = EnumParIn;
        return (EnumLoc == Ident_3);
    }

}