
from entityfx.hash_base import HashBase
from entityfx.writer import Writer

class ParallelHashBenchmark(HashBase):  
    def __init__(self, writer : Writer=None, print_to_console : bool=True) -> None:
        super().__init__(writer, print_to_console)
        self.is_parallel = True

    def benchImplementation(self) -> list:
        return self.bench_in_parallel(lambda : 0, lambda a: self.benchFunc(a), lambda a, r : self.setBenchResultFunc(a, r))

    def benchFunc(self, a):
        result = None
        for i in range(0, self._iterrations):
            result = HashBase._doHash(i, self._array_of_bytes)
        return result

    def setBenchResultFunc(self, a, r):
        pass