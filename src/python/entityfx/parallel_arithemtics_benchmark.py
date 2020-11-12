from entityfx.arithmetics_base import ArithmeticsBase
from entityfx.writer import Writer

class ParallelArithemticsBenchmark(ArithmeticsBase):

    def __init__(self, writer: Writer, print_to_console : bool=True, is_enabled : bool=True) -> None:
        super().__init__(writer, print_to_console, is_enabled)
        self.is_parallel = True
    
    def benchImplementation(self) -> list:
        return self.bench_in_parallel(lambda : 0, lambda a: self.benchFunc(a), lambda a, r : self.setBenchResultFunc(a, r))

    def benchFunc(self, a):
        r2 = 0
        for i in range(0, self._iterrations):
            r2 = ArithmeticsBase._doArithmetics(i)
        return r2

    def setBenchResultFunc(self, a, r):
        r["Output"] = r