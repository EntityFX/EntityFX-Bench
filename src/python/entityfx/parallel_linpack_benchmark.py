from entityfx.linpack import Linpack
from entityfx.benchmark_base import BenchmarkBase
from entityfx.writer import Writer

class ParallelLinpackBenchmark(BenchmarkBase):

    def __init__(self, writer: Writer, print_to_console : bool=True, is_enabled : bool=True) -> None:
        super().__init__(writer, print_to_console, is_enabled)
        self.is_parallel = True
        self.ratio = 10
    
    def benchImplementation(self) -> list:
        return self.bench_in_parallel(lambda : Linpack(False), lambda a: a.bench(1000), lambda a, r : self.setBenchResultFunc(a, r))

    def warmup(self, aspect : float=.05) -> None:
        pass

    def populateResult(self, bench_result, results : list):
        result = self._buildParallelResult(bench_result, results) 
        result["Result"] = sum(map(lambda x : x["Result"], results))
        result["Units"] = "MFLOPS"
        result["Output"] = "".join(map(lambda x : x["Output"], results))
        return result

    def setBenchResultFunc(self, a, r):
        r["Points"] = a["MFLOPS"] * self.ratio
        r["Result"] = a["MFLOPS"]
        r["Output"] = a["Output"]