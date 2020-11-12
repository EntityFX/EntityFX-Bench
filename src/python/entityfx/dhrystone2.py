import time, math
from entityfx.writer import Writer

LOOPS = 2000000

class Dhrystone2:
    
    class Record:
        
        def __init__(self) -> None:
            self.ptr_comp = None
            self.discr = 0
            self.enum_comp = 0
            self.int_comp = 0
            self.string_comp = None
    
    class CheckRecord:
        
        def __init__(self) -> None:
            self.int_loc1 = 0
            self.int_loc2 = 0
            self.int_loc3 = 0
            self.string1loc = None
            self.string2loc = None
            self.enum_loc = 0
        
        @staticmethod
        def _new3(_arg1 : int, _arg2 : int, _arg3 : int, _arg4 : int, _arg5 : str, _arg6 : str) -> 'CheckRecord':
            res = Dhrystone2.CheckRecord()
            res.enum_loc = _arg1
            res.int_loc1 = _arg2
            res.int_loc2 = _arg3
            res.int_loc3 = _arg4
            res.string1loc = _arg5
            res.string2loc = _arg6
            return res
    
    IDENT_1 = 0
    
    IDENT_2 = 1
    
    IDENT_3 = 2
    
    IDENT_4 = 3
    
    IDENT_5 = 4
    
    CHECK = None
    
    def __init__(self, print_to_console : bool=True) -> None:
        self.__output = Writer()
        self.__int_glob = 0
        self.__bool_glob = False
        self.__char1glob = '\0'
        self.__char2glob = '\0'
        self.__array1glob = [0] * 50
        self.__array2glob = [0] * 50
        self.__ptr_glb_next = None
        self.__ptr_glb = None
        self.__output.use_console = print_to_console
    
    @staticmethod
    def __isOptimized() -> bool:
        return False
    
    def bench(self, loops : int=LOOPS) :
        self.__output.write_line("##########################################")  
        self.__output.write_line("")  
        self.__output.write_line("Dhrystone Benchmark, Version 2.1 (Language: Python)")  
        self.__output.write_line("")  
        self.__output.write_line("Optimization {0}", ("Optimised" if Dhrystone2.__isOptimized() else "Non-optimised"))  
        result = self.__proc0(loops)
        self.__output.write_line("")  
        self.__output.write_line("Final values (* implementation-dependent):\n")  
        self.__output.write_line("")  
        self.__output.write("Int_Glob:      ")  
        if (self.__int_glob == 5): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write("{0}  ", self.__int_glob)  
        self.__output.write("Bool_Glob:     ")  
        if (self.__bool_glob): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write_line("{0}", self.__bool_glob)  
        self.__output.write("Ch_1_Glob:     ")  
        if (self.__char1glob == 'A'): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write("{0}  ", self.__char1glob)  
        self.__output.write("Ch_2_Glob:     ")  
        if (self.__char2glob == 'B'): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write_line("{0}", self.__char2glob)  
        self.__output.write("Arr_1_Glob[8]: ")  
        if (self.__array1glob[8] == 7): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write("{0}  ", self.__array1glob[8])  
        self.__output.write("Arr_2_Glob8/7: ")  
        if (self.__array2glob[8][7] == (LOOPS + 10)): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write_line("{0}", self.__array2glob[8][7])  
        self.__output.write("Ptr_Glob->            ")  
        self.__output.write_line("  Ptr_Comp:       *    {0}", self.__ptr_glb.ptr_comp is not None)  
        self.__output.write("  Discr:       ")  
        if (self.__ptr_glb.discr == 0): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write("{0}  ", self.__ptr_glb.discr)  
        self.__output.write("Enum_Comp:     ")  
        if (self.__ptr_glb.enum_comp == 2): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write_line("{0}", self.__ptr_glb.enum_comp)  
        self.__output.write("  Int_Comp:    ")  
        if (self.__ptr_glb.int_comp == 17): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write("{0} ", self.__ptr_glb.int_comp)  
        self.__output.write("Str_Comp:      ")  
        if (self.__ptr_glb.string_comp == "DHRYSTONE PROGRAM, SOME STRING"): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write_line("{0}", self.__ptr_glb.string_comp)  
        self.__output.write("Next_Ptr_Glob->       ")  
        self.__output.write("  Ptr_Comp:       *    {0}", self.__ptr_glb_next.ptr_comp is not None)  
        self.__output.write_line(" same as above")  
        self.__output.write("  Discr:       ")  
        if (self.__ptr_glb_next.discr == 0): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write("{0}  ", self.__ptr_glb_next.discr)  
        self.__output.write("Enum_Comp:     ")  
        if (self.__ptr_glb_next.enum_comp == 1): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write_line("{0}", self.__ptr_glb_next.enum_comp)  
        self.__output.write("  Int_Comp:    ")  
        if (self.__ptr_glb_next.int_comp == 18): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write("{0} ", self.__ptr_glb_next.int_comp)  
        self.__output.write("Str_Comp:      ")  
        if (self.__ptr_glb_next.string_comp == "DHRYSTONE PROGRAM, SOME STRING"): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write_line("{0}", self.__ptr_glb_next.string_comp)  
        self.__output.write("Int_1_Loc:     ")  
        if (Dhrystone2.CHECK["int_loc1"] == 5): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write("{0}  ", Dhrystone2.CHECK["int_loc1"])  
        self.__output.write("Int_2_Loc:     ")  
        if (Dhrystone2.CHECK["int_loc2"] == 13): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write_line("{0}", Dhrystone2.CHECK["int_loc2"])  
        self.__output.write("Int_3_Loc:     ")  
        if (Dhrystone2.CHECK["int_loc3"] == 7): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write("{0}  ", Dhrystone2.CHECK["int_loc3"])  
        self.__output.write("Enum_Loc:      ")  
        if (Dhrystone2.CHECK["enum_loc"] == 1): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write_line("{0}  ", Dhrystone2.CHECK["enum_loc"])  
        self.__output.write("Str_1_Loc:                             ")  
        if (Dhrystone2.CHECK["string1_loc"] == "DHRYSTONE PROGRAM, 1'ST STRING"): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write_line("{0}", Dhrystone2.CHECK["string1_loc"])  
        self.__output.write("Str_2_Loc:                             ")  
        if (Dhrystone2.CHECK["string2_loc"]== "DHRYSTONE PROGRAM, 2'ND STRING"): 
            self.__output.write("O.K.  ")  
        else: 
            self.__output.write("WRONG ")  
        self.__output.write_line("{0}", Dhrystone2.CHECK["string2_loc"])  
        self.__output.write_line()  
        self.__output.write_line("Nanoseconds one Dhrystone run: {0}", (1000000000) / result["Dhrystones"])  
        self.__output.write_line("Dhrystones per Second:         {0}", result["Dhrystones"])  
        self.__output.write_line("VAX  MIPS rating =             {0}", result["VaxMips"])  
        self.__output.write_line("")  
        result["Output"] = self.__output.output
        return result
    
    @staticmethod
    def __structAssign(destination : 'Record', source : 'Record') -> None:
        destination.int_comp = source.int_comp
        destination.ptr_comp = source.ptr_comp
        destination.string_comp = source.string_comp
        destination.enum_comp = source.enum_comp
        destination.discr = source.discr
    
    def __proc0(self, loops : int) -> 'DhrystoneResult':
        self.__ptr_glb_next = Dhrystone2.Record()
        self.__ptr_glb = Dhrystone2.Record()
        self.__ptr_glb.ptr_comp = self.__ptr_glb_next
        self.__ptr_glb.discr = Dhrystone2.IDENT_1
        self.__ptr_glb.enum_comp = Dhrystone2.IDENT_3
        self.__ptr_glb.int_comp = 40
        self.__ptr_glb.string_comp = "DHRYSTONE PROGRAM, SOME STRING"
        string1loc = "DHRYSTONE PROGRAM, 1'ST STRING"
        string2loc = "DHRYSTONE PROGRAM, 2'ND STRING"
        self.__int_glob = 0
        self.__bool_glob = False
        self.__char1glob = '\0'
        self.__char2glob = '\0'
        self.__array1glob = [0] * 50
        self.__array2glob = [0] * 50
        i = 0
        while i < len(self.__array2glob): 
            self.__array2glob[i] = [0] * 50
            i += 1
        self.__array2glob[8][7] = 10
        enum_loc = 0
        int_loc1 = 0
        int_loc2 = 0
        int_loc3 = 0
        start = time.time()
        i = 0
        while i < loops: 
            self.__proc5()
            self.__proc4()
            int_loc1 = 2
            int_loc2 = 3
            int_loc3 = 0
            string2loc = "DHRYSTONE PROGRAM, 2'ND STRING"
            enum_loc = Dhrystone2.IDENT_2
            self.__bool_glob = not Dhrystone2.__func2(string1loc, string2loc)
            while int_loc1 < int_loc2:
                int_loc3 = ((5 * int_loc1) - int_loc2)
                int_loc3 = Dhrystone2.__proc7(int_loc1, int_loc2)
                int_loc1 += 1
            self.__proc8(self.__array1glob, self.__array2glob, int_loc1, int_loc3)
            self.__ptr_glb = self.__proc1(self.__ptr_glb)
            char_index = 'A'

            char_index_ord = ord(char_index)
            char2glob_ord = ord(self.__char2glob)

            while char_index_ord <= char2glob_ord: 
                if (enum_loc == Dhrystone2.__func1(char_index, 'C')): 
                    enum_loc = self.__proc6(Dhrystone2.IDENT_1)
                char_index_ord += 1

            int_loc2 = (int_loc2 * int_loc1)
            int_loc1 = (math.floor(int_loc2 / int_loc3))
            int_loc2 = ((7 * ((int_loc2 - int_loc3))) - int_loc1)
            int_loc1 = self.__proc2(int_loc1)
            i += 1

        benchtime = (time.time() - start) * 1000
        loops_per_benchtime = .0
        if (benchtime == .0): 
            loops_per_benchtime = .0
        else: 
            loops_per_benchtime = ((math.floor((loops) / benchtime)))
        dhrystones = (1000) * loops_per_benchtime

        Dhrystone2.CHECK = {
            "enum_loc" : enum_loc,
            "int_loc1" : int_loc1,
            "int_loc2" : int_loc2,
            "int_loc3" : int_loc3,
            "string1_loc" : string1loc,
            "string2_loc" : string2loc
        }

        return {
            "Output" : self.__output.output,
            "Dhrystones" : dhrystones,
            "TimeUsed" : benchtime,
            "VaxMips" : dhrystones / 1757
        }
    
    def __proc1(self, ptr_val_par : 'Record') -> 'Record':
        next_record = self.__ptr_glb.ptr_comp
        Dhrystone2.__structAssign(self.__ptr_glb.ptr_comp, self.__ptr_glb)
        ptr_val_par.int_comp = 5
        next_record.int_comp = ptr_val_par.int_comp
        next_record.ptr_comp = ptr_val_par.ptr_comp
        next_record.ptr_comp = self.__proc3(next_record.ptr_comp)
        if (next_record.discr == Dhrystone2.IDENT_1): 
            next_record.int_comp = 6
            next_record.enum_comp = self.__proc6(ptr_val_par.enum_comp)
            next_record.ptr_comp = self.__ptr_glb.ptr_comp
            next_record.int_comp = Dhrystone2.__proc7(next_record.int_comp, 10)
        else: 
            Dhrystone2.__structAssign(ptr_val_par, ptr_val_par.ptr_comp)
        return ptr_val_par
    
    def __proc2(self, int_pario : int) -> int:
        int_loc = int_pario + 10
        enum_loc = Dhrystone2.IDENT_2
        while True:
            if (self.__char1glob == 'A'): 
                int_loc = (int_loc - 1)
                int_pario = (int_loc - self.__int_glob)
                enum_loc = Dhrystone2.IDENT_1
            if (enum_loc == Dhrystone2.IDENT_1): 
                break
        return int_pario
    
    def __proc3(self, ptr_par_out : 'Record') -> 'Record':
        if (self.__ptr_glb is not None): 
            ptr_par_out = self.__ptr_glb.ptr_comp
        else: 
            self.__int_glob = 100
        self.__ptr_glb.int_comp = Dhrystone2.__proc7(10, self.__int_glob)
        return ptr_par_out
    
    def __proc4(self) -> None:
        bool_loc = self.__char1glob == 'A'
        self.__bool_glob = bool_loc or self.__bool_glob
        self.__char2glob = 'B'
    
    def __proc5(self) -> None:
        self.__char1glob = 'A'
        self.__bool_glob = False
    
    def __proc6(self, enum_par_in : int) -> int:
        enum_par_out = enum_par_in
        if (not Dhrystone2.__func3(enum_par_in)): 
            enum_par_out = Dhrystone2.IDENT_4
        swichVal = enum_par_in
        if (swichVal == Dhrystone2.IDENT_1): 
            enum_par_out = Dhrystone2.IDENT_1
        elif (swichVal == Dhrystone2.IDENT_2): 
            if (self.__int_glob > 100): 
                enum_par_out = Dhrystone2.IDENT_1
            else: 
                enum_par_out = Dhrystone2.IDENT_4
        elif (swichVal == Dhrystone2.IDENT_3): 
            enum_par_out = Dhrystone2.IDENT_2
        elif (swichVal == Dhrystone2.IDENT_4): 
            pass
        elif (swichVal == Dhrystone2.IDENT_5): 
            enum_par_out = Dhrystone2.IDENT_3
        return enum_par_out
    
    @staticmethod
    def __proc7(int_pari1 : int, int_pari2 : int) -> int:
        int_loc = int_pari1 + 2
        int_par_out = int_pari2 + int_loc
        return int_par_out
    
    def __proc8(self, array1par, array2par, int_pari1 : int, int_pari2 : int) -> None:
        int_loc = int_pari1 + 5
        array1par[int_loc] = int_pari2
        array1par[int_loc + 1] = array1par[int_loc]
        array1par[int_loc + 30] = int_loc
        int_index = int_loc
        while int_index < (int_loc + 2): 
            array2par[int_loc][int_index] = int_loc
            int_index += 1
        array2par[int_loc][int_loc - 1] = (array2par[int_loc][int_loc - 1] + 1)
        array2par[int_loc + 20][int_loc] = array1par[int_loc]
        self.__int_glob = 5
    
    @staticmethod
    def __func1(char_par1 : 'char', char_par2 : 'char') -> int:
        char_loc1 = char_par1
        char_loc2 = char_loc1
        return (Dhrystone2.IDENT_1 if char_loc2 != char_par2 else Dhrystone2.IDENT_2)
    
    @staticmethod
    def __func2(str_pari1 : str, str_pari2 : str) -> bool:
        int_loc = 1
        char_loc = '\0'
        while int_loc <= 1:
            if (Dhrystone2.__func1(str_pari1[int_loc], str_pari2[int_loc + 1]) == Dhrystone2.IDENT_1): 
                char_loc = 'A'
                int_loc = (int_loc + 1)
        if (char_loc >= 'W' and char_loc <= 'Z'): 
            int_loc = 7
        if (char_loc == 'X'): 
            return True
        elif (Dhrystone2.compare(str_pari1, str_pari2) > 0): 
            int_loc = (int_loc + 7)
            return True
        else: 
            return False
    
    @staticmethod
    def __func3(enum_par_in : int) -> bool:
        enum_loc = enum_par_in
        return enum_loc == Dhrystone2.IDENT_3

    @staticmethod
    def compare(a, b):
        return ((a > b) - (a < b))