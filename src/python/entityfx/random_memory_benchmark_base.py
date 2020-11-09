
from entityfx.benchmark_base import BenchmarkBase
from entityfx.writer import Writer
import random
import math
import time
from statistics import mean
import sys

class RandomMemoryBenchmarkBase(BenchmarkBase):
    
    
    def __init__(self, writer: Writer, print_to_console : bool=True) -> None:
        super().__init__(writer, print_to_console)

        random.seed()
        
        self.__output = Writer()
        self._iterrations = 500000
        self.ratio = 2
        self._use_console(print_to_console)
    
    def warmup(self, aspect : float=.05) -> None:
        self._use_console(False)
        super().warmup(aspect)
        self._use_console(True)
    
    def benchRandomMemory(self) :
        int4k = self._measureArrayRandomRead(1024)
        self._output.write_line("Random int 4k: {0:5.2f} MB/s", int4k[0])
        int512k = self._measureArrayRandomRead(131072)
        self._output.write_line("Random int 512k: {0:5.2f} MB/s", int512k[0])
        int8m = self._measureArrayRandomRead(2097152)
        self._output.write_line("Random int 8m: {0:5.2f} MB/s", int8m[0])

        long4k = self._measureArrayRandomLongRead(512)
        self._output.write_line("Random long 4k: {0:5.2f} MB/s", long4k[0])
        long512k = self._measureArrayRandomLongRead(65536)
        self._output.write_line("Random long 512k: {0:5.2f} MB/s", long512k[0])
        long8m = self._measureArrayRandomLongRead(1048576)
        self._output.write_line("Random long 8m: {0:5.2f} MB/s", long8m[0])

        avg = mean([int4k[0], int512k[0], int8m[0], long4k[0], long512k[0], long8m[0] ])
        
        return { "Average" : avg, "Output" : self.__output.output }
    
    def _measureArrayRandomRead(self, size) :
        i = 0

        array0_ = list(map(lambda x: random.randint(-2147483647, 2147483647), range(0, size)))
        end = len(array0_) - 1

        indexes = list(map(lambda x: random.randint(0, end), range(0, end)))

        k0 = math.floor(size / 1024)
        k1 = 1 if k0 == 0 else k0

        iter_internal = math.floor(self._iterrations / k1)
        iter_internal = 1 if iter_internal == 0 else iter_internal

        idx = 0
        while idx < end: 
            i = (array0_[idx])
            idx += 1

        start = time.time()
        it = 0


        while it < iter_internal:
            for idx in indexes:
                i = (array0_[idx])
            it += 1


        elapsed = time.time() - start
        return (iter_internal * len(array0_) * 4 / elapsed / 1024 / 1024, i)
    
    def _measureArrayRandomLongRead(self, size) :
        i = 0

        array0_ = list(map(lambda x: random.randint(-2147483647, 2147483647), range(0, size)))
        end = len(array0_) - 1

        indexes = list(map(lambda x: random.randint(0, end), range(0, end)))

        k0 = math.floor(size / 1024)
        k1 = 1 if k0 == 0 else k0

        iter_internal = math.floor(self._iterrations / k1)
        iter_internal = 1 if iter_internal == 0 else iter_internal

        idx = 0
        while idx < end: 
            i = (array0_[idx])
            idx += 1

        start = time.time()
        it = 0


        while it < iter_internal:
            for idx in indexes:
                i = (array0_[idx])
            it += 1


        elapsed = time.time() - start
        return (iter_internal * len(array0_) * 4 / elapsed / 1024 / 1024, i)