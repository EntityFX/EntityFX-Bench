from entityfx.benchmark_base import BenchmarkBase
from entityfx.writer import Writer

class ParallelIfElseBenchmark(BenchmarkBase):

    def __init__(self, writer: Writer, print_to_console : bool=True, is_enabled : bool=True) -> None:
        super().__init__(writer, print_to_console, is_enabled)
        self._iterrations = 2000000000
        self.ratio = .01
        self.is_parallel = True
    
    def benchImplementation(self) -> list:
        return self.bench_in_parallel(lambda : 0, lambda a: self.benchFunc(a), lambda a, r : None)

    def benchFunc(self, a):
        d = 0
        i = 0; c = -1
        while i < self._iterrations: 
            c = ((-1 if c == (-4) else c))
            if (i == (-1)): 
                d = 3
            elif (i == (-2)): 
                d = 2
            elif (i == (-3)): 
                d = 1
            d = (d + 1)
            i += 1; c -= 1
        return d