
from entityfx.benchmark_base import BenchmarkBase
from entityfx.writer import Writer
import math, time

class CallBenchmarkBase(BenchmarkBase):
    
    def __init__(self, writer: Writer, print_to_console : bool=True, is_enabled : bool=True) -> None:
        super().__init__(writer, print_to_console, is_enabled)
        self._iterrations = 2000000000
        self.ratio = .01
    
    @staticmethod
    def _doCall(i : float, b : float) -> float:
        z = i * .7
        z1 = i * .01
        return z + z1 + .5
    
    def _doCallBench(self) -> int:
        self._beforeBench()
        start = time.time()
        elapsed2 = 0
        i = 0
        a = .0
        i = 0

        while i < self._iterrations: 
            z = a * .7
            z1 = a * .01
            a = (z + z1 + .5)
            i += 1

        elapsed1 = time.time()
        a = .0
        i = 0

        while i < self._iterrations: 
            a = CallBenchmarkBase._doCall(a, .01)
            i += 1

        elapsed2 = time.time()

        if (elapsed2 <= elapsed1): 
            call_time = (elapsed1 - elapsed2)
        else: 
            call_time = (elapsed2 - elapsed1)

        return call_time