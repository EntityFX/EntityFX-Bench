
from entityfx.writer import Writer
import math
import time

class Whetstone:
    
    def __init__(self) -> None:
        self.__loop_time = [0] * 9
        self.__loop_mops = [0] * 9
        self.__loop_mflops = [0] * 9
        self.__time_used = 0
        self.__mwips = 0
        self.__headings = [""] * 9
        self.__check = 0
        self.__results = [0] * 9
        self.__output = Writer()
    
    def bench(self, getinput : bool=True):
        count = 10
        calibrate = 1
        xtra = 1
        x100 = 100
        duration = 100
        general = [""] * 8
        self.write_line("{0} Precision Python Whetstone Benchmark\n", "Double")
        if (not getinput): 
            self.write_line("No run time input data\n")
        else: 
            self.write_line("With run time input data\n")
        self.write_line("Calibrate")
        first_pass = True
        while first_pass or (count > 0):
            first_pass = False
            self.__time_used = (0)
            self.__whetstones(xtra, x100, calibrate)
            self.write_line("{0:11.2f} Seconds {1:10}   Passes (x 100)", self.__time_used, xtra)
            calibrate = (calibrate + 1)
            count = (count - 1)
            if (self.__time_used > 2.0): 
                count = 0
            else: 
                xtra = (xtra * (5))
        if (self.__time_used > 0): 
            xtra = (math.floor(((((duration) * xtra)) / self.__time_used)))
        if (xtra < (1)): 
            xtra = (1)
        calibrate = 0
        self.write_line("\nUse {0}  passes (x 100)", xtra)
        self.write_line("\n          {0} Precision Python Whetstone Benchmark", "Double")
        self.write_line("\n                  {0}", "??")
        self.write_line("\n                  {0}", "")
        self.write_line("\nLoop content                  Result              MFLOPS " + "     MOPS   Seconds\n")
        self.__time_used = 0
        self.__whetstones(xtra, x100, calibrate)
        self.__output.write("MWIPS            ")
        if (self.__time_used > 0): 
            self.__mwips = ((((xtra)) * ((x100))) / (((10) * (self.__time_used))))
        else: 
            self.__mwips = (0)
        self.__output.write_line("{0:40.3f}{1:19.3f}\n", self.__mwips, self.__time_used)
        if (self.__check == 0): 
            self.write_line("Wrong answer  ")
        return {
                "Output" : self.__output.output,
                "MWIPS" : self.__mwips,
                "TimeUsed" : self.__time_used 
            }
    
    def __whetstones(self, xtra : int, x100 : int, calibrate : int) -> None:
        e1 = [0] * 4
        t = .49999975
        t0 = t
        t1 = .50000025
        t2 = 2
        self.__check = (0)
        n1 = ((12) * x100)
        n2 = ((14) * x100)
        n3 = ((345) * x100)
        n4 = ((210) * x100)
        n5 = ((32) * x100)
        n6 = ((899) * x100)
        n7 = ((616) * x100)
        n8 = ((93) * x100)
        n1mult = (10)
        e1[0] = (1)
        e1[1] = (-1)
        e1[2] = (-1)
        e1[3] = (-1)
        start = time.time()
        timea = (time.time())
        ix = (0)
        while ix < xtra: 
            i = (0)
            while i < (n1 * n1mult): 
                e1[0] = ((((e1[0] + e1[1] + e1[2]) - e1[3])) * t)
                e1[1] = (((((e1[0] + e1[1]) - e1[2]) + e1[3])) * t)
                e1[2] = ((((e1[0] - e1[1]) + e1[2] + e1[3])) * t)
                e1[3] = (((((- e1[0]) + e1[1] + e1[2]) + e1[3])) * t)
                i += 1
            t = ((1) - t)
            ix += 1
        t = t0
        timeb = (time.time() - timea) / (n1mult)
        self.__pout("N1 doubleing point", ((n1 * (16))) * ((xtra)), 1, e1[3], timeb, calibrate, 1)
        timea = time.time()
        ix = (0)
        while ix < xtra: 
            i = (0)
            while i < n2: 
                Whetstone.__pa(e1, t, t2)
                i += 1
            t = ((1) - t)
            ix += 1
        t = t0
        timeb = (time.time() - timea)
        self.__pout("N2 doubleing point", ((n2 * (96))) * ((xtra)), 1, e1[3], timeb, calibrate, 2)
        j = (1)
        timea = time.time()
        ix = (0)
        while ix < xtra: 
            i = (0)
            while i < n3: 
                if (j == (1)): 
                    j = (2)
                else: 
                    j = (3)
                if (j > (2)): 
                    j = (0)
                else: 
                    j = (1)
                if (j < (1)): 
                    j = (1)
                else: 
                    j = (0)
                i += 1
            ix += 1
        timeb = (time.time() - timea)
        self.__pout("N3 if then else  ", ((n3 * (3))) * ((xtra)), 2, (j), timeb, calibrate, 3)
        j = (1)
        k = (2)
        l_ = (3)
        timea = time.time()
        ix = (0)
        while ix < xtra: 
            i = (0)
            while i < n4: 
                j = (j * ((k - j)) * ((l_ - k)))
                k = ((l_ * k) - (((l_ - j)) * k))
                l_ = (((l_ - k)) * ((k + j)))
                e1[l_ - (2)] = (j + k + l_)
                e1[k - (2)] = (j * k * l_)
                i += 1
            ix += 1
        timeb = (time.time() - timea)
        x = (e1[0] + e1[1])
        self.__pout("N4 fixed point   ", ((n4 * (15))) * ((xtra)), 2, x, timeb, calibrate, 4)
        x = (.5)
        y = (.5)
        timea = time.time()
        ix = (0)
        while ix < xtra: 
            i = (1)
            while i < n5: 
                x = ((t * math.atan((t2 * math.sin(x) * math.cos(x)) / (((math.cos(x + y) + math.cos(x - y)) - 1.0)))))
                y = ((t * math.atan((t2 * math.sin(y) * math.cos(y)) / (((math.cos(x + y) + math.cos(x - y)) - 1.0)))))
                i += 1
            t = ((1) - t)
            ix += 1
        t = t0
        timeb = (time.time() - timea)
        self.__pout("N5 sin,cos etc.  ", ((n5 * (26))) * ((xtra)), 2, y, timeb, calibrate, 5)
        x = (1)
        y = (1)
        z = (1)
        timea = time.time()
        ix = (0)
        while ix < xtra: 
            i = (0)
            while i < n6: 
                Whetstone.__p3(x, y, z, t, t1, t2)
                i += 1
            ix += 1
        timeb = (time.time() - timea)
        self.__pout("N6 doubleing point", ((n6 * (6))) * ((xtra)), 1, z, timeb, calibrate, 6)
        j = (0)
        k = (1)
        l_ = (2)
        e1[0] = (1)
        e1[1] = (2)
        e1[2] = (3)
        timea = time.time()
        ix = (0)
        while ix < xtra: 
            i = (0)
            while i < n7: 
                Whetstone.__po(e1, j, k, l_)
                i += 1
            ix += 1
        timeb = (time.time() - timea)
        self.__pout("N7 assignments   ", ((n7 * (3))) * ((xtra)), 2, e1[2], timeb, calibrate, 7)
        x = (.75)
        timea = time.time()
        ix = (0)
        while ix < xtra: 
            i = (0)
            while i < n8: 
                x = (math.sqrt(math.exp(math.log(x) / t1)))
                i += 1
            ix += 1
        timeb = (time.time() - timea)
        self.__pout("N8 exp,sqrt etc. ", ((n8 * (4))) * ((xtra)), 2, x, timeb, calibrate, 8)
    
    @staticmethod
    def __pa(e0_ , t : float, t2 : float) -> None:
        j = (0)
        while j < (6): 
            e0_[0] = ((((e0_[0] + e0_[1] + e0_[2]) - e0_[3])) * t)
            e0_[1] = (((((e0_[0] + e0_[1]) - e0_[2]) + e0_[3])) * t)
            e0_[2] = ((((e0_[0] - e0_[1]) + e0_[2] + e0_[3])) * t)
            e0_[3] = (((((- e0_[0]) + e0_[1] + e0_[2]) + e0_[3])) / t2)
            j += 1
        return
    
    @staticmethod
    def __po(e1 , j : int, k : int, l_ : int) -> None:
        e1[j] = e1[k]
        e1[k] = e1[l_]
        e1[l_] = e1[j]
        return
    
    @staticmethod
    def __p3(x : float, y : float, z : float, t : float, t1 : float, t2 : float) -> None:
        x = y
        y = z
        x = (t * ((x + y)))
        y = (t1 * ((x + y)))
        z = (((x + y)) / t2)
        return
    
    def __pout(self, title : str, ops : float, type0_ : int, checknum : float, time : float, calibrate : int, section : int) -> None:
        self.__check = (self.__check + checknum)
        self.__loop_time[section] = time
        self.__headings[section] = title
        self.__time_used = (self.__time_used + time)
        if (calibrate == 1): 
            self.__results[section] = checknum
        if (calibrate == 0): 
            self.write("{0:<19}{1:24.17f}    ", self.__headings[section], self.__results[section])
            if (type0_ == 1): 
                if (time > 0): 
                    mflops = (ops / (((1000000) * (time))))
                else: 
                    mflops = (0)
                self.__loop_mops[section] = (99999)
                self.__loop_mflops[section] = mflops
                self.write_line("{0:9.3f}           {1:9.3f}", self.__loop_mflops[section], self.__loop_time[section])
            else: 
                if (time > 0): 
                    mops = (ops / (((1000000) * (time))))
                else: 
                    mops = (0)
                self.__loop_mops[section] = mops
                self.__loop_mflops[section] = (0)
                self.write_line("           {0:9.3f}{1:9.3f}", self.__loop_mops[section], self.__loop_time[section])
        return
    
    def write_line(self, text : str, *args ) -> None:
        self.__output.write_line(text, *(args))
    
    def write(self, text : str, *args ) -> None:
        self.__output.write(text, *(args))