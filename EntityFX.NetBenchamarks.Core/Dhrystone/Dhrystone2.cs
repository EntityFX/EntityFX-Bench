using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.NetBenchamarks.Core.Dhrystone
{
    class Dhrystone2
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

        Writer output = new Writer();

        public Dhrystone2()
        {
            Record PtrGlb = null;
            Record PtrGlbNext = null;
            for (int i = 0; i < 50; i++)
            {
                Array2Glob[i] = new int[51];
            }
        }

        public DhrystoneResult Bench(int loops = LOOPS)
        {
            output.WriteLine("##########################################");

            output.WriteLine("");
            output.WriteLine("Dhrystone Benchmark, Version 2.1 (Language: C#)");
            output.WriteLine("");

            output.WriteLine("Register option selected\n");
            var result = Proc0(loops);

            output.WriteLine("Nanoseconds one Dhrystone run: {0}", 1000000000 / result.Dhrystones);
            output.WriteLine("Dhrystones per Second:         {0}", result.Dhrystones);
            output.WriteLine("VAX  MIPS rating =             {0}", result.VaxMips);
            output.WriteLine("");
            return result;
        }

        private const int LOOPS = 20000000;

        private int IntGlob = 0;
        private bool BoolGlob = false;
        private char Char1Glob = '\0';
        private char Char2Glob = '\0';
        private int[] Array1Glob = new int[51];
        private int[][] Array2Glob = new int[51][];

        Record PtrGlbNext;
        Record PtrGlb;

        double clock() { return DateTime.Now.Ticks / 1000.0; }

        DhrystoneResult Proc0(int loops)
        {
            for (var i = 0; i < loops; ++i)
                /* nothing*/
                ;


            PtrGlbNext = new Record();
            PtrGlb = new Record();
            PtrGlb.PtrComp = PtrGlbNext;
            PtrGlb.Discr = Ident_1;
            PtrGlb.EnumComp = Ident_3;
            PtrGlb.IntComp = 40;
            PtrGlb.StringComp = "DHRYSTONE PROGRAM, SOME STRING";
            var String1Loc = "DHRYSTONE PROGRAM, 1'ST STRING";
            Array2Glob[8][7] = 10;

            var sw = new Stopwatch();
            sw.Start();

            for (var i = 0; i < loops; ++i)
            {
                Proc5();
                Proc4();
                var IntLoc1 = 2;
                var IntLoc2 = 3;
                int IntLoc3 = 0;
                var String2Loc = "DHRYSTONE PROGRAM, 2'ND STRING";
                int EnumLoc = Ident_2;
                BoolGlob = !Func2(String1Loc, String2Loc);
                while (IntLoc1 < IntLoc2)
                {
                    IntLoc3 = 5 * IntLoc1 - IntLoc2;
                    IntLoc3 = Proc7(IntLoc1, IntLoc2);
                    IntLoc1 = IntLoc1 + 1;


                }
                Proc8(ref Array1Glob, ref Array2Glob, IntLoc1, IntLoc3);
                PtrGlb = Proc1(PtrGlb);
                var CharIndex = 'A';
                //while (CharIndex <= Char2Glob)
                //{
                //    if (EnumLoc == Func1(CharIndex, 'C'))
                //    {
                //        EnumLoc = Proc6(DhrystoneEnum.Ident_1);
                //    }
                //    CharIndex = String.fromCharCode(CharIndex.charCodeAt(0) + 1);

                //}

                for (CharIndex = 'A'; CharIndex <= Char2Glob; ++CharIndex)
                {
                    if (EnumLoc == Func1(CharIndex, 'C'))
                    {
                        EnumLoc = Proc6(Ident_1);
                    }
                }


                IntLoc3 = IntLoc2 * IntLoc1;
                IntLoc2 = IntLoc3 / IntLoc1;
                IntLoc2 = 7 * (IntLoc3 - IntLoc2) - IntLoc1;
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
            return new DhrystoneResult()
            {
                Dhrystones = dhrystones,
                Output = output.ToString(),
                TimeUsed = benchtime,
                VaxMips = dhrystones / 1757
            };
        }

        Record Proc1(Record PtrParIn)
        {
            Record NextRecord = new Record()
            {
                Discr = PtrGlb.Discr,
                EnumComp = PtrGlb.EnumComp,
                StringComp = PtrGlb.StringComp,
                IntComp = PtrGlb.IntComp,
                PtrComp = PtrGlb.PtrComp
            };
            PtrParIn.PtrComp = NextRecord;
            PtrParIn.IntComp = 5;
            NextRecord.IntComp = PtrParIn.IntComp;
            NextRecord.PtrComp = PtrParIn.PtrComp;
            NextRecord.PtrComp = Proc3(NextRecord.PtrComp);
            if (NextRecord.Discr == Ident_1)
            {
                NextRecord.IntComp = 6;
                NextRecord.EnumComp = Proc6(PtrParIn.EnumComp);
                NextRecord.PtrComp = PtrGlb.PtrComp;
                NextRecord.IntComp = Proc7(NextRecord.IntComp, 10);
            }
            else
            {
                PtrParIn = new Record()
                {
                    Discr = NextRecord.Discr,
                    EnumComp = NextRecord.EnumComp,
                    StringComp = NextRecord.StringComp,
                    IntComp = NextRecord.IntComp,
                    PtrComp = NextRecord.PtrComp
                };
            }
            NextRecord.PtrComp = null;
            return PtrParIn;
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
            BoolLoc = BoolLoc || BoolGlob;
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

            if (EnumParIn == Ident_1)
            {
                EnumParOut = Ident_1;
            }
            else if (EnumParIn == Ident_2)
            {
                if (IntGlob > 100)
                {
                    EnumParOut = Ident_1;


                }
                else
                {
                    EnumParOut = Ident_4;



                }
            }
            else if (EnumParIn == Ident_3)
            {
                EnumParOut = Ident_2;
            }
            else if (EnumParIn == Ident_4)
            {
                EnumParOut = 0;
            }
            else if (EnumParIn == Ident_5)
            {
                EnumParOut = Ident_3;
            }
            return EnumParOut;
        }

        int Proc7(int IntParI1, int IntParI2)
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

        int Func1(char CharPar1, char CharPar2)
        {
            var CharLoc1 = CharPar1;
            var CharLoc2 = CharLoc1;
            if (CharLoc2 != CharPar2)
            {
                return Ident_1;
            }
            else
            {
                return Ident_2;
            }
        }

        bool Func2(string StrParI1, string StrParI2)
        {
            var IntLoc = 1;
            char CharLoc = ' ';
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

        bool Func3(int EnumParIn)
        {
            var EnumLoc = EnumParIn;
            if (EnumLoc == Ident_3) return true;
            return false;
        }
    }
}
