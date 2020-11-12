
from entityfx.benchmark_base import BenchmarkBase
from entityfx.dhrystone2 import Dhrystone2
from entityfx.writer import Writer


class DhrystoneBenchmark(BenchmarkBase):
    
    def __init__(self, writer: Writer, print_to_console : bool=True) -> None:
        super().__init__(writer, print_to_console)
        self.__dhrystone = Dhrystone2()
        self.ratio = 4
    
    def benchImplementation(self):
        return self.__dhrystone.bench()
    
    def populateResult(self, bench_result, dhrystone_result):
        bench_result["Points"] = dhrystone_result["VaxMips"]
        bench_result["Result"] = dhrystone_result["VaxMips"] * self.ratio
        bench_result["Units"] = "DMIPS"
        bench_result["Output"] = dhrystone_result["Output"]
        return bench_result
    
    def warmup(self, aspect : float=.05) -> None:
        pass
