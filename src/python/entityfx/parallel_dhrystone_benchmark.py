from entityfx.dhrystone2 import Dhrystone2
from entityfx.benchmark_base import BenchmarkBase
from entityfx.writer import Writer

class ParallelDhrystoneBenchmark(BenchmarkBase):

    def __init__(self, writer: Writer, print_to_console : bool=True) -> None:
        super().__init__(writer, print_to_console)
        self.is_parallel = True
        self.ratio = 4
    
    def benchImplementation(self) -> list:
        return self.bench_in_parallel(lambda : Dhrystone2(False), lambda a: a.bench(), lambda a, r : self.setBenchResultFunc(a, r))

    def warmup(self, aspect : float=.05) -> None:
        pass

    def populateResult(self, bench_result, dhrystone_result : list):
        result = self._buildParallelResult(bench_result, dhrystone_result) 
        result["Result"] = sum(map(lambda x : x["Result"], dhrystone_result))
        result["Units"] = "DMIPS"
        result["Output"] = "".join(map(lambda x : x["Output"], dhrystone_result))
        return result


    def setBenchResultFunc(self, a, r):
        r["Points"] = a["VaxMips"] * self.ratio
        r["Result"] = a["VaxMips"]
        r["Output"] = a["Output"]