
from entityfx.benchmark_base import BenchmarkBase
from entityfx.writer import Writer
import random
import math
import time
from statistics import mean

class MemoryBenchmarkBase(BenchmarkBase):
    
    
    def __init__(self, writer: Writer, print_to_console : bool=True) -> None:
        super().__init__(writer, print_to_console)

        random.seed()
        
        self.__output = Writer()
        self._iterrations = 500000
        self.ratio = (1)
        self._use_console(print_to_console)
    
    def warmup(self, aspect : float=.05) -> None:
        self._use_console(False)
        super().warmup(aspect)
        self._use_console(True)
    
    def benchRandomMemory(self) :
        int4k = self._measureArrayRandomRead(1024)
        self._output.write_line("int 4k: {0:5.2f} MB/s", int4k[0])
        int512k = self._measureArrayRandomRead(131072)
        self._output.write_line("int 512k: {0:5.2f} MB/s", int512k[0])
        int8m = self._measureArrayRandomRead(2097152)
        self._output.write_line("int 8m: {0:5.2f} MB/s", int8m[0])
        int32m = self._measureArrayRandomRead(math.floor(32 * 1024 * 1024 / 4))
        self._output.write_line("int 32m: {0:5.2f} MB/s", int32m[0])

        long4k = self._measureArrayRandomLongRead(512)
        self._output.write_line("long 4k: {0:5.2f} MB/s", long4k[0])
        long512k = self._measureArrayRandomLongRead(65536)
        self._output.write_line("long 512k: {0:5.2f} MB/s", long512k[0])
        long8m = self._measureArrayRandomLongRead(1048576)
        self._output.write_line("long 8m: {0:5.2f} MB/s", long8m[0])
        long32m = self._measureArrayRandomRead(math.floor(32 * 1024 * 1024 / 4))
        self._output.write_line("long 32m: {0:5.2f} MB/s", long32m[0])

        avg = mean([int4k[0], int512k[0], int8m[0], int32m[0], long4k[0], long512k[0], long8m[0], long32m[0]])
        
        return { "Average" : avg, "Output" : self.__output.output }
    
    def _measureArrayRandomRead(self, size) :
        block_size = 16
        i = [0] * block_size

        array0_ = list(range(0, size))
        end = len(array0_) - 1
        k0 = math.floor(size / 1024)
        k1 = 1 if k0 == 0 else k0

        iter_internal = math.floor(self._iterrations / k1)
        iter_internal = 1 if iter_internal == 0 else iter_internal

        idx = 0
        while idx < end: 
            i[0] = (array0_[idx])
            i[1] = (array0_[idx + 1])
            i[2] = (array0_[idx + 2])
            i[3] = (array0_[idx + 3])
            i[4] = (array0_[idx + 4])
            i[5] = (array0_[idx + 5])
            i[6] = (array0_[idx + 6])
            i[7] = (array0_[idx + 7])
            i[8] = (array0_[idx + 8])
            i[9] = (array0_[idx + 9])
            i[0xA] = (array0_[idx + 0xA])
            i[0xB] = (array0_[idx + 0xB])
            i[0xC] = (array0_[idx + 0xC])
            i[0xD] = (array0_[idx + 0xD])
            i[0xE] = (array0_[idx + 0xE])
            i[0xF] = (array0_[idx + 0xF])
            idx += block_size

        start = time.time()
        it = 0

        while it < iter_internal: 
            idx = 0
            while idx < end: 
                i[0] = (array0_[idx])
                i[1] = (array0_[idx + 1])
                i[2] = (array0_[idx + 2])
                i[3] = (array0_[idx + 3])
                i[4] = (array0_[idx + 4])
                i[5] = (array0_[idx + 5])
                i[6] = (array0_[idx + 6])
                i[7] = (array0_[idx + 7])
                i[8] = (array0_[idx + 8])
                i[9] = (array0_[idx + 9])
                i[0xA] = (array0_[idx + 0xA])
                i[0xB] = (array0_[idx + 0xB])
                i[0xC] = (array0_[idx + 0xC])
                i[0xD] = (array0_[idx + 0xD])
                i[0xE] = (array0_[idx + 0xE])
                i[0xF] = (array0_[idx + 0xF])
                idx += block_size
            it += 1
        
        elapsed = time.time() - start
        return (iter_internal * len(array0_) * 4 / elapsed / 1024 / 1024, i)
    
    def _measureArrayRandomLongRead(self, size) :
        block_size = 8
        l_ = [0] * block_size

        array0_ = list(range(0, size))
        end = len(array0_) - 1
        k0 = math.floor(size / 1024)
        k1 = 1 if k0 == 0 else k0

        iter_internal = math.floor(self._iterrations / k1)
        iter_internal = 1 if iter_internal == 0 else iter_internal

        idx = 0

        while idx < end: 
            l_[0] = (array0_[idx])
            l_[1] = (array0_[idx + 1])
            l_[2] = (array0_[idx + 2])
            l_[3] = (array0_[idx + 3])
            l_[4] = (array0_[idx + 4])
            l_[5] = (array0_[idx + 5])
            l_[6] = (array0_[idx + 6])
            l_[7] = (array0_[idx + 7])
            idx += block_size

        start = time.time()
        it = 0

        while it < iter_internal: 
            idx = 0
            while idx < end: 
                l_[0] = (array0_[idx])
                l_[1] = (array0_[idx + 1])
                l_[2] = (array0_[idx + 2])
                l_[3] = (array0_[idx + 3])
                l_[4] = (array0_[idx + 4])
                l_[5] = (array0_[idx + 5])
                l_[6] = (array0_[idx + 6])
                l_[7] = (array0_[idx + 7])
                idx += block_size
            it += 1

        elapsed = time.time() - start
        return (iter_internal * len(array0_) * 4 / elapsed / 1024 / 1024, l_)