
from entityfx.benchmark_base import BenchmarkBase
from entityfx.writer import Writer

class LoopsBenchmark(BenchmarkBase):
    
    def __init__(self, writer : Writer=None, print_to_console : bool=True) -> None:
        super().__init__(writer, print_to_console)
        self.__r = 0
        self._iterrations = 2000000000
    
    def benchImplementation(self) -> int:
        i = 0
        i = 0
        while i < self._iterrations: 
            self.__r = i
            i += 1
        i = 0
        while i < self._iterrations:
            i += 1
        return i