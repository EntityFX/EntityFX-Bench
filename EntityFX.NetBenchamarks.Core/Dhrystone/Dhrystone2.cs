using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.NetBenchmark.Core.Dhrystone
{
    public class Dhrystone2
    {

        const int Ident_1 = 0;
        const int Ident_2 = 1;
        const int Ident_3 = 2;
        const int Ident_4 = 3;
        const int Ident_5 = 4;


        class Record
        {
            public Record PtrComp;
            public int Discr;
            public int EnumComp;
            public int IntComp;
            public string StringComp;
        }

        private static CheckRecord Check;

        public class CheckRecord
        {
            public int IntLoc1;
            public int IntLoc2;
            public int IntLoc3;
            public string String1Loc;
            public string String2Loc;
            public int EnumLoc;
        }

        Writer output = new Writer();

        public Dhrystone2(bool printToConsole = true)
        {
            output.UseConsole = printToConsole;
        }

        private static bool IsOptimized(Assembly asm)
        {
            var att = asm.GetCustomAttribute<DebuggableAttribute>();
            return att == null || att.IsJITOptimizerDisabled == false;
        }

        public DhrystoneResult Bench(int loops = LOOPS)
        {
            output.WriteLine("##########################################");

            output.WriteLine("");
            output.WriteLine("Dhrystone Benchmark, Version 2.1 (Language: C#)");
            output.WriteLine("");

            output.WriteLine("Optimization {0}", IsOptimized(GetType().Assembly) ? "Optimised" : "Non-optimised");
            var result = Proc0(loops);

            output.WriteLine("");
            output.WriteLine("Final values (* implementation-dependent):\n");
            output.WriteLine("");
            output.Write("Int_Glob:      ");

            if (IntGlob == 5) output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.Write("{0}  ", IntGlob);

            output.Write("Bool_Glob:     ");
            if (BoolGlob) output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.WriteLine("{0}", BoolGlob);

            output.Write("Ch_1_Glob:     ");
            if (Char1Glob == 'A') output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.Write("{0}  ", Char1Glob);

            output.Write("Ch_2_Glob:     ");
            if (Char2Glob == 'B') output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.WriteLine("{0}", Char2Glob);

            output.Write("Arr_1_Glob[8]: ");
            if (Array1Glob[8] == 7) output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.Write("{0}  ", Array1Glob[8]);

            output.Write("Arr_2_Glob8/7: ");
            if (Array2Glob[8][7] == LOOPS + 10)
                output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.WriteLine("{0}", Array2Glob[8][7]);

            output.Write("Ptr_Glob->            ");
            output.WriteLine("  Ptr_Comp:       *    {0}", PtrGlb.PtrComp != null);

            output.Write("  Discr:       ");
            if (PtrGlb.Discr == 0) output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.Write("{0}  ", PtrGlb.Discr);

            output.Write("Enum_Comp:     ");
            if (PtrGlb.EnumComp == 2)
                output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.WriteLine("{0}", PtrGlb.EnumComp);

            output.Write("  Int_Comp:    ");
            if (PtrGlb.IntComp == 17) output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.Write("{0} ", PtrGlb.IntComp);

            output.Write("Str_Comp:      ");
            if (PtrGlb.StringComp ==
                                 "DHRYSTONE PROGRAM, SOME STRING")
            output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.WriteLine("{0}", PtrGlb.StringComp);

            output.Write("Next_Ptr_Glob->       ");
            output.Write("  Ptr_Comp:       *    {0}", PtrGlbNext.PtrComp != null);
            output.WriteLine(" same as above");

            output.Write("  Discr:       ");
            if (PtrGlbNext.Discr == 0)
                output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.Write("{0}  ", PtrGlbNext.Discr);

            output.Write("Enum_Comp:     ");
            if (PtrGlbNext.EnumComp == 1)
                output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.WriteLine("{0}", PtrGlbNext.EnumComp);

            output.Write("  Int_Comp:    ");
            if (PtrGlbNext.IntComp == 18)
                output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.Write("{0} ", PtrGlbNext.IntComp);

            output.Write("Str_Comp:      ");
            if (PtrGlbNext.StringComp ==
                                 "DHRYSTONE PROGRAM, SOME STRING")
               output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.WriteLine("{0}", PtrGlbNext.StringComp);

            output.Write("Int_1_Loc:     ");
            if (Check.IntLoc1 == 5)
                output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.Write("{0}  ", Check.IntLoc1);

            output.Write("Int_2_Loc:     ");
            if (Check.IntLoc2 == 13)
                output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.WriteLine("{0}", Check.IntLoc2);

            output.Write("Int_3_Loc:     ");
            if (Check.IntLoc3 == 7)
                output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.Write("{0}  ", Check.IntLoc3);

            output.Write("Enum_Loc:      ");
            if (Check.EnumLoc == 1)
                output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.WriteLine("{0}  ", Check.EnumLoc);


            output.Write("Str_1_Loc:                             ");
            if (Check.String1Loc == "DHRYSTONE PROGRAM, 1'ST STRING")
                output.Write("O.K.  ");

            else output.Write("WRONG ");
            output.WriteLine("{0}", Check.String1Loc);

            output.Write("Str_2_Loc:                             ");
            if (Check.String2Loc == "DHRYSTONE PROGRAM, 2'ND STRING")
                output.Write("O.K.  ");
            else output.Write("WRONG ");
            output.WriteLine("{0}", Check.String2Loc);


            output.WriteLine();

            output.WriteLine("Nanoseconds one Dhrystone run: {0}", 1000000000 / result.Dhrystones);
            output.WriteLine("Dhrystones per Second:         {0}", result.Dhrystones);
            output.WriteLine("VAX  MIPS rating =             {0}", result.VaxMips);
            output.WriteLine("");

            result.Output = output.Output;
            return result;
        }

        private const int LOOPS = 20000000;

        private int IntGlob = 0;
        private bool BoolGlob = false;
        private char Char1Glob = '\0';
        private char Char2Glob = '\0';
        private int[] Array1Glob = new int[50];
        private int[][] Array2Glob = new int[50][];

        private Record PtrGlbNext;
        private Record PtrGlb;

        static void StructAssign(Record destination, Record source)
        {
            destination.IntComp = source.IntComp;
            destination.PtrComp = source.PtrComp;
            destination.StringComp = source.StringComp;
            destination.EnumComp = source.EnumComp;
            destination.Discr = source.Discr;
        }

        DhrystoneResult Proc0(int loops)
        {
            PtrGlbNext = new Record();
            PtrGlb = new Record();
            PtrGlb.PtrComp = PtrGlbNext;
            PtrGlb.Discr = Ident_1;
            PtrGlb.EnumComp = Ident_3;
            PtrGlb.IntComp = 40;
            PtrGlb.StringComp = "DHRYSTONE PROGRAM, SOME STRING";
            var String1Loc = "DHRYSTONE PROGRAM, 1'ST STRING";
            var String2Loc = "DHRYSTONE PROGRAM, 2'ND STRING";


            IntGlob = 0;
            BoolGlob = false;
            Char1Glob = '\0';
            Char2Glob = '\0';
            Array1Glob = new int[50];
            Array2Glob = new int[50][];
            for (int i = 0; i < Array2Glob.Length; i++)
            {
                Array2Glob[i] = new int[Array2Glob.Length];
            }
            Array2Glob[8][7] = 10;

            int EnumLoc = 0;
            int IntLoc1 = 0;
            int IntLoc2 = 0;
            int IntLoc3 = 0;

            var sw = new Stopwatch();
            sw.Start();

            for (var i = 0; i < loops; ++i)
            {
                Proc5();
                Proc4();
                IntLoc1 = 2;
                IntLoc2 = 3;
                IntLoc3 = 0;
                String2Loc = "DHRYSTONE PROGRAM, 2'ND STRING";
                EnumLoc = Ident_2;
                BoolGlob = !Func2(String1Loc, String2Loc);
                while (IntLoc1 < IntLoc2)
                {
                    IntLoc3 = 5 * IntLoc1 - IntLoc2;
                    IntLoc3 = Proc7(IntLoc1, IntLoc2);
                    IntLoc1 += 1;


                }
                Proc8(ref Array1Glob, ref Array2Glob, IntLoc1, IntLoc3);
                PtrGlb = Proc1(PtrGlb);
                var CharIndex = 'A';

                for (CharIndex = 'A'; CharIndex <= Char2Glob; ++CharIndex)
                {
                    if (EnumLoc == Func1(CharIndex, 'C'))
                    {
                        EnumLoc = Proc6(Ident_1);
                    }
                }


                IntLoc2 = IntLoc2 * IntLoc1;
                IntLoc1 = IntLoc2 / IntLoc3;
                IntLoc2 = 7 * (IntLoc2 - IntLoc3) - IntLoc1;
                IntLoc1 = Proc2(IntLoc1);
            }

            var benchtime = sw.ElapsedMilliseconds;
            var loopsPerBenchtime = 0.0;
            if (benchtime == 0.0)
            {
                loopsPerBenchtime = 0.0;
            }
            else
            {
                loopsPerBenchtime = (loops / benchtime);
            }
            var dhrystones = 1000 * loopsPerBenchtime;

            Check = new CheckRecord()
            {
                EnumLoc = EnumLoc,
                IntLoc1 = IntLoc1,
                IntLoc2 = IntLoc2,
                IntLoc3 = IntLoc3,
                String1Loc = String1Loc,
                String2Loc = String2Loc
            };

            return new DhrystoneResult()
            {
                Dhrystones = dhrystones,
                Output = output.ToString(),
                TimeUsed = benchtime,
                VaxMips = dhrystones / 1757
            };
        }

        Record Proc1(Record PtrValPar)
        {
            Record NextRecord = PtrGlb.PtrComp;
            StructAssign(PtrGlb.PtrComp, PtrGlb);
            PtrValPar.IntComp = 5;
            NextRecord.IntComp = PtrValPar.IntComp;
            NextRecord.PtrComp = PtrValPar.PtrComp;
            NextRecord.PtrComp = Proc3(NextRecord.PtrComp);
            if (NextRecord.Discr == Ident_1)
            {
                NextRecord.IntComp = 6;
                NextRecord.EnumComp = Proc6(PtrValPar.EnumComp);
                NextRecord.PtrComp = PtrGlb.PtrComp;
                NextRecord.IntComp = Proc7(NextRecord.IntComp, 10);
            }
            else
            {
                StructAssign(PtrValPar, PtrValPar.PtrComp);
            }

            return PtrValPar;
        }

        int Proc2(int IntParIO)
        {
            var IntLoc = IntParIO + 10;
            int EnumLoc = Ident_2;
            while (true)
            {
                if (Char1Glob == 'A')
                {
                    IntLoc = IntLoc - 1;
                    IntParIO = IntLoc - IntGlob;
                    EnumLoc = Ident_1;
                }
                if (EnumLoc == Ident_1)
                    break;
            }
            return IntParIO;
        }

        Record Proc3(Record PtrParOut)
        {
            if (PtrGlb != null)
            {
                PtrParOut = PtrGlb.PtrComp;
            }
            else
            {
                IntGlob = 100;
            }
            PtrGlb.IntComp = Proc7(10, IntGlob);
            return PtrParOut;
        }

        void Proc4()
        {
            var BoolLoc = Char1Glob == 'A';
            BoolGlob = BoolLoc || BoolGlob;
            Char2Glob = 'B';
        }

        void Proc5()
        {
            Char1Glob = 'A';
            BoolGlob = false;
        }

        int Proc6(int EnumParIn)
        {
            int EnumParOut = EnumParIn;
            if (!Func3(EnumParIn))
            {
                EnumParOut = Ident_4;
            }


            switch (EnumParIn)
            {
                case Ident_1:
                    EnumParOut = Ident_1;
                    break;
                case Ident_2:
                    if (IntGlob > 100)
                        EnumParOut = Ident_1;
                    else
                        EnumParOut = Ident_4;
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

        static int Proc7(int IntParI1, int IntParI2)
        {
            var IntLoc = IntParI1 + 2;
            var IntParOut = IntParI2 + IntLoc;
            return IntParOut;
        }

        void Proc8(ref int[] Array1Par, ref int[][] Array2Par, int IntParI1, int IntParI2)
        {
            var IntLoc = IntParI1 + 5;
            Array1Par[IntLoc] = IntParI2;
            Array1Par[IntLoc + 1] = Array1Par[IntLoc];
            Array1Par[IntLoc + 30] = IntLoc;
            // for IntIndex in range(IntLoc, IntLoc+2):
            for (var IntIndex = IntLoc; IntIndex < IntLoc + 2; ++IntIndex)
            {
                Array2Par[IntLoc][IntIndex] = IntLoc;
            }
            Array2Par[IntLoc][IntLoc - 1] = Array2Par[IntLoc][IntLoc - 1] + 1;
            Array2Par[IntLoc + 20][IntLoc] = Array1Par[IntLoc];
            IntGlob = 5;
        }

        static int Func1(char CharPar1, char CharPar2)
        {
            var CharLoc1 = CharPar1;
            var CharLoc2 = CharLoc1;
            return CharLoc2 != CharPar2 ? Ident_1 : Ident_2;
        }

        static bool Func2(string StrParI1, string StrParI2)
        {
            var IntLoc = 1;
            char CharLoc = '\0';
            while (IntLoc <= 1)
            {
                if (Func1(StrParI1[IntLoc], StrParI2[IntLoc + 1]) == Ident_1)
                {
                    CharLoc = 'A';
                    IntLoc = IntLoc + 1;
                }
            }
            if (CharLoc >= 'W' && CharLoc <= 'Z')
            {
                IntLoc = 7;
            }
            if (CharLoc == 'X')
            {
                return true;
            }
            else
            {
                if (StrParI1.CompareTo(StrParI2) > 0)
                {
                    IntLoc = IntLoc + 7;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        static bool Func3(int EnumParIn)
        {
            var EnumLoc = EnumParIn;
            return EnumLoc == Ident_3;
        }
    }
}
