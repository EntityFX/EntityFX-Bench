from entityfx.memory_benchmark import MemoryBenchmarkBase
from entityfx.writer import Writer

class ParallelMemoryBenchmark(MemoryBenchmarkBase):

    def __init__(self, writer: Writer, print_to_console : bool=True, is_enabled : bool=True) -> None:
        super().__init__(writer, print_to_console, is_enabled)
        self.is_parallel = True
    
    def benchImplementation(self) -> list:
        return self.bench_in_parallel(lambda : 0, lambda a: self.benchFunc(a), lambda a, r : self.setBenchResultFunc(a, r))

    def benchFunc(self, a):
        return self.benchRandomMemory()

    def setBenchResultFunc(self, a, r):
        r["Result"] = a["Average"]
        r["Output"] = a["Output"]

    def populateResult(self, bench_result, dhrystone_result):
        result = self._buildParallelResult(bench_result, dhrystone_result)
        sum0_ = sum(map(lambda x : x["Result"], dhrystone_result))
        result["Result"] = sum0_
        result["Points"] = sum0_ * self.ratio
        result["Units"] = "MB/s"
        result["Output"] = "".join(map(lambda x : x["Output"], dhrystone_result))
        return result